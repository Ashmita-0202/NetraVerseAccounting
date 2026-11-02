using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TradingAccount;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class TradingService : ITradingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverterHelper;

        public TradingService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData, IDateConvertHelper dateConverterHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _dateConverterHelper = dateConverterHelper;
        }

        public async Task<GetTradingAccountQueryResponse> GenerateTradingReport(string? startDate, string? endDate,string? companyId, CancellationToken cancellationToken=default)
        {
            try
            {
                // Get scoped query and user details
                var (journalEntryQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                IQueryable<JournalEntryDetails> filterQuery;

                // Company-based filtering
                if (!string.IsNullOrEmpty(companyId))
                {
                    filterQuery = journalEntryQuery.Where(x => x.CompanyId == companyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filterQuery = journalEntryQuery.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id));

                    filterQuery = journalEntryQuery.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filterQuery = journalEntryQuery;
                }

                // Date filtering
                DateTime? fromDate = !string.IsNullOrEmpty(startDate)
                    ? await _dateConverterHelper.ConvertToEnglish(startDate)
                    : null;

                DateTime? toDate = !string.IsNullOrEmpty(endDate)
                    ? await _dateConverterHelper.ConvertToEnglish(endDate)
                    : null;

                if (fromDate.HasValue && toDate.HasValue)
                {
                    filterQuery = filterQuery.Where(x => x.TransactionDate >= fromDate.Value && x.TransactionDate <= toDate.Value);
                }
                else if (fromDate.HasValue)
                {
                    filterQuery = filterQuery.Where(x => x.TransactionDate >= fromDate.Value);
                }
                else if (toDate.HasValue)
                {
                    filterQuery = filterQuery.Where(x => x.TransactionDate <= toDate.Value);
                }

                // Include ledger with subledger and ledger group
                filterQuery = filterQuery
                    .Include(x => x.Ledger)
                        .ThenInclude(l => l.SubLedgerGroup)
                            .ThenInclude(sg => sg.LedgerGroup);

                var entries = await filterQuery.ToListAsync(cancellationToken);

                // Filter only Direct Income and Direct Expense ledger groups
                var filteredEntries = entries
                    .Where(x =>
                        x.Ledger != null &&
                        x.Ledger.SubLedgerGroup != null &&
                        x.Ledger.SubLedgerGroup.LedgerGroup != null &&
                        (
                            x.Ledger.SubLedgerGroup.LedgerGroupId == LedgerGroupConstrants.DirectIncomeLedgerId ||
                            x.Ledger.SubLedgerGroup.LedgerGroupId == LedgerGroupConstrants.DirectExpenseLedgerId
                        )
                    ).ToList();

                // Group by LedgerGroupId (Direct Income or Direct Expense)
                var grouped = filteredEntries
                    .GroupBy(x => x.Ledger.SubLedgerGroup.LedgerGroupId)
                    .Select(g =>
                    {
                        var ledgers = g
                            .GroupBy(x => x.LedgerId)
                            .Select(l =>
                            {
                                var ledgerGroupId = l.First().Ledger.SubLedgerGroup.LedgerGroupId;

                                decimal balance = ledgerGroupId == LedgerGroupConstrants.DirectIncomeLedgerId
                                    ? l.Sum(e => e.CreditAmount - e.DebitAmount) // Income: Credit - Debit
                                    : l.Sum(e => e.DebitAmount - e.CreditAmount); // Expense: Debit - Credit

                                return new TradingReportLedgerItem(
                                    l.Key,
                                    l.First().Ledger.Name,
                                    balance
                                );
                            }).ToList();

                        return new TradingReportGroup(
                            g.Key,
                            ledgers,
                            ledgers.Sum(x => x.Balance)
                        );
                    }).ToList();

                // Separate income and expense groups by LedgerGroupId
                var incomeGroups = grouped
                    .Where(x => x.LedgerGroupId == LedgerGroupConstrants.DirectIncomeLedgerId)
                    .ToList();

                var expenseGroups = grouped
                    .Where(x => x.LedgerGroupId == LedgerGroupConstrants.DirectExpenseLedgerId)
                    .ToList();

                var totalIncome = incomeGroups.Sum(x => x.GroupTotal);
                var totalExpense = expenseGroups.Sum(x => x.GroupTotal);
                var grossProfitOrGrossLoss = totalIncome - totalExpense;

                return new GetTradingAccountQueryResponse(
                    incomeGroups,
                    expenseGroups,
                    totalIncome,
                    totalExpense,
                    grossProfitOrGrossLoss
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the Trading Report.", ex);
            }
        }
    }
}
