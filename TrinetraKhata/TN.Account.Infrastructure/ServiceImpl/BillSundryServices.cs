using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.BillSundry;
using TN.Account.Application.Account.Command.UpdateBillSundry;
using TN.Account.Application.Account.Queries.FilterSundryBill;
using TN.Account.Application.Account.Queries.GetBillSundry;
using TN.Account.Application.Account.Queries.GetBillSundryById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Shared.Application.Shared.Command.CalculationBillSundry;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;



namespace TN.Account.Infrastructure.ServiceImpl
{
    public class BillSundryServices : IBillSundryServices
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;    
        private readonly IGetUserScopedData _getUserScopedData;

        private readonly IDateConvertHelper _dateConvertHelper;

        public BillSundryServices(ITokenService tokenService,IUnitOfWork unitOfWork, IMapper mapper,IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _getUserScopedData = getUserScopedData;
            _dateConvertHelper = dateConvertHelper;
        }
        public async Task<Result<AddBillSundryResponse>> Add(AddBillSundryCommand addBillSundryCommand)
        {

            var userId = _tokenService.GetUserId();
            var companyId = _tokenService.CompanyId().FirstOrDefault();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var billSundry = new BillSundry
                   (
                        newId,
                        addBillSundryCommand.name,
                        addBillSundryCommand.billType,
                        addBillSundryCommand.defaultValue ?? 0,
                        addBillSundryCommand.BillSundryNature,
                        addBillSundryCommand.isCOGSAffected,
                        addBillSundryCommand.isCOGPAffected,
                        addBillSundryCommand.isCOGSTAffected,
                        addBillSundryCommand.isSalesAccountingAffected,
                        addBillSundryCommand.isPurchaseAccountingAffected,
                        addBillSundryCommand.isSalesAmountAdjusted,
                        addBillSundryCommand.isPurchaseAmountAdjusted,
                        addBillSundryCommand.CustomerAmountAdjusted,
                        addBillSundryCommand.VendorAmountAdjusted,
                        addBillSundryCommand.salesAdjustedLedgerId,
                        addBillSundryCommand.customerAdjustedLedgerId,
                        addBillSundryCommand.purchaseAdjustedLedgerId,
                        addBillSundryCommand.vendorAdjustedLedgerId,
                        addBillSundryCommand.calculationType,
                        addBillSundryCommand.calculationTypeOf,
                        companyId,
                        userId,
                        DateTime.Now,
                        true


                    );

                    await _unitOfWork.BaseRepository<BillSundry>().AddAsync(billSundry);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddBillSundryResponse>(billSundry);
                    return Result<AddBillSundryResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Bill Sundry ", ex);
                }
            }
        }

        public async Task<Result<CalculationBillResponseDTOs>> CalculateBillSundry(CalculationBIllDTOs  calculationBIllDTOs)
        {
            try
            {
                var configurationBillSundry = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(calculationBIllDTOs.billSundryId);
                if (configurationBillSundry is null)
                {
                    return Result<CalculationBillResponseDTOs>.Failure("NotFound", "billSundry Cannot be Found");
                }

                decimal baseAmount = configurationBillSundry.CalculationTypeOf switch
                {
                    CalculationTypeOf.SubTotalAmount => calculationBIllDTOs.subTotalAmount ?? 0,
                    CalculationTypeOf.TaxableAmount => calculationBIllDTOs.taxableAmount?? 0,
                    CalculationTypeOf.AmountAfterVAT => calculationBIllDTOs.amountAfterVat ?? 0,
                    _ => calculationBIllDTOs.subTotalAmount ?? 0
                };




                
                decimal amount = configurationBillSundry.CalculationType == CalculationType.Percentage
                    ? (configurationBillSundry.DefaultValue ?? 0 / 100m) * baseAmount
                    : configurationBillSundry.DefaultValue ?? 0;

                amount = configurationBillSundry.BillType == BillSundryType.Subtractive ? -amount : amount;





       
              
                decimal costOfGoodSoldInSalesAmount = 0;
                decimal costOfGoodSoldInPurchaseAmount = 0;
                decimal costOfGoodsInStockTransferAmount = 0;

                // Update based on conditions
                if (configurationBillSundry.IsCOGSAffected)
                {
                    costOfGoodSoldInSalesAmount += amount;
                }

                if (configurationBillSundry.IsCOGPAffected)
                {
                    costOfGoodSoldInPurchaseAmount += amount;
                }

                if (configurationBillSundry.IsCOGSTAffected)
                {
                    costOfGoodsInStockTransferAmount += amount;
                }

                var journalDetails = new List<JournalEntryDetails>();
      
           
                //if(!calculationBIllDTOs.isPurchaseTransaction && configurationBillSundry.IsSalesAccountingAffected)
                //{
                //    if(configurationBillSundry.IsSalesAmountAdjusted)
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //       Guid.NewGuid().ToString(),
                //       calculationBIllDTOs.newJournalId,
                //       LedgerConstants.SalesLedgerId,
                //       0,
                //       amount,
                //       DateTime.Now,
                //       "",
                //       ""
                //        ));

                //    }
                //    else
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //      Guid.NewGuid().ToString(),
                //      calculationBIllDTOs.newJournalId,
                //      configurationBillSundry.SalesAdjustedLedgerId,
                //      amount,
                //      0,
                //      DateTime.Now,
                //      "",
                //      ""
                //       ));

                //    }




                //    if (configurationBillSundry.CustomerAmountAdjusted)
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //       Guid.NewGuid().ToString(),
                //       calculationBIllDTOs.newJournalId,
                //       calculationBIllDTOs.partiesId,
                //       amount,
                //       0,
                //       DateTime.Now,
                //       "",
                //       ""
                //        ));

                //    }
                //    else
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //      Guid.NewGuid().ToString(),
                //      calculationBIllDTOs.newJournalId,
                //      configurationBillSundry.CustomerAdjustedLedgerId,
                //      0,
                //      amount,
                //      DateTime.Now,
                //      "",
                //      ""
                //       ));

                //    }
                //}





                //if (calculationBIllDTOs.isPurchaseTransaction && configurationBillSundry.IsPurchaseAccountingAffected)
                //{
                //    if (configurationBillSundry.IsPurchaseAmountAdjusted)
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //       Guid.NewGuid().ToString(),
                //       calculationBIllDTOs.newJournalId,
                //       LedgerConstants.StockLedgerId,
                //       amount,
                //       0,
                //       DateTime.Now,
                //       "",
                //       ""
                //        ));

                //    }
                //    else
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //      Guid.NewGuid().ToString(),
                //      calculationBIllDTOs.newJournalId,
                //      configurationBillSundry.PurchaseAdjustedLedgerId,
                //      0,
                //      amount,
                //      DateTime.Now,
                //      "",
                //      ""
                //       ));

                //    }




                //    if (configurationBillSundry.CustomerAmountAdjusted)
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //       Guid.NewGuid().ToString(),
                //       calculationBIllDTOs.newJournalId,
                //       calculationBIllDTOs.partiesId,
                //       amount,
                //       0,
                //       DateTime.Now,
                //       "",
                //       ""
                //        ));

                //    }
                //    else
                //    {
                //        journalDetails.Add(new JournalEntryDetails(
                //      Guid.NewGuid().ToString(),
                //      calculationBIllDTOs.newJournalId,
                //      configurationBillSundry.CustomerAdjustedLedgerId,
                //      0,
                //      amount,
                //      DateTime.Now,
                //      "",
                //      ""
                //       ));

                //    }
                //}


                var response = new CalculationBillResponseDTOs(
                    configurationBillSundry.Id,
                    amount,
                    costOfGoodSoldInSalesAmount,
                    costOfGoodSoldInPurchaseAmount,
                    costOfGoodsInStockTransferAmount
                );

                return Result<CalculationBillResponseDTOs>.Success(response);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting bill Sundry having");
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var billSundry = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(id);
                billSundry.IsActive = false;
                if (billSundry is null)
                {
                    return Result<bool>.Failure("NotFound", "billSundry Cannot be Found");
                }

                _unitOfWork.BaseRepository<BillSundry>().Update(billSundry);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting bill Sundry having {id}", ex);
            }
        }


        public async Task<Result<PagedResult<FilterSundryBillQueryResponse>>> FilterBillSundry(PaginationRequest paginationRequest, FilterSundryBillDto filterSundryBillDto)
        {
            try
            {
                var (sundryBill, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<BillSundry>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                var filterBillSundry = isSuperAdmin
                    ? sundryBill
                    : sundryBill.Where(x =>x.IsActive &&
                        x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");


                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );


                DateTime today = DateTime.Today;
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;


                if (filterSundryBillDto.startDate != default)
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterSundryBillDto.startDate);

                if (filterSundryBillDto.endDate != default)
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterSundryBillDto.endDate);

               
                if (startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = today;
                    endEnglishDate = today.AddDays(1).AddTicks(-1);
                }
              
                else if (startEnglishDate != null && endEnglishDate == null)
                {
                    endEnglishDate = startEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                else if (endEnglishDate != null && startEnglishDate == null)
                {
                    startEnglishDate = endEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                else
                {
                    startEnglishDate = startEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                var result = await _unitOfWork.BaseRepository<BillSundry>().GetConditionalAsync(
                    x => (x.IsActive && x.CompanyId == currentCompanyId || x.CompanyId =="" )&&
                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate)
                   
                );


                var responseList = result.Select(i => new FilterSundryBillQueryResponse(
                  i.Id,
                  i.Name,
                  i.BillType,
                  i.DefaultValue,
                  i.BillSundryNature,
                  i.IsCOGSAffected,
                  i.IsCOGPAffected,
                  i.IsCOGSTAffected,
                  i.IsSalesAccountingAffected,
                  i.IsPurchaseAccountingAffected,
                  i.IsSalesAmountAdjusted,
                  i.IsPurchaseAmountAdjusted,
                  i.CustomerAmountAdjusted,
                  i.VendorAmountAdjusted,
                  i.SalesAdjustedLedgerId,
                  i.CustomerAdjustedLedgerId,
                  i.PurchaseAdjustedLedgerId,
                  i.VendorAdjustedLedgerId,
                  i.CalculationType,
                  i.CalculationTypeOf


                )).ToList();


                PagedResult<FilterSundryBillQueryResponse> finalResponseList;
                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count;

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSundryBillQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSundryBillQueryResponse>
                    {
                        Items = responseList,
                        TotalItems = responseList.Count,
                        PageIndex = 1,
                        pageSize = responseList.Count
                    };
                }

                return Result<PagedResult<FilterSundryBillQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Bill Sundry: {ex.Message}", ex);
            }
            //throw new NotImplementedException();

        }


        public async Task<Result<PagedResult<GetBillSundryQueryResponse>>> GetSundryBill(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                
                var (billSundries, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<BillSundry>();

              
                var queryable = await _unitOfWork.BaseRepository<BillSundry>()
                    .FindBy(x => x.IsActive && x.CompanyId == currentCompanyId || x.CompanyId=="");

                var finalQuery = queryable.AsNoTracking();

               
                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);

               
                var mappedItems = _mapper.Map<List<GetBillSundryQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<GetBillSundryQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetBillSundryQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Sundry Bill", ex);
            }

        }

        public async Task<Result<GetBillSundryByIdQueryResponse>> GetSundryBillById(string Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var billSundry = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(Id);

                var billSundryResponse = _mapper.Map<GetBillSundryByIdQueryResponse>(billSundry);

                return Result<GetBillSundryByIdQueryResponse>.Success(billSundryResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sundry Bill by using Id", ex);
            }
        }

        public async Task<Result<UpdateBillSundryResponse>> Update(string Id, UpdateBillSundryCommand updateBillSundryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (Id == null)
                    {
                        return Result<UpdateBillSundryResponse>.Failure("NotFound", "Please provide valid sundry Bill id");
                    }

                    var billSundryTobeUpdated = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(Id);
                    if (billSundryTobeUpdated is null)
                    {
                        return Result<UpdateBillSundryResponse>.Failure("NotFound", "SundryBill are not Found");
                    }

                    _mapper.Map(updateBillSundryCommand, billSundryTobeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateBillSundryResponse
                        (
                           
                            billSundryTobeUpdated.Name,
                            billSundryTobeUpdated.BillType,
                            billSundryTobeUpdated.DefaultValue,
                            billSundryTobeUpdated.BillSundryNature,
                            billSundryTobeUpdated.IsCOGSAffected,
                            billSundryTobeUpdated.IsCOGPAffected,
                            billSundryTobeUpdated.IsCOGSTAffected,
                            billSundryTobeUpdated.IsSalesAccountingAffected,
                            billSundryTobeUpdated.IsPurchaseAccountingAffected,
                            billSundryTobeUpdated.IsSalesAmountAdjusted,
                            billSundryTobeUpdated.IsPurchaseAmountAdjusted,
                            billSundryTobeUpdated.CustomerAmountAdjusted,
                            billSundryTobeUpdated.VendorAmountAdjusted,
                            billSundryTobeUpdated.SalesAdjustedLedgerId,
                            billSundryTobeUpdated.CustomerAdjustedLedgerId,
                            billSundryTobeUpdated.PurchaseAdjustedLedgerId,
                            billSundryTobeUpdated.VendorAdjustedLedgerId,
                            billSundryTobeUpdated.CalculationType,
                            billSundryTobeUpdated.CalculationTypeOf
                          
                        );

                    return Result<UpdateBillSundryResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating Bill sundry");
                }
            }
        }
    }
}
