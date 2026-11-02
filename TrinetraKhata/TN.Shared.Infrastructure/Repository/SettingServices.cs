
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusByCompany;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetInventoryMethodByCompany;
using TN.Shared.Application.Shared.Queries.GetJournalRefByCompany;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInSales;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Infrastructure.Repository
{
    public class SettingServices : ISettingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly ITokenService _tokenService;
        private readonly FiscalContext _fiscalContext;
     


        public SettingServices(FiscalContext fiscalContext,IUnitOfWork unitOfWork,IMapper mapper, IBillNumberGenerator billNumberGenerator, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _billNumberGenerator = billNumberGenerator;
            _tokenService = tokenService;
            _fiscalContext = fiscalContext;

        }

        public async Task<Result<UpdateFiscalYearResponse>> UpdateFiscalYear(string companyId, string? currentFiscalYearId, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    bool IsDemoUser = await _tokenService.isDemoUser();
                    string userId = _tokenService.GetUserId();
                    CompanySettings? fiscalYear;

                    if (IsDemoUser)
                    {
                        fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>()
                            .GetSingleAsync(x => x.CompanyId == companyId
                                              && x.UserId == userId);
                    }
                    else
                    {
                        fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>()
                            .GetSingleAsync(x => x.CompanyId == companyId
                                              && x.UserId == null);
                    }

                    fiscalYear.CurrentFiscalYearId = currentFiscalYearId;
                    fiscalYear.CompanyId = companyId;

                    _unitOfWork.BaseRepository<CompanySettings>().Update(fiscalYear);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var fiscalYearDisplay = new UpdateFiscalYearResponse(fiscalYear.CompanyId, fiscalYear.CurrentFiscalYearId);
                    return Result<UpdateFiscalYearResponse>.Success(fiscalYearDisplay);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Failed to update current fiscal Year");
                }
            }
        }
        public async Task<Result<PagedResult<GetAllFiscalYearQueryResponse>>> GetAllFiscalYear(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>().GetAllAsyncWithPagination();
                //var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                //        .GetSingleAsync(fy => fy.Id == _fiscalContext.CurrentFiscalYearId && !fy.IsClosed);

                //            var fiscalYearQueryable = await _unitOfWork.BaseRepository<FiscalYears>()
                //.FindBy(fy => !fy.IsClosed );

                fiscalYear = fiscalYear.OrderBy(fy => fy.StartDate);

                var fiscalYearResult = await fiscalYear
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var fiscalYearResponse = _mapper.Map<PagedResult<GetAllFiscalYearQueryResponse>>(fiscalYearResult.Data);

                return Result<PagedResult<GetAllFiscalYearQueryResponse>>.Success(fiscalYearResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all fiscal year", ex);
            }
        }

        public async Task<Result<GetCurrentFiscalYearQueryResponse>> GetCurrentFiscalYearBy(string companyId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? fiscalYear;

                if (IsDemoUser)
                {
                    fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


                var fiscalYearData = new GetCurrentFiscalYearQueryResponse(fiscalYear.CurrentFiscalYearId,fiscalYear.CompanyId, currentFiscalYear.StartDate.ToString(), currentFiscalYear.FyName);
                return Result<GetCurrentFiscalYearQueryResponse>.Success(fiscalYearData);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Current Fiscal Year ", ex);
            }
        }

        public async Task<Result<GetInventoryByCompanyIdQueryResponse>> GetInventoryMethodByCompany(string companyId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? companySettings;

                if (IsDemoUser)
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

              
                if (companySettings == null)
                {
                    return Result<GetInventoryByCompanyIdQueryResponse>.Failure("Company settings not found.");
                }

                var response = new GetInventoryByCompanyIdQueryResponse(companySettings.InventoryMethod);

                return Result<GetInventoryByCompanyIdQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch inventory method", ex);
            }
        }

      
        public async Task<Result<GetItemStatusByCompanyResponse>> GetItemStatusByCompany(string companyId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? expiredDate;

                if (IsDemoUser)
                {
                    expiredDate = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    expiredDate = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                var expiredDateStatus = new GetItemStatusByCompanyResponse(expiredDate.ShowExpiredDateInItem, expiredDate.ShowSerialNumberInItem);
                return Result<GetItemStatusByCompanyResponse>.Success(expiredDateStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch ExpiredDateItemStatus", ex);
            }
        }


        public async Task<Result<GetJournalRefByCompanyQueryResponse>> GetJournalRefByCompany(string companyId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? journalRef;

                if (IsDemoUser)
                {
                    journalRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    journalRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                if (journalRef == null)
                {
                    return Result<GetJournalRefByCompanyQueryResponse>.Failure("Company settings not found.");
                }

                var journalReferencesNumber = journalRef.JournalReference == JournalReferencesType.Automatic
                ? await _billNumberGenerator.GenerateJournalReference(companyId)
                : string.Empty;

                var journalRefDisplay = new GetJournalRefByCompanyQueryResponse(journalRef.JournalReference, journalRef.CompanyId, journalReferencesNumber);
                return Result<GetJournalRefByCompanyQueryResponse>.Success(journalRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch journal reference number");
            }
        }

        public async Task<Result<GetPurchaseReferenceNumberQueryResponse>> GetPurchaseReferenceNumber(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? purchaseRef;

                if (IsDemoUser)
                {
                    purchaseRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }



                if (purchaseRef == null)
                {
                    return Result<GetPurchaseReferenceNumberQueryResponse>.Failure("Company settings not found.");
                }
                var purchaseReferencesNumber = purchaseRef.PurchaseReferences == PurchaseReferencesType.Automatic
                         ? await _billNumberGenerator.GenerateReferenceNumber(companyId, ReferenceType.Purchase)
                         : string.Empty;

                var purchaseRefDisplay = new GetPurchaseReferenceNumberQueryResponse(purchaseRef.PurchaseReferences, purchaseRef.CompanyId, purchaseReferencesNumber);
                return Result<GetPurchaseReferenceNumberQueryResponse>.Success(purchaseRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch SalesReference Numbers");
            }
        }

        public async Task<Result<GetSalesReferenceNumberQueryResponse>> GetSalesReferenceNumber(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? salesReference;

                if (IsDemoUser)
                {
                    salesReference = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReference = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                var salesSerialNumberStatus = new GetSalesReferenceNumberQueryResponse(companyId, salesReference.ShowReferenceNumberForSales);
                return Result<GetSalesReferenceNumberQueryResponse>.Success(salesSerialNumberStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch SalesReference Numbers");
            }
        }

        public async Task<Result<GetSerialNumberForPurchaseQueryResponse>> GetSerialNumberForPurchase(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? serialNumber;

                if (IsDemoUser)
                {
                    serialNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    serialNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                var serialNumberDisplay = new GetSerialNumberForPurchaseQueryResponse( serialNumber.ShowSerialNumberForPurchase, serialNumber.CompanyId);
                return Result<GetSerialNumberForPurchaseQueryResponse>.Success(serialNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        public async Task<Result<GetTaxStatusInPurchaseResponse>> GetTaxStatusInPurchase(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? taxStatusInPurchase;

                if (IsDemoUser)
                {
                    taxStatusInPurchase = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatusInPurchase = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                var taxStatusInPurchaseDetails = new GetTaxStatusInPurchaseResponse(taxStatusInPurchase.CompanyId, taxStatusInPurchase.ShowTaxInPurchase);
                return Result<GetTaxStatusInPurchaseResponse>.Success(taxStatusInPurchaseDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        public async Task<Result<GetTaxStatusInSalesResponse>> GetTaxStatusInSales(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? taxStatusInSales;

                if (IsDemoUser)
                {
                    taxStatusInSales = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatusInSales = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                var taxStatusInSalesDetails = new GetTaxStatusInSalesResponse(taxStatusInSales.CompanyId, taxStatusInSales.ShowTaxInSales);
                return Result<GetTaxStatusInSalesResponse>.Success(taxStatusInSalesDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        

        public async Task<Result<UpdateInventoryMethodResponse>> UpdateInventoryMethodByCompany(string companyId, InventoryMethodType inventoryMethod, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? inventory;

                if (IsDemoUser)
                {
                    inventory = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    inventory = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }



                inventory.InventoryMethod = inventoryMethod;
                inventory.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(inventory);
                await _unitOfWork.SaveChangesAsync();

                var inventoryDisplay = new UpdateInventoryMethodResponse(inventory.InventoryMethod,inventory.CompanyId);
                return Result<UpdateInventoryMethodResponse>.Success(inventoryDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update Inventory Method");
            }
        }

        public async Task<Result<UpdateItemStatusByCompanyResponse>> UpdateItemStatusByCompany(string companyId, bool isExpiredDate,bool isSerialNo, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? itemStatus;

                if (IsDemoUser)
                {
                    itemStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    itemStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }
                itemStatus.ShowExpiredDateInItem = isExpiredDate;
                itemStatus.ShowSerialNumberInItem = isSerialNo;

                _unitOfWork.BaseRepository<CompanySettings>().Update(itemStatus);
                await _unitOfWork.SaveChangesAsync();

                var expiredDateStatus = new UpdateItemStatusByCompanyResponse(itemStatus.CompanyId, itemStatus.ShowExpiredDateInItem, itemStatus.ShowSerialNumberInItem);
                return Result<UpdateItemStatusByCompanyResponse>.Success(expiredDateStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch ExpiredDateItemStatus");
            }
        }

        public async Task<Result<UpdateJournalRefByCompanyResponse>> UpdateJournalRefByCompany(string companyId, JournalReferencesType journalReferences, CancellationToken cancellationToken)
        {
            try
            {


                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? journalRef;

                if (IsDemoUser)
                {
                    journalRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    journalRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                journalRef.JournalReference = journalReferences;
                journalRef.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(journalRef);
                await _unitOfWork.SaveChangesAsync();

                var journalRefDisplay = new UpdateJournalRefByCompanyResponse( journalRef.JournalReference, journalRef.CompanyId);
                return Result<UpdateJournalRefByCompanyResponse>.Success(journalRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update journal reference number");
            }
        }
        
        public async Task<Result<UpdatePurchaseReferenceNumberResponse>> UpdatePurchaseReferenceNumberByCompany(string companyId,  PurchaseReferencesType purchaseReferencesType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? purchaseRef;

                if (IsDemoUser)
                {
                    purchaseRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseRef = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                purchaseRef.PurchaseReferences = purchaseReferencesType;
                purchaseRef.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(purchaseRef);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdatePurchaseReferenceNumberResponse(purchaseRef.PurchaseReferences, purchaseRef.CompanyId);
                return Result<UpdatePurchaseReferenceNumberResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Reference number for purchase");
            }
        }

        public async Task<Result<UpdateSalesReferenceNumberResponse>> UpdateSalesReferenceNumberByCompany(string companyId, bool showReferenceNumberForSales, CancellationToken cancellationToken)
        {
            try
            {



                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? salesReference;

                if (IsDemoUser)
                {
                    salesReference = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReference = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                salesReference.ShowReferenceNumberForSales = showReferenceNumberForSales;
               salesReference.CompanyId= companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(salesReference);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdateSalesReferenceNumberResponse(salesReference.ShowReferenceNumberForSales, salesReference.CompanyId);
                return Result<UpdateSalesReferenceNumberResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Reference number for sales");
            }
        }

        public async Task<Result<UpdateTaxStatusInPurchaseResponse>> UpdateTaxStatusInPurchaseBy(string companyId, bool showTaxInPurchase, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? taxStatus;

                if (IsDemoUser)
                {
                    taxStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                taxStatus.ShowTaxInPurchase = showTaxInPurchase;
                taxStatus.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(taxStatus);
                await _unitOfWork.SaveChangesAsync();

                var purchaseResult = new UpdateTaxStatusInPurchaseResponse(taxStatus.CompanyId,taxStatus.ShowTaxInPurchase);
                return Result<UpdateTaxStatusInPurchaseResponse>.Success(purchaseResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch tax Status for purchase");
            }
        }

        public async Task<Result<UpdateTaxStatusInSalesResponse>> UpdateTaxStatusInSalesBy(string companyId, bool showTaxInSales, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? taxStatus;

                if (IsDemoUser)
                {
                    taxStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatus = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                taxStatus.ShowTaxInSales = showTaxInSales;
                taxStatus.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(taxStatus);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdateTaxStatusInSalesResponse(taxStatus.CompanyId, taxStatus.ShowTaxInPurchase);
                return Result<UpdateTaxStatusInSalesResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch tax Status for sales");
            }
        }

        public async Task<Result<ReceiptTransactionNumberTypeResponse>> GetReceiptTransactionType(string companyId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }


            if (receiptTransactionType == null)
            {
                return Result<ReceiptTransactionNumberTypeResponse>.Failure("Company settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var receiptTransactionNumberType = receiptTransactionType.ReceiptTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(companyId, "RECEIPT", currentFiscalYear.FyName)
            : string.Empty;

            var receiptTransactionTypeDisplay = new ReceiptTransactionNumberTypeResponse(receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.CompanyId, receiptTransactionNumberType);
            return Result<ReceiptTransactionNumberTypeResponse>.Success(receiptTransactionTypeDisplay);
        }

        public async Task<Result<UpdateReceiptTransactionNumberTypeResponse>> UpdateReceiptTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }


            if (receiptTransactionType is null)
                return Result<UpdateReceiptTransactionNumberTypeResponse>.Failure("Company settings not found.");

            receiptTransactionType.ReceiptTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<CompanySettings>().Update(receiptTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateReceiptTransactionNumberTypeResponse>.Success(
                new UpdateReceiptTransactionNumberTypeResponse(receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.CompanyId)
            );

        }

        public async Task<Result<UpdatePaymentTransactionNumberTypeResponse>> UpdatePaymentTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }


            if (receiptTransactionType is null)
                return Result<UpdatePaymentTransactionNumberTypeResponse>.Failure("Company settings not found.");

            receiptTransactionType.PaymentTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<CompanySettings>().Update(receiptTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdatePaymentTransactionNumberTypeResponse>.Success(
                new UpdatePaymentTransactionNumberTypeResponse(receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.CompanyId)
            );
        }

        public async Task<Result<GetPaymentTransactionNumberTypeResponse>> GetPaymentTransactionType(string companyId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? paymentTransactionType;

            if (IsDemoUser)
            {
                paymentTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                paymentTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }

            if (paymentTransactionType == null)
            {
                return Result<GetPaymentTransactionNumberTypeResponse>.Failure("Company settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var paymentTransactionNumberType = paymentTransactionType.PaymentTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(companyId, "PAYMENT", currentFiscalYear.FyName)
            : string.Empty;

            var paymentTransactionTypeDisplay = new GetPaymentTransactionNumberTypeResponse(paymentTransactionType.PaymentTransactionNumberType, paymentTransactionType.CompanyId, paymentTransactionNumberType);
            return Result<GetPaymentTransactionNumberTypeResponse>.Success(paymentTransactionTypeDisplay);
        }

        public async Task<Result<GetIncomeTransactionNumberTypeResponse>> GetIncomeTransactionType(string companyId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? incomeTransactionType;

            if (IsDemoUser)
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }



            var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


            if (incomeTransactionType == null)
            {
                return Result<GetIncomeTransactionNumberTypeResponse>.Failure("Company settings not found.");
            }

            var incomeTransactionNumberType = incomeTransactionType.IncomeTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(companyId, "INCOME", currentFiscalYear.FyName)
            : string.Empty;

            var incomeTransactionTypeDisplay = new GetIncomeTransactionNumberTypeResponse(incomeTransactionType.IncomeTransactionNumberType, incomeTransactionType.CompanyId, incomeTransactionNumberType);
            return Result<GetIncomeTransactionNumberTypeResponse>.Success(incomeTransactionTypeDisplay);

        }

        public async Task<Result<GetExpenseTransactionNumberTypeResponse>> GetExpenseTransactionType(string companyId, CancellationToken cancellationToken)
        {
            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? expenseTransactionType;

            if (IsDemoUser)
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }


            if (expenseTransactionType == null)
            {
                return Result<GetExpenseTransactionNumberTypeResponse>.Failure("Company settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var expenseTransactionNumberType = expenseTransactionType.ExpensesTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(companyId, "EXPENSES", currentFiscalYear.FyName)
            : string.Empty;

            var expenseTransactionTypeDisplay = new GetExpenseTransactionNumberTypeResponse(expenseTransactionType.ExpensesTransactionNumberType, expenseTransactionType.CompanyId, expenseTransactionNumberType);
            return Result<GetExpenseTransactionNumberTypeResponse>.Success(expenseTransactionTypeDisplay);
        }

        public async Task<Result<UpdateExpenseTransactionNumberTypeResponse>> UpdateExpenseTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {
            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? expenseTransactionType;

            if (IsDemoUser)
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }

            if (expenseTransactionType is null)
                return Result<UpdateExpenseTransactionNumberTypeResponse>.Failure("Company settings not found.");

            expenseTransactionType.ExpensesTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<CompanySettings>().Update(expenseTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateExpenseTransactionNumberTypeResponse>.Success(
                new UpdateExpenseTransactionNumberTypeResponse(expenseTransactionType.ExpensesTransactionNumberType, expenseTransactionType.CompanyId)
            );
        }

        public async Task<Result<UpdateIncomeTransactionNumberTypeResponse>> UpdateIncomeTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            CompanySettings? incomeTransactionType;

            if (IsDemoUser)
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == userId);
            }
            else
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<CompanySettings>()
                    .GetSingleAsync(x => x.CompanyId == companyId
                                      && x.UserId == null);
            }

            if (incomeTransactionType is null)
                return Result<UpdateIncomeTransactionNumberTypeResponse>.Failure("Company settings not found.");

            incomeTransactionType.IncomeTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<CompanySettings>().Update(incomeTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateIncomeTransactionNumberTypeResponse>.Success(
                new UpdateIncomeTransactionNumberTypeResponse(incomeTransactionType.IncomeTransactionNumberType, incomeTransactionType.CompanyId)
            );
        }

        public async Task<Result<GetPurchaseReturnNumberQueryResponse>> GetPurchaseReturnNumber(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? companySettings;

                if (IsDemoUser)
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                if (companySettings == null)
                {
                    return Result<GetPurchaseReturnNumberQueryResponse>.Failure("Company settings not found.");
                }

                string purchaseReturnNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (companySettings.PurchaseReturnNumberType == CompanySettings.PurchaseSalesReturnNumberType.Automatic)
                {
                    purchaseReturnNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(companyId, "PURCHASERETURN", currentFiscalYear.FyName);
                }

                var response = new GetPurchaseReturnNumberQueryResponse(
                    companyId,
                    companySettings.PurchaseReturnNumberType,
                    purchaseReturnNumber

                );

                return Result<GetPurchaseReturnNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Purchase Return Number", ex);
            }
        }

        public async Task<Result<GetSalesReturnNumberQueryResponse>> GetSalesReturnNumber(string companyId, CancellationToken cancellationToken)
        {
            try
            {


                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? companySettings;

                if (IsDemoUser)
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                if (companySettings == null)
                {
                    return Result<GetSalesReturnNumberQueryResponse>.Failure("Company settings not found.");
                }

                string salesReturnNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


                if (companySettings.SalesReturnNumberType == CompanySettings.PurchaseSalesReturnNumberType.Automatic)
                {
                    salesReturnNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(companyId, "SALESRETURN", currentFiscalYear.FyName);
                }

                var response = new GetSalesReturnNumberQueryResponse(
                    companyId,
                    companySettings.SalesReturnNumberType,
                    salesReturnNumber
                );

                return Result<GetSalesReturnNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Sales Return Number", ex);
            }
        }

        public async Task<Result<UpdatePurchaseReturnTypeResponse>> UpdatePurchaseReturnType(string companyId, PurchaseSalesReturnNumberType purchaseReturnNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? purchaseReturn;

                if (IsDemoUser)
                {
                    purchaseReturn = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseReturn = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                purchaseReturn.PurchaseReturnNumberType = purchaseReturnNumberType;
                purchaseReturn.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(purchaseReturn);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdatePurchaseReturnTypeResponse(purchaseReturn.CompanyId, purchaseReturn.PurchaseReturnNumberType);
                return Result<UpdatePurchaseReturnTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update purchase return Number");
            }
        }

        public async Task<Result<GetPurchaseQuotationNumberQueryResponse>> GetPurchaseQuotationNumber(string companyId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? companySettings;

                if (IsDemoUser)
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }



                if (companySettings == null)
                {
                    return Result<GetPurchaseQuotationNumberQueryResponse>.Failure("Company settings not found.");
                }

                string purchaseQuotationNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (companySettings.PurchaseQuotationNumberType == CompanySettings.PurchaseSalesQuotationNumberType.Automatic)
                {
                    purchaseQuotationNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(companyId, "PURCHASEQUOTATION", currentFiscalYear.FyName);
                }

                var response = new GetPurchaseQuotationNumberQueryResponse(
                    companyId,
                    companySettings.PurchaseQuotationNumberType,
                    purchaseQuotationNumber
                );

                return Result<GetPurchaseQuotationNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Purchase Quotation Number", ex);
            }
        }

        public async Task<Result<UpdateSalesReturnTypeResponse>> UpdateSalesReturnType(string companyId, PurchaseSalesReturnNumberType salesReturnNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? salesReturn;

                if (IsDemoUser)
                {
                    salesReturn = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReturn = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                salesReturn.SalesReturnNumberType = salesReturnNumberType;
                salesReturn.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(salesReturn);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdateSalesReturnTypeResponse(salesReturn.CompanyId, salesReturn.SalesReturnNumberType);
                return Result<UpdateSalesReturnTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update sales return Number");
            }
        }

        public async Task<Result<UpdatePurchaseQuotationTypeResponse>> UpdatePurchaseQuotationType(string companyId, PurchaseSalesQuotationNumberType purchaseQuotationNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? quotationNumber;

                if (IsDemoUser)
                {
                    quotationNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    quotationNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                quotationNumber.PurchaseQuotationNumberType = purchaseQuotationNumberType;
                quotationNumber.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(quotationNumber);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdatePurchaseQuotationTypeResponse(quotationNumber.CompanyId, quotationNumber.PurchaseQuotationNumberType);
                return Result<UpdatePurchaseQuotationTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update purchase Quotation Number");
            }
        }

        public async Task<Result<GetSalesQuotationTypeQueryResponse>> GetSalesQuotationType(string companyId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? companySettings;

                if (IsDemoUser)
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    companySettings = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }


                if (companySettings == null)
                {
                    return Result<GetSalesQuotationTypeQueryResponse>.Failure("Company settings not found.");
                }

                string salesQuotationNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (companySettings.SalesQuotationNumberType == CompanySettings.PurchaseSalesQuotationNumberType.Automatic)
                {
                    salesQuotationNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(companyId, "SALESQUOTATION", currentFiscalYear.FyName);
                }

                var response = new GetSalesQuotationTypeQueryResponse(
                    companyId,
                    companySettings.SalesQuotationNumberType,
                    salesQuotationNumber

                );

                return Result<GetSalesQuotationTypeQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Sales Quotation Number", ex);
            }
        }

        public async Task<Result<UpdateSalesQuotationTypeResponse>> UpdateSalesQuotationType(string companyId, PurchaseSalesQuotationNumberType salesQuotationNumberType, CancellationToken cancellationToken)
        {

            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                CompanySettings? quotationNumber;

                if (IsDemoUser)
                {
                    quotationNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == userId);
                }
                else
                {
                    quotationNumber = await _unitOfWork.BaseRepository<CompanySettings>()
                        .GetSingleAsync(x => x.CompanyId == companyId
                                          && x.UserId == null);
                }

                quotationNumber.SalesQuotationNumberType = salesQuotationNumberType;
                quotationNumber.CompanyId = companyId;

                _unitOfWork.BaseRepository<CompanySettings>().Update(quotationNumber);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdateSalesQuotationTypeResponse(quotationNumber.CompanyId, quotationNumber.SalesQuotationNumberType);
                return Result<UpdateSalesQuotationTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update sales Quotation Number");
            }
        }
    }
}
