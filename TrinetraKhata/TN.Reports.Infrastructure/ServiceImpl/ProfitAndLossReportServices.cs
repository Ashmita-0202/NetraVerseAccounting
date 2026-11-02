using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.Profit_LossReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class ProfitAndLossReportServices : IProfitAndLossServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGetUserScopedData _getUserScopedData;

        public ProfitAndLossReportServices(IUnitOfWork unitOfWork,IMapper mapper,IGetUserScopedData getUserScopedData)
        {
            _unitOfWork=unitOfWork;
            _mapper = mapper;
            _getUserScopedData = getUserScopedData;
        }

        public async Task<Result<PagedResult<ProfitAndLossFinalResponse>>> GetProfitLossReport(PaginationRequest PaginationRequest, string? CompanyId,CancellationToken cancellationToken=default)
        {
            try
            {
                // 1. Get user scoped data
                var (journalEntry, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                // 2. Filter by company
                IQueryable<JournalEntryDetails> filterJournalEntry;

                if (!string.IsNullOrEmpty(CompanyId))
                    filterJournalEntry = journalEntry.Where(x => x.CompanyId == CompanyId);
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                    filterJournalEntry = journalEntry.Where(x => x.CompanyId == currentCompanyId);
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
                    filterJournalEntry = journalEntry;

                // 3. Project only necessary fields for P&L
                var entries = await filterJournalEntry
                    .Where(e => e.Ledger != null &&
                                (e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Income ||
                                 e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Expenses ||
                                 e.Ledger.SubLedgerGroup.Id == LedgerGroupConstrants.DutiesAndTaxesLedgerId))

                    .Select(e => new
                    {
                        LedgerId = e.Ledger.Id,
                        LedgerName = e.Ledger.Name,
                        SubLedgerGroupId = e.Ledger.SubLedgerGroup.Id,
                        SubLedgerGroupName = e.Ledger.SubLedgerGroup.Name,
                        LedgerGroupId = e.Ledger.SubLedgerGroup.LedgerGroup.Id,
                        LedgerGroupName = e.Ledger.SubLedgerGroup.LedgerGroup.Name,
                        SectionName = e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Income ? "Income" : "Expenses",
                        Balance = e.DebitAmount - e.CreditAmount
                    })
                    .ToListAsync(cancellationToken);

                // 4. Map to DTOs
                var profitLossDetails = entries.Select(e =>
                {
                    decimal balance = Math.Abs(e.Balance);
                    string balanceType = e.Balance >= 0 ? "Dr" : "Cr";

                    return new ProfitAndLossDetails(
                        e.SectionName,
                        e.LedgerGroupId,
                        e.LedgerGroupName,
                        e.SubLedgerGroupId,
                        e.SubLedgerGroupName,
                        e.LedgerId,
                        e.LedgerName,
                        balance,
                        balanceType
                    );
                }).ToList();

                // 5. Group hierarchy (Fixed: sum by LedgerId to avoid duplicates)
                var masterGroups = profitLossDetails
                    .GroupBy(x => new { x.SectionName, x.LedgerGroupId, x.LedgerGroupName })
                    .Select(master => new MasterLevelProfitAndLoss(
                        master.Key.SectionName,
                        master.Key.LedgerGroupId,
                        master.Key.LedgerGroupName,
                        master.Sum(x => x.Balance),
                        master.GroupBy(x => new { x.SubLedgerGroupId, x.SubLedgerGroupName })
                            .Select(subGroup =>
                            {
                                // First aggregate all ledgers with the same LedgerId under this subgroup
                                var ledgers = subGroup
                                    .GroupBy(l => new { l.LedgerId, l.LedgerName })
                                    .Select(lg =>
                                    {
                                        decimal totalBalance = lg.Sum(x => x.Balance);
                                        string balanceType = totalBalance >= 0 ? "Dr" : "Cr";
                                        return new LedgerLevelProfitAndLoss(
                                            lg.Key.LedgerId,
                                            lg.Key.LedgerName,
                                            Math.Abs(totalBalance),
                                            balanceType
                                        );
                                    })
                                    .ToList();

                                // Compute total for this subgroup
                                decimal subTotal = ledgers.Sum(l => l.Balance);

                                return new LedgerGroupLevelProfitAndLoss(
                                    master.Key.LedgerGroupId,
                                    master.Key.LedgerGroupName,
                                    subTotal,
                                    new List<SubLedgerGroupLevelProfitAndLoss>
                                    {
                                        new SubLedgerGroupLevelProfitAndLoss(
                                            subGroup.Key.SubLedgerGroupId,
                                            subGroup.Key.SubLedgerGroupName,
                                            subTotal,
                                            ledgers
                                        )
                                    }
                                );
                            })
                            .ToList()
                    )).ToList();


                var incomeGroups = masterGroups.Where(m => m.SectionName == "Income").ToList();
                var expenseGroups = masterGroups.Where(m => m.SectionName == "Expenses").ToList();

                decimal totalIncome = incomeGroups.Sum(m => m.Total);
                decimal totalExpenses = expenseGroups.Sum(m => m.Total);
                decimal grossProfitOrLoss = totalIncome - totalExpenses;
                decimal netProfitOrLoss = grossProfitOrLoss;


                // Duties & Taxes Calculation
                        var dutiesAndTaxes = profitLossDetails
                  .Where(x => x.SubLedgerGroupId == LedgerGroupConstrants.DutiesAndTaxesLedgerId)
                  .GroupBy(x => new { x.LedgerId, x.LedgerName })
                  .Select(g =>
                  {
                      decimal total = g.Sum(x => x.Balance);
                      string balanceType = total >= 0 ? "Dr" : "Cr";

                      return new DutiesAndTaxesDto(
                          g.Key.LedgerName,
                          Math.Abs(total),
                          balanceType
                      );
                  })
                  .ToList();

                
                decimal totalDutiesAndTaxes = dutiesAndTaxes.Sum(d => d.Amount);
                decimal netProfitAfterTax = netProfitOrLoss - totalDutiesAndTaxes;

                // Final response
                var finalResponse = new ProfitAndLossFinalResponse(
                    incomeGroups,
                    expenseGroups,
                    grossProfitOrLoss,
                    netProfitOrLoss,
                    dutiesAndTaxes,
                    totalDutiesAndTaxes,
                    netProfitAfterTax
                );


                PagedResult<ProfitAndLossFinalResponse> paginatedResult;
                var items = new List<ProfitAndLossFinalResponse> { finalResponse };

                if (PaginationRequest.IsPagination)
                {
                    int pageIndex = PaginationRequest.pageIndex <= 0 ? 1 : PaginationRequest.pageIndex;
                    int pageSize = PaginationRequest.pageSize <= 0 ? 10 : PaginationRequest.pageSize;

                    paginatedResult = new PagedResult<ProfitAndLossFinalResponse>
                    {
                        Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                        TotalItems = items.Count,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    paginatedResult = new PagedResult<ProfitAndLossFinalResponse>
                    {
                        Items = items,
                        TotalItems = 1,
                        PageIndex = 1,
                        pageSize = 1
                    };
                }

                return Result<PagedResult<ProfitAndLossFinalResponse>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("An error occurred while generating Profit and Loss Report", ex);
            }

        }
    }
}
