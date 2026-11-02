using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.Parties_Statements.Queries;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate;
using TN.Reports.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class PartyStatementServices : IPartyStatementServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;


        public PartyStatementServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;

        }
        public async Task<Result<List<PartyStatementQueryResponse>>> GetPartyStatement(string partyId)
        {
            try
            {
                var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>()
                      .GetConditionalAsync(
                           x => x.LedgerId == partyId && !x.IsDeleted,
                           query => query.Include(p => p.JournalEntry)
                                                 .ThenInclude(j => j.JournalEntryDetails)
                      );

                var excludedLedgerIds = new[]
                 {
                        "b5e7d3c9-4a6f-4d1e-92b0-7c2f8a9d6f3b",
                       "6c2f8a7d-3b4e-41d9-91f0-a5e7b6d3c9f2"
                    };

                var purchaseStatements = purchaseDetails
                    .SelectMany(p => p.JournalEntry?.JournalEntryDetails?
                        .Where(jd => !excludedLedgerIds.Contains(jd.LedgerId)) 
                        .GroupBy(jd => jd.JournalEntryId)
                        .Select(group => new PartyStatementQueryResponse
                        (
                            group.First().TransactionDate,             
                            p.BillNumber,
                            p.JournalEntry.ReferenceNumber,                               
                            group.Sum(jd => jd.DebitAmount),
                            group.Sum(jd => jd.CreditAmount),
                            p.GrandTotalAmount,
                            p.Id
                        )) ?? Enumerable.Empty<PartyStatementQueryResponse>());

                var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
                    .GetConditionalAsync(
                        x => x.LedgerId == partyId,
                        query => query.Include(s => s.JournalEntry)
                                               .ThenInclude(j => j.JournalEntryDetails)
                    );

                var salesStatements = salesDetails
                    .SelectMany(s => s.JournalEntry?.JournalEntryDetails?
                        .Where(jd => !excludedLedgerIds.Contains(jd.LedgerId))
                        .GroupBy(jd => jd.JournalEntryId)
                        .Select(groupSales => new PartyStatementQueryResponse
                        (
                            groupSales.First().TransactionDate,
                            s.BillNumber,
                            s.JournalEntry.ReferenceNumber,                 
                             groupSales.Sum(jd => jd.DebitAmount),
                            groupSales.Sum(jd => jd.CreditAmount),
                            s.GrandTotalAmount,
                            s.Id
                        )) ?? Enumerable.Empty<PartyStatementQueryResponse>())

                    .ToList();



                var fullStatement = purchaseStatements
                    .Concat(salesStatements)
                    .OrderBy(x => x.dateTime)
                    .ToList();

                return Result<List<PartyStatementQueryResponse>>.Success(fullStatement);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Party by {partyId}");
            }
        }

        public async Task<Result<PagedResult<GetPartyStatementFilterResponse>>> GetPartyStatementFilter(PaginationRequest paginationRequest,PartyStatementDto partyStatementDto)
        {
            try
            {
                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                var currentUserId = _tokenService.GetUserId().FirstOrDefault();
                var (ledger, scopedCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Ledger>();

             
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (!string.IsNullOrWhiteSpace(partyStatementDto.startDate))
                {
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(partyStatementDto.startDate);
                    if (startEnglishDate.Value.TimeOfDay == TimeSpan.Zero)
                        endEnglishDate = startEnglishDate.Value.AddDays(1);
                }

                if (!string.IsNullOrWhiteSpace(partyStatementDto.endDate))
                {
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(partyStatementDto.endDate);
                    if (endEnglishDate.Value.TimeOfDay == TimeSpan.Zero)
                        endEnglishDate = endEnglishDate.Value.AddDays(1);
                }

                if (!startEnglishDate.HasValue && !endEnglishDate.HasValue)
                {
                    startEnglishDate = DateTime.UtcNow.Date;
                    endEnglishDate = startEnglishDate.Value.AddDays(1);
                }
                else if (startEnglishDate.HasValue && !endEnglishDate.HasValue)
                {
                    endEnglishDate = startEnglishDate.Value.AddDays(1);
                }
                else if (!startEnglishDate.HasValue && endEnglishDate.HasValue)
                {
                    startEnglishDate = endEnglishDate.Value.AddDays(-1);
                }

                var excludedLedgerIds = new[]
                {
            "b5e7d3c9-4a6f-4d1e-92b0-7c2f8a9d6f3b",
            "6c2f8a7d-3b4e-41d9-91f0-a5e7b6d3c9f2"
        };

           
                var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .GetConditionalAsync(
                        x => x.LedgerId == partyStatementDto.partyId &&
                             (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                             (endEnglishDate == null || x.CreatedAt < endEnglishDate) &&
                             !x.IsDeleted,
                        query => query.Include(p => p.JournalEntry)
                                      .ThenInclude(j => j.JournalEntryDetails)
                    );

                var purchaseStatements = purchaseDetails
                    .SelectMany(p => p.JournalEntry?.JournalEntryDetails?
                        .Where(jd => !excludedLedgerIds.Contains(jd.LedgerId))
                        .GroupBy(jd => jd.JournalEntryId)
                        .Select(group => new GetPartyStatementFilterResponse(
                            group.First().TransactionDate,
                            p.BillNumber,
                            p.LedgerId,
                            p.PaymentId,
                            p.ReferenceNumber,
                            p.JournalEntry.ReferenceNumber,
                            group.Sum(jd => jd.DebitAmount),
                            group.Sum(jd => jd.CreditAmount),
                            p.GrandTotalAmount,
                            p.Id
                        )) ?? Enumerable.Empty<GetPartyStatementFilterResponse>());



                var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
                    .GetConditionalAsync(
                        x => x.LedgerId == partyStatementDto.partyId &&
                             (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                             (endEnglishDate == null || x.CreatedAt < endEnglishDate),
                        query => query.Include(s => s.JournalEntry)
                                      .ThenInclude(j => j.JournalEntryDetails)
                    );

                var salesStatements = salesDetails
                    .SelectMany(s => s.JournalEntry?.JournalEntryDetails?
                        .Where(jd => !excludedLedgerIds.Contains(jd.LedgerId))
                        .GroupBy(jd => jd.JournalEntryId)
                        .Select(groupSales => new GetPartyStatementFilterResponse(
                            groupSales.First().TransactionDate,
                            s.BillNumber,
                            s.LedgerId,
                             s.PaymentId,
                             "",
                            s.JournalEntry.ReferenceNumber,
                            groupSales.Sum(jd => jd.DebitAmount),
                            groupSales.Sum(jd => jd.CreditAmount),
                            s.GrandTotalAmount,
                            s.Id
                        )) ?? Enumerable.Empty<GetPartyStatementFilterResponse>());

            
                var fullStatement = purchaseStatements
                    .Concat(salesStatements)
                    .OrderBy(x => x.dateTime)
                    .ToList();


                PagedResult<GetPartyStatementFilterResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = salesStatements.Count();

                    var pagedItems = salesStatements
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetPartyStatementFilterResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetPartyStatementFilterResponse>
                    {
                        Items = salesStatements.ToList(),
                        TotalItems = salesStatements.Count(),
                        PageIndex = 1,
                        pageSize = salesStatements.Count()
                    };
                }

                return Result<PagedResult<GetPartyStatementFilterResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching party statement : {ex.Message}", ex);
            }
        }
    }
}
