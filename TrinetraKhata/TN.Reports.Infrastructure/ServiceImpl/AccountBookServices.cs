using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.AccountBook.Queries.JournalRegister;
using TN.Reports.Application.AccountBook.Queries.PurchaseRegister;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Reports.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class AccountBookServices : IAccountBookServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;

        public AccountBookServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;
            
        }

        public async Task<Result<PagedResult<JournalRegisterQueryResponse>>> GetJournalRegisterByLedger(PaginationRequest paginationRequest, JournalRegisterDTOs journalRegisterDTOs)
        {
            try
            {
               
                DateTime startEnglishDate = journalRegisterDTOs.startDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(journalRegisterDTOs.startDate);

                DateTime endEnglishDate = journalRegisterDTOs.endDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(journalRegisterDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

              
                var (journalEntryQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                IQueryable<JournalEntryDetails> filterJournalEntry;

                
                if (!string.IsNullOrEmpty(journalRegisterDTOs.companyId))
                {
                    filterJournalEntry = journalEntryQuery.Where(x => x.CompanyId == journalRegisterDTOs.companyId);
                }
              
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filterJournalEntry = journalEntryQuery.Where(x => x.CompanyId == currentCompanyId);
                }
               
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(x => x.InstitutionId == institutionId, query => query.Select(c => c.Id));

                    filterJournalEntry = journalEntryQuery.Where(x => companyIds.Contains(x.CompanyId));
                }
               
                else
                {
                    filterJournalEntry = journalEntryQuery;
                }

               
                filterJournalEntry = filterJournalEntry.Where(x =>
                    (string.IsNullOrEmpty(journalRegisterDTOs.ledgerId) || x.LedgerId == journalRegisterDTOs.ledgerId) &&
                    x.TransactionDate >= startEnglishDate &&
                    x.TransactionDate <= endEnglishDate);

               
                var resultQuery = filterJournalEntry.Select(journal => new JournalRegisterQueryResponse(
                    journal.Id,
                    journal.JournalEntryId,
                    journal.LedgerId,
                    journal.DebitAmount,
                    journal.CreditAmount,
                    journal.TransactionDate,
                    journal.CompanyId
                ));

                 PagedResult<JournalRegisterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = resultQuery.Count();

                    var pagedItems = resultQuery
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<JournalRegisterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<JournalRegisterQueryResponse>
                    {
                        Items = resultQuery.ToList(),
                        TotalItems = resultQuery.Count(),
                        PageIndex = 1,
                        pageSize = resultQuery.Count() 
                    };
                }

                return Result<PagedResult<JournalRegisterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching journal register for ledgerId = {journalRegisterDTOs.ledgerId}", ex);
            }
        }



        public async Task<Result<PagedResult<PurchaseRegisterQueryResponse>>> GetPurchaseRegister(PaginationRequest paginationRequest, PurchaseRegisterDTOs purchaseRegisterDTOs)
        {
            try
            {
                
                DateTime startEnglishDate = purchaseRegisterDTOs.startDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(purchaseRegisterDTOs.startDate);

                DateTime endEnglishDate = purchaseRegisterDTOs.endDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(purchaseRegisterDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

                var (purchaseQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();

                IQueryable<PurchaseDetails> filterQuery;

                
                if (!string.IsNullOrEmpty(purchaseRegisterDTOs.companyId))
                {
                    filterQuery = purchaseQuery.Where(x => x.CompanyId == purchaseRegisterDTOs.companyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filterQuery = purchaseQuery.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(x => x.InstitutionId == institutionId, query => query.Select(c => c.Id));

                    filterQuery = purchaseQuery.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filterQuery = purchaseQuery;
                }

                filterQuery = filterQuery
                 .Where(sd => sd.CreatedAt >= startEnglishDate && sd.CreatedAt <= endEnglishDate)
                 .Include(sd => sd.PurchaseItems);
               

               
                var purchaseRegisterResponse = await filterQuery
                    .Select(purchase => new PurchaseRegisterQueryResponse(
                        purchase.Date,
                        purchase.BillNumber,
                        purchase.LedgerId,
                        "", 
                        purchase.GrandTotalAmount,
                        purchase.CompanyId,
                        purchase.VatPercent,
                        purchase.VatAmount,
                        purchase.ReferenceNumber,
                        purchase.LedgerId,
                        purchase.PurchaseItems.Select(item => new PurchaseRegisterItemsDTOs(
                            item.Quantity,
                            item.UnitId,
                            item.ItemId,
                            item.Amount
                        )).ToList()
                    )).ToListAsync();


                PagedResult<PurchaseRegisterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = purchaseRegisterResponse.Count();

                    var pagedItems = purchaseRegisterResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<PurchaseRegisterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<PurchaseRegisterQueryResponse>
                    {
                        Items = purchaseRegisterResponse.ToList(),
                        TotalItems = purchaseRegisterResponse.Count(),
                        PageIndex = 1,
                        pageSize = purchaseRegisterResponse.Count()
                    };
                }

                return Result<PagedResult<PurchaseRegisterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Purchase Register", ex);
            }
        }

        public async Task<Result<PagedResult<SalesRegisterQueryResponse>>> GetSalesRegister(PaginationRequest paginationRequest,SalesRegisterDTOs salesRegisterDTOs)
        {
            try
            {
             
                DateTime startEnglishDate = salesRegisterDTOs.startDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(salesRegisterDTOs.startDate);

                DateTime endEnglishDate = salesRegisterDTOs.endDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(salesRegisterDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

              
                var (salesQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

                IQueryable<SalesDetails> filterQuery;

              
                if (!string.IsNullOrEmpty(salesRegisterDTOs.companyId))
                {
                    filterQuery = salesQuery.Where(x => x.CompanyId == salesRegisterDTOs.companyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filterQuery = salesQuery.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(x => x.InstitutionId == institutionId, query => query.Select(c => c.Id));

                    filterQuery = salesQuery.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filterQuery = salesQuery;
                }


                filterQuery = filterQuery
                .Where(sd => sd.CreatedAt >= startEnglishDate && sd.CreatedAt <= endEnglishDate)
                .Include(sd => sd.SalesItems);


                var salesRegisterResponse = await filterQuery
                    .Select(sales => new SalesRegisterQueryResponse(
                        sales.Date,
                        sales.BillNumber,
                        sales.LedgerId,
                        "",
                        sales.GrandTotalAmount,
                        sales.CompanyId,
                        sales.VatAmount,
                        sales.VatPercent,
                        sales.SalesItems.Select(items => new SalesRegisterItemDTOs(
                            items.ItemId,
                            items.Amount,
                            items.Quantity,
                            items.UnitId
                         
                        )).ToList()
                    )).ToListAsync();


                PagedResult<SalesRegisterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = salesRegisterResponse.Count();

                    var pagedItems = salesRegisterResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<SalesRegisterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<SalesRegisterQueryResponse>
                    {
                        Items = salesRegisterResponse.ToList(),
                        TotalItems = salesRegisterResponse.Count(),
                        PageIndex = 1,
                        pageSize = salesRegisterResponse.Count()
                    };
                }


                return Result<PagedResult<SalesRegisterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Sales Register", ex);
            }
        }
    }
}
