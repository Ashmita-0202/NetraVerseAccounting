using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Application.Payment.Command.AddPayment;
using NV.Payment.Application.Payment.Command.UpdatePayment;
using NV.Payment.Application.Payment.Queries.FilterPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPaymentMethodById;
using NV.Payment.Application.ServiceInterface;
using NV.Payment.Domain.Entities;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace NV.Payment.Infrastructure.ServiceImpl
{
    public class PaymentMethodService:IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;

        public PaymentMethodService(IUnitOfWork unitOfWork,IMapper mapper, IGetUserScopedData getUserScopedData, IDateConvertHelper dateConvertHelper, ITokenService tokenService) 
        {
            _unitOfWork=unitOfWork;
            _getUserScopedData = getUserScopedData;
            _mapper =mapper;
            _tokenService = tokenService;
            _dateConvertHelper = dateConvertHelper;
        }

        public async Task<Result<AddPaymentMethodResponse>> Add(AddPaymentMethodCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string newId = Guid.NewGuid().ToString();
                    string companyId = _tokenService.CompanyId().FirstOrDefault();
                    var paymentMethodData = new PaymentMethod
                   (
                        newId,
                        command.name,
                        command.subLedgerGroupsId,
                        //command.type,
                        companyId,
                           userId,
                       DateTime.UtcNow,
                       "",
                       default,
                       command.isChequeNo,
                          command.isBankName,
                          command.isAccountName,
                            command.isChequeDate,
                            command.isCardNumber,
                            command.isCardHolderName,
                            command.isExpiryDate

                    );

                    await _unitOfWork.BaseRepository<PaymentMethod>().AddAsync(paymentMethodData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddPaymentMethodResponse>(paymentMethodData);
                    return Result<AddPaymentMethodResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding PaymentMethod ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(id);
                if (paymentMethod is null)
                {
                    return Result<bool>.Failure("NotFound", "payment Method Cannot be Found");
                }

                _unitOfWork.BaseRepository<PaymentMethod>().Delete(paymentMethod);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting paymentMethod having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllPaymentMethodQueryResponse>>> GetAllPaymentMethod(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var (paymentMethods, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PaymentMethod>();

                var filterPaymentsMethods = isSuperAdmin ? paymentMethods : paymentMethods.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filterPaymentsMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                        .FindBy(x => companyIds.Contains(x.CompanyId));
                }

                var paymentPagedResult = await filterPaymentsMethods.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allPaymentMethodResponse = _mapper.Map<PagedResult<GetAllPaymentMethodQueryResponse>>(paymentPagedResult.Data);
                return Result<PagedResult<GetAllPaymentMethodQueryResponse>>.Success(allPaymentMethodResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all PaymentMethod ", ex);
            }

        }

        public async Task<Result<GetPaymentMethodByIdQueryResponse>> GetPaymentMethodById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(id);

                var paymentMethodResponse = _mapper.Map<GetPaymentMethodByIdQueryResponse>(paymentMethod);

                var updatedPaymentMethod = paymentMethod is not null
                    ? paymentMethodResponse with
                    {
                        isChequeNo = paymentMethod.IsChequeNo,
                        isBankName = paymentMethod.IsBankName,
                        isAccountName = paymentMethod.IsAccountName,
                        isChequeDate = paymentMethod.IsChequeDate,
                        isCardNumber = paymentMethod.IsCardNumber,
                        isCardHolderName = paymentMethod.IsCardHolderName,
                        isExpiryDate = paymentMethod.IsExpiryDate
                    } : paymentMethodResponse;

                return Result<GetPaymentMethodByIdQueryResponse>.Success(updatedPaymentMethod);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Payment Method by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<GetFilterPaymentMethodResponse>>> GetPaymentMethodFilter(PaginationRequest paginationRequest, FilterPaymentMethodDto filterPaymentMethodDto)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PaymentMethod>();

                var filterItems = isSuperAdmin ? ledger : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );

                DateTime startEnglishDate = filterPaymentMethodDto.startDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterPaymentMethodDto.startDate);
                DateTime endEnglishDate = filterPaymentMethodDto.endDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterPaymentMethodDto.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                
                var userId = _tokenService.GetUserId();

                var filterPaymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>().GetConditionalAsync(
                     x =>
                            x.CreatedBy == userId &&
                         (string.IsNullOrEmpty(filterPaymentMethodDto.name) || x.Name.Contains(filterPaymentMethodDto.name))
                         &&
                         (filterPaymentMethodDto.startDate == default || x.CreatedAt >= startEnglishDate) &&
                         (filterPaymentMethodDto.endDate == default || x.CreatedAt <= endEnglishDate)
                 );


                var paymentMethodResponse = filterPaymentMethods.Select(p => new GetFilterPaymentMethodResponse(
                  
                  p.Id,
                  p.Name,
                  p.SubLedgerGroupsId

                )).ToList();

                PagedResult<GetFilterPaymentMethodResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = paymentMethodResponse.Count();

                    var pagedItems = paymentMethodResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterPaymentMethodResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterPaymentMethodResponse>
                    {
                        Items = paymentMethodResponse.ToList(),
                        TotalItems = paymentMethodResponse.Count(),
                        PageIndex = 1,
                        pageSize = paymentMethodResponse.Count() // all items in one page
                    };
                }
                return Result<PagedResult<GetFilterPaymentMethodResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Payment Method by date: {ex.Message}");
            }
        }

        public async Task<Result<UpdatePaymentMethodResponse>> Update(string id, UpdatePaymentMethodCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    if (id == null)
                    {
                        return Result<UpdatePaymentMethodResponse>.Failure("NotFound", "Please provide valid paymentmethod id");
                    }

                    var paymentToBeUpdated = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(id);
                    if (paymentToBeUpdated is null)
                    {
                        return Result<UpdatePaymentMethodResponse>.Failure("NotFound", "Payment Method are not Found");
                    }

                    paymentToBeUpdated.ModifiedAt = DateTime.UtcNow;
                    paymentToBeUpdated.ModifiedBy = userId;

                    _mapper.Map(command, paymentToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdatePaymentMethodResponse
                        (
                            command.name,
                            command.subLedgerGroupsId
                    

                        );

                    return Result<UpdatePaymentMethodResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating Payment method", ex);
                }
            }
        }
    }
}
