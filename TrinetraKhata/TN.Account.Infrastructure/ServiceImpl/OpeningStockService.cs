using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TN.Account.Application.Account.Queries.OpeningStockByCompanyId;
using TN.Account.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.IRepository;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class OpeningStockService:IOpeningStockService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGetUserScopedData _getUserScopedData;
        public OpeningStockService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IGetUserScopedData getUserScopedData)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _getUserScopedData = getUserScopedData;


        }

        public async Task<Result<GetOpeningStockQueryResponse>> GetOpeningStock(string companyId, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _unitOfWork.BaseRepository<Items>().GetConditionalAsync(
                    x => x.CompanyId == companyId &&
                         x.OpeningStockQuantity > 0 &&
                         x.CostPrice != null
                    
                );

                var result = new List<OpeningStockItemDto>();
                decimal totalOpeningStock = 0;

                foreach (var item in items)
                {
                    var quantity = item.OpeningStockQuantity ?? 0;
                    var costPrice = Convert.ToDecimal(item.CostPrice);
                    var totalValue = quantity * costPrice;

                    result.Add(new OpeningStockItemDto(
                        ItemName: item.Name ?? "Unnamed Item",
                        Quantity: quantity,
                        CostPrice: costPrice,
                        TotalValue: totalValue
                    ));

                    totalOpeningStock += totalValue;
                }

                var response = new GetOpeningStockQueryResponse(
                    TotalOpeningStockValue: totalOpeningStock,
                    stockItems: result
                );

                return Result<GetOpeningStockQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<GetOpeningStockQueryResponse>.Failure($"Error calculating opening stock: {ex.Message}");
            }
        }
    }
}
