using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TrialBalance;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class TrialBalanceServices : ITrialBalanceServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;


        public TrialBalanceServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;

        }
        public async Task<Result<PagedResult<MasterLevelQueryRespones>>> GetTrialBalanceReport(PaginationRequest paginationRequest, string? companyId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Get user scope
                var (journalEntry, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                IQueryable<JournalEntryDetails> filterJournalEntry;

                // Apply company/institution filters
                if (!string.IsNullOrEmpty(companyId))
                {
                    filterJournalEntry = journalEntry.Where(x => x.CompanyId == companyId);
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

                // Aggregate journal entries into trial balance per ledger
                var trialBalanceData = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalFilterType(
                        c => filterJournalEntry.Select(x => x.JournalEntryId).Contains(c.JournalEntryId),
                        query => query.AsNoTracking()
                            .GroupBy(c => c.LedgerId)
                            .Select(g => new
                            {
                                LedgerId = g.Key,
                                DebitAmount = g.Sum(x => x.DebitAmount) == 0 ? (decimal?)null : g.Sum(x => x.DebitAmount),
                                CreditAmount = g.Sum(x => x.CreditAmount) == 0 ? (decimal?)null : g.Sum(x => x.CreditAmount)
                            })
                    );

                var ledgerIds = trialBalanceData.Select(x => x.LedgerId).Distinct().ToList();

                // Fetch ledgers + related groups in one go
                var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                    .FindBy(l => ledgerIds.Contains(l.Id));

                var subLedgerGroupIds = ledgers.Select(l => l.SubLedgerGroupId).Distinct().ToList();

                var subLedgerGroups = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                    .FindBy(sg => subLedgerGroupIds.Contains(sg.Id));

                var ledgerGroupIds = subLedgerGroups.Select(sg => sg.LedgerGroupId).Distinct().ToList();

                var ledgerGroups = await _unitOfWork.BaseRepository<LedgerGroup>()
                    .FindBy(lg => ledgerGroupIds.Contains(lg.Id));

                // Build trial balance details
                var trialBalanceDetails = trialBalanceData.Select(entry =>
                {
                    var ledger = ledgers.FirstOrDefault(l => l.Id == entry.LedgerId);
                    var subLedgerGroup = ledger != null
                        ? subLedgerGroups.FirstOrDefault(sg => sg.Id == ledger.SubLedgerGroupId)
                        : null;
                    var ledgerGroup = subLedgerGroup != null
                        ? ledgerGroups.FirstOrDefault(lg => lg.Id == subLedgerGroup.LedgerGroupId)
                        : null;

                    return new TrialBalanceDetails(
                        ledgerGroup?.MasterId,
                        subLedgerGroup?.Id,
                        entry.LedgerId,
                        (ledger?.OpeningBalance ?? 0) + (entry.DebitAmount ?? 0), // Opening balance added to debit
                        entry.CreditAmount
                    );
                }).ToList();

                // Group by Master → LedgerGroup → Ledger
                var masterGroups = trialBalanceDetails
                    .GroupBy(x => x.masterId)
                    .Select(master => new MasterLevelQueryRespones(
                        master.Key,
                        master.Sum(x => x.debitAmount),
                        master.Sum(x => x.creditAmount),
                        master.GroupBy(x => x.subLedgerGroupId)
                            .Select(lg => new LedgerGroupLevel(
                                lg.Key,
                                lg.Sum(x => x.debitAmount),
                                lg.Sum(x => x.creditAmount),
                                lg.Select(l => new LedgerLevel(
                                    l.ledgerId,
                                    l.debitAmount,
                                    l.creditAmount
                                )).ToList()
                            )).ToList()
                    )).ToList();

                PagedResult<MasterLevelQueryRespones> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = masterGroups.Count();

                    var pagedItems = masterGroups
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<MasterLevelQueryRespones>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<MasterLevelQueryRespones>
                    {
                        Items = masterGroups.ToList(),
                        TotalItems = masterGroups.Count(),
                        PageIndex = 1,
                        pageSize = masterGroups.Count()
                    };
                }


                return Result<PagedResult<MasterLevelQueryRespones>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching trial balance", ex);
            }
        }
    }
}
