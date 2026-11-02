using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TrialBalance;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class BalanceSheetServices : IBalanceSheetServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;


        public BalanceSheetServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;

        }
        public async Task<Result<PagedResult<BalanceSheetFinalResponse>>> GetBalanceSheetReport(PaginationRequest paginationRequest, string? requestedCompanyId, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Get user scoped data
                var (journalEntry, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                IQueryable<JournalEntryDetails> filterJournalEntry;

                // 2. Filter journal entries by company or institution
                if (!string.IsNullOrEmpty(requestedCompanyId))
                {
                    filterJournalEntry = journalEntry.Where(x => x.CompanyId == requestedCompanyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filterJournalEntry = journalEntry.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filterJournalEntry = journalEntry.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filterJournalEntry = journalEntry;
                }

                // 3. Fetch balances grouped by LedgerId
                var getBalanceSheetDetails = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalFilterType(
                        x => filterJournalEntry.Select(f => f.LedgerId).Contains(x.LedgerId),
                        query => query
                            .Include(x => x.Ledger)
                                .ThenInclude(l => l.SubLedgerGroup)
                                    .ThenInclude(sg => sg.LedgerGroup)
                                        .ThenInclude(lg => lg.Masters)
                            .AsNoTracking()
                            .GroupBy(x => x.LedgerId)
                            .Select(g => new TrialBalanceDTOs(
                                g.Key,
                                g.Sum(x => x.DebitAmount) == 0 ? (decimal?)null : g.Sum(x => x.DebitAmount),
                                g.Sum(x => x.CreditAmount) == 0 ? (decimal?)null : g.Sum(x => x.CreditAmount)
                            ))
                    );

                var ledgerIds = getBalanceSheetDetails.Select(x => x.ledgerId).Distinct().ToList();

                // 4. Fetch ledger details with navigations
                var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                 .GetConditionalFilterType(
                     x => ledgerIds.Contains(x.Id),
                     query => query
                         .Include(l => l.SubLedgerGroup)
                             .ThenInclude(sg => sg.LedgerGroup)
                                 .ThenInclude(lg => lg.Masters)
                 );

                          var trialBalanceDetails = getBalanceSheetDetails
                .Select(entry =>
                {
                    var ledger = ledgers.FirstOrDefault(l => l.Id == entry.ledgerId);
                    if (ledger == null) return null; // skip if ledger not found

                    var subGroup = ledger.SubLedgerGroup;
                    var ledgerGroup = subGroup?.LedgerGroup;

                    // Exclude Income & Expense
                    if (ledgerGroup?.MasterId == MasterConstraints.Income ||
                        ledgerGroup?.MasterId == MasterConstraints.Expenses)
                        return null;

                    decimal rawBalance = (entry.debitAmount ?? 0) - (entry.creditAmount ?? 0);

                    // Determine balance type (Dr/Cr) based on master account
                    string balanceType = ledgerGroup?.MasterId switch
                    {
                        MasterConstraints.Assets or MasterConstraints.Expenses => rawBalance >= 0 ? "Dr" : "Cr",
                        MasterConstraints.Liabilities or MasterConstraints.Income => rawBalance >= 0 ? "Cr" : "Dr",
                        _ => rawBalance >= 0 ? "Dr" : "Cr"
                    };

                    decimal Balance = Math.Abs(rawBalance);

                    string formattedBalance = $"{Balance} {balanceType}";
                    // Determine section (Assets / Liabilities)
                    string sectionName = ledgerGroup?.MasterId switch
                    {
                        MasterConstraints.Assets => "Assets",
                        MasterConstraints.Liabilities => "Liabilities",
                        _ => ledgerGroup?.Id switch
                        {
                            LedgerGroupConstrants.CurrentAssets => "Assets",
                            LedgerGroupConstrants.FixedAssets => "Assets",
                            LedgerGroupConstrants.LoansLiability => "Liabilities",
                            LedgerGroupConstrants.ShareHolderEquity => "Liabilities",
                            LedgerGroupConstrants.CurrentLiabilities => "Liabilities",
                            LedgerGroupConstrants.LongTermLiabilities => "Liabilities",
                            _ => ledger?.Name?.Contains("Cash", StringComparison.OrdinalIgnoreCase) == true ||
                                 ledger?.Name?.Contains("Stock", StringComparison.OrdinalIgnoreCase) == true
                                 ? "Assets"
                                 : "Liabilities"
                         }
                     };

                    return new BalanceSheetDetails(
                        sectionName ?? "Liabilities",        
                        ledgerGroup?.MasterId ?? "",
                        ledgerGroup?.Masters?.Name ?? "",
                        ledgerGroup?.Id ?? "",
                        ledgerGroup?.Name ?? "",
                        subGroup?.Id ?? "",
                        subGroup?.Name ?? "",
                        ledger?.Id ?? "",
                        ledger?.Name ?? "",
                        Balance,
                        balanceType
                    );
            })
            .Where(x => x != null) // remove Income/Expense & missing ledgers
            .ToList()!;


                // 6. Group hierarchy
                var masterGroups = trialBalanceDetails
                    .GroupBy(x => new { x.sectionName, x.masterId, x.masterName })
                    .Select(master => new MasterLevelBalanceSheetResponse(
                        master.Key.sectionName,
                        master.Key.masterId,
                        master.Key.masterName ?? master.Key.sectionName,
                        master.Sum(x => x.balance),
                        master.GroupBy(x => new { x.ledgerGroupId, x.ledgerGroupName })
                            .Select(ledgerGroup => new LedgerGroupLevelBalanceSheet(
                                ledgerGroup.Key.ledgerGroupId,
                                ledgerGroup.Key.ledgerGroupName ?? "",
                                ledgerGroup.Sum(x => x.balance),
                                ledgerGroup.GroupBy(x => new { x.subledgerGroupId, x.subledgerGroupName })
                                    .Select(subGroup => new SubLedgerGroupLevelBalanceSheet(
                                        subGroup.Key.subledgerGroupId,
                                        subGroup.Key.subledgerGroupName ?? "",
                                        subGroup.Sum(x => x.balance),
                                        subGroup.Select(l => new LedgerLevelBalanceSheet(
                                            l.ledgerId,
                                            l.ledgerName,
                                            l.balance,
                                             l.balanceType
                                        )).ToList()
                                    )).ToList()
                            )).ToList()
                    )).ToList();

                // 7. Split Assets & Liabilities
                var assets = masterGroups.Where(m => m.sectionName == "Assets").ToList();
                var liabilities = masterGroups.Where(m => m.sectionName == "Liabilities").ToList();

                var finalResponse = new BalanceSheetFinalResponse(assets, liabilities);

                PagedResult<BalanceSheetFinalResponse> paginatedResult;

                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    var items = new List<BalanceSheetFinalResponse> { finalResponse };
                    int totalItems = items.Count;

                    var pagedItems = items
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    paginatedResult = new PagedResult<BalanceSheetFinalResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    paginatedResult = new PagedResult<BalanceSheetFinalResponse>
                    {
                        Items = new List<BalanceSheetFinalResponse> { finalResponse },
                        TotalItems = 1,
                        PageIndex = 1,
                        pageSize = 1
                    };
                }
                return Result<PagedResult<BalanceSheetFinalResponse>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching balance sheet", ex);
            }
        }
    }
}
