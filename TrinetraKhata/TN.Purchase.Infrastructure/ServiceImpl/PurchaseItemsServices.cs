using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Events.StockUpdated;
using TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems;
using TN.Purchase.Application.ServiceInterface;
using TN.Purchase.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Purchase.Infrastructure.ServiceImpl
{
    public class PurchaseItemsServices : IPurchaseItemsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public PurchaseItemsServices(IMediator mediator,IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _mediator = mediator;
        }



        public async Task<Result<AddPurchaseItemsResponse>> Add(AddPurchaseItemsCommand addPurchaseItemsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();

                    var purchaseItemsData = new PurchaseItems(
                        newId,
                        addPurchaseItemsCommand.quantity,
                        addPurchaseItemsCommand.unitId,
                        addPurchaseItemsCommand.itemId,
                        addPurchaseItemsCommand.price,
                        addPurchaseItemsCommand.amount,
                        userId,
                        DateTime.Now.ToString(),
                        "",
                        "",
                        false,
                        ""
                        //addPurchaseItemsCommand.purchaseDetailsId

                    );


                    var stockToBeUpdated = await _unitOfWork.BaseRepository<Items>().GetByGuIdAsync(addPurchaseItemsCommand.itemId);
                    if (stockToBeUpdated == null)
                    {
                        return Result<AddPurchaseItemsResponse>.Failure("Item not found.");
                    }
                    stockToBeUpdated.OpeningStockQuantity += addPurchaseItemsCommand.quantity;
                    _unitOfWork.BaseRepository<Items>().Update(stockToBeUpdated);


                    await _unitOfWork.BaseRepository<PurchaseItems>().AddAsync(purchaseItemsData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    // Fire stock updated event (for future extensions)
                    await _mediator.Publish(new StockUpdatedEvent(addPurchaseItemsCommand.itemId, Convert.ToDouble(addPurchaseItemsCommand.quantity)));

                    var resultDTOs = _mapper.Map<AddPurchaseItemsResponse>(purchaseItemsData);
                    return Result<AddPurchaseItemsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding purchase items ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>> GetAllPurchaseItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var purchaseItems = await _unitOfWork.BaseRepository<PurchaseItems>().GetAllAsyncWithPagination();
                var purchaseItemsPagedResult = await purchaseItems.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allPurchaseItemsResponse = _mapper.Map<PagedResult<GetAllPurchaseItemsByQueryResponse>>(purchaseItemsPagedResult.Data);

                return Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>.Success(allPurchaseItemsResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all purchase Items", ex);
            }
        }
    }
}