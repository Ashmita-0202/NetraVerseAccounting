using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Application.Account.Queries.ChartOfAccounts;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.DataSeed;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class ChartAccountServices : IChartAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly DataSeeder _dataSeeder;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IBaseRepository<LedgerGroup> _ledgerGroupRepository;
        private readonly IBaseRepository<Master> _masterRepository;
        private readonly IBaseRepository<SubLedgerGroup> _subLedgerGroup;
        private readonly IBaseRepository<Ledger> _ledgerRepository;

        public ChartAccountServices(IUnitOfWork unitOfWork,ITokenService tokenService, IMapper mapper, DataSeeder dataSeeder,IGetUserScopedData getUserScopedData, IBaseRepository<LedgerGroup> ledgerGroupRepository,
        IBaseRepository<Master> masterRepository,
        IBaseRepository<SubLedgerGroup> subLedgerGroup,
        IBaseRepository<Ledger> ledgerRepository)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dataSeeder = dataSeeder;
            _getUserScopedData = getUserScopedData;
            _ledgerGroupRepository = ledgerGroupRepository;
            _masterRepository = masterRepository;
            _subLedgerGroup = subLedgerGroup;
            _ledgerRepository = ledgerRepository;

        }

        public async Task<Result<List<ChartsOfAccountsQueryResponse>>> GetFullChartAsync()
        {
            try
            {
                var userId = _tokenService.GetUserId();
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var masters = await _masterRepository.GetAllAsyncWithPagination();
                var ledgerGroups = await _ledgerGroupRepository.GetConditionalAsync(x => x.CompanyId == companyId || x.CompanyId == "");
                var subLedgerGroups = await _subLedgerGroup.GetConditionalAsync(x => x.CompanyId == companyId || x.CompanyId == "");
                var ledgers = await _ledgerRepository.GetConditionalAsync(x => x.CompanyId == companyId || x.CompanyId == "");
                // Fetch balances
                var ledgerBalances = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalFilterType(
                        j => j.IsActive && j.CompanyId == companyId,
                        query => query
                            .GroupBy(g => new { g.LedgerId, g.Ledger.SubLedgerGroupId })
                            .Select(g => new
                            {
                                LedgerId = g.Key.LedgerId,
                                SubLedgerGroupId = g.Key.SubLedgerGroupId,
                                // Always store raw balance as Debit - Credit
                                Balance = g.Sum(x => x.DebitAmount - x.CreditAmount),
                            })
                    );

                // Materialize once
                var mastersList = await masters.ToListAsync();
                var ledgerGroupsList = ledgerGroups.ToList();
                var subLedgerGroupsList = subLedgerGroups.ToList();
                var ledgersList = ledgers.ToList();

                // Build Chart of Accounts
                var chart = mastersList.Select(master =>
                {
                    var masterLedgerGroups = ledgerGroupsList.Where(group => group.MasterId == master.Id);

                    var ledgerGroupResponses = masterLedgerGroups.Select(group =>
                    {
                        var groupSubLedgers = subLedgerGroupsList.Where(sub => sub.LedgerGroupId == group.Id);

                        var subLedgerGroupResponses = groupSubLedgers.Select(sub =>
                        {
                            decimal subBalance;
                            string subBalanceType;
                            List<LedgerResponse> ledgerResponses = new();

                            if (sub.Id == SubLedgerGroupConstants.SundryDebtor || sub.Id == SubLedgerGroupConstants.SundryCreditors)
                            {
                                // Only calculate balance (exclude ledgers)
                                subBalance = ledgerBalances
                                    .Where(lb => lb.SubLedgerGroupId == sub.Id)
                                    .Sum(lb => lb.Balance);

                                subBalanceType = (master.Id == MasterConstraints.Assets || master.Id == MasterConstraints.Expenses)
                                    ? (subBalance >= 0 ? "Dr" : "Cr")
                                    : (subBalance >= 0 ? "Cr" : "Dr");
                            }
                            else
                            {
                                // Normal case → include ledgers
                                var subLedgers = ledgersList.Where(ledger => ledger.SubLedgerGroupId == sub.Id);

                                ledgerResponses = subLedgers.Select(ledger =>
                                {
                                    var balance = ledgerBalances
                                        .FirstOrDefault(lb => lb.LedgerId == ledger.Id)?.Balance ?? 0m;

                                    string balanceType = (master.Id == MasterConstraints.Assets || master.Id == MasterConstraints.Expenses)
                                        ? (balance >= 0 ? "Dr" : "Cr")
                                        : (balance >= 0 ? "Cr" : "Dr");

                                    return new LedgerResponse(
                                        ledger.Id,
                                        ledger.Name,
                                        balance,      // raw signed value
                                        balanceType
                                    );
                                }).ToList();

                                subBalance = ledgerResponses.Sum(l => l.balance);

                                subBalanceType = (master.Id == MasterConstraints.Assets || master.Id == MasterConstraints.Expenses)
                                    ? (subBalance >= 0 ? "Dr" : "Cr")
                                    : (subBalance >= 0 ? "Cr" : "Dr");

                                // Convert ledgerResponses balances to absolute values for display
                                ledgerResponses = ledgerResponses.Select(l =>
                                    new LedgerResponse(l.id, l.name, Math.Abs(l.balance), l.balanceType)
                                ).ToList();
                            }

                            return new SubLedgerGroupResponse(
                                sub.Id,
                                sub.Name,
                                Math.Abs(subBalance),   // absolute only at final response
                                subBalanceType,
                                ledgerResponses
                            );
                        }).ToList();

                        var groupBalance = subLedgerGroupResponses.Sum(s => s.balance);

                        string groupBalanceType = (master.Id == MasterConstraints.Assets || master.Id == MasterConstraints.Expenses)
                            ? (groupBalance >= 0 ? "Dr" : "Cr")
                            : (groupBalance >= 0 ? "Cr" : "Dr");

                        return new LedgerGroupResponse(
                            group.Id,
                            group.Name,
                            Math.Abs(groupBalance),
                            groupBalanceType,
                            subLedgerGroupResponses
                        );
                    }).ToList();

                    var masterBalance = ledgerGroupResponses.Sum(g => g.balance);

                    string masterBalanceTypeStr = (master.Id == MasterConstraints.Assets || master.Id == MasterConstraints.Expenses)
                        ? (masterBalance >= 0 ? "Dr" : "Cr")
                        : (masterBalance >= 0 ? "Cr" : "Dr");

                    return new ChartsOfAccountsQueryResponse(
                        master.Id,
                        master.Name,
                        Math.Abs(masterBalance),
                        masterBalanceTypeStr,
                        ledgerGroupResponses
                    );
                }).ToList();

                return Result<List<ChartsOfAccountsQueryResponse>>.Success(chart);
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Full Charts");
            }
        }
    }
}
