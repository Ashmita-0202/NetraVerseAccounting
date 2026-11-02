using AutoMapper;
using DateConverterNepali;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.OpeningClosingBalance;
using TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class OpeningClosingBalanceServices : IOpeningClosingBalanceServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;
        public OpeningClosingBalanceServices(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IDateConvertHelper dateConvertHelper,
            IGetUserScopedData getUserScopedData)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;
        }

        public async Task<Result<PagedResult<OpeningClosingBalanceResponse>>> GetOpeningClosingBalance(string fyId,PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                        .FirstOrDefaultAsync(f => f.Id == fyId);


                if (fiscalYear == null)
                    throw new Exception("Invalid Fiscal Year");

                var startDate = fiscalYear.StartDate;
                var endDate = fiscalYear.EndDate;

                //var journalQuery = await _unitOfWork.BaseRepository<JournalEntry>()
                //        .GetConditionalAsync(null, query=>query.Include(x=>x.JournalEntryDetails));

                var journalDetailsQuery = await _unitOfWork.BaseRepository<JournalEntryDetails>().GetAllAsyncWithPagination();

                var filteredDetails = await journalDetailsQuery
                   .Where(jd => jd.TransactionDate <= endDate)
                   .ToListAsync();

                #region Filter ByDate
                //var ledgerJournalGroups = await journalQuery
                //    .Where(j => j.EntryDate <= endDate)
                //    .GroupBy(j => j.LedgerId)
                //    .Select(g => new
                //    {
                //        LedgerId = g.Key,
                //        OpeningBalance = g
                //            .Where(e => e.EntryDate < startDate)
                //            .Sum(e => e.Debit - e.Credit),

                //        PeriodDebit = g
                //            .Where(e => e.EntryDate >= startDate && e.EntryDate <= endDate)
                //            .Sum(e => e.Debit),

                //        PeriodCredit = g
                //            .Where(e => e.EntryDate >= startDate && e.EntryDate <= endDate)
                //            .Sum(e => e.Credit)
                //    })
                //    .AsNoTracking()
                //    .ToListAsync();

                #endregion
                var journalSummary = filteredDetails
                    .GroupBy(jd => jd.LedgerId)
                    .Select(g => new
                    {
                        LedgerId = g.Key,
                        OpeningBalance = g
                            .Where(jd => jd.TransactionDate < startDate)
                            .Sum(jd => jd.DebitAmount - jd.CreditAmount),

                        PeriodDebit = g
                            .Where(jd => jd.TransactionDate >= startDate && jd.TransactionDate <= endDate)
                            .Sum(jd => jd.DebitAmount),

                        PeriodCredit = g
                            .Where(jd => jd.TransactionDate >= startDate && jd.TransactionDate <= endDate)
                            .Sum(jd => jd.CreditAmount)
                    })
                    .ToList();

                var ledgerQuery = await _unitOfWork.BaseRepository<Ledger>().GetAllAsyncWithPagination();
                var ledgers = await ledgerQuery.ToListAsync();

                var allBalances = ledgers.Select(ledger =>
                {
                    var group = journalSummary.FirstOrDefault(g => g.LedgerId == ledger.Id);

                    var journalOpening = group?.OpeningBalance ?? 0;
                    var periodDebit = group?.PeriodDebit ?? 0;
                    var periodCredit = group?.PeriodCredit ?? 0;

                    var manualOpening = ledger.OpeningBalance ?? 0;
                    var openingBalance = journalOpening != 0 ? journalOpening : manualOpening;

                    var closingBalance = openingBalance + periodDebit - periodCredit;

                    return new OpeningClosingBalanceResponse(
                        ledger.Id,
                        ledger.Name,
                        openingBalance,
   
                        closingBalance
                    );
                }).ToList();



                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;


                int totalItems = allBalances.Count;

                var pagedItems = allBalances
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Step 3: Construct paged result
                var pagedResult = new PagedResult<OpeningClosingBalanceResponse>
                {
                    Items = pagedItems,
                    TotalItems = totalItems,
                    PageIndex = pageIndex,
                    pageSize = pageSize
                };
                return Result<PagedResult<OpeningClosingBalanceResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                return Result<PagedResult<OpeningClosingBalanceResponse>>.Failure(ex.Message);
            }
        }

        public async Task<Result<OpeningClosingBalanceByLedgerResponse>> GetOpeningClosingBalanceByLedger(OpeningClosingBalanceDTOs openingClosingBalanceDTOs, CancellationToken cancellationToken = default)
        {
            try
            {
                var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .FirstOrDefaultAsync(f => f.Id == openingClosingBalanceDTOs.fyId);

                if (fiscalYear == null)
                    throw new Exception("Invalid Fiscal Year");

                var startDate = fiscalYear.StartDate.Date;
                var endDate = fiscalYear.EndDate.Date;

                var ledger = await _unitOfWork.BaseRepository<Ledger>()
                    .FirstOrDefaultAsync(l => l.Id == openingClosingBalanceDTOs.ledgerId);

                if (ledger == null)
                    throw new Exception("Ledger not found");


                var journalDetailsQuery = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetAllAsyncWithPagination();

                var test = journalDetailsQuery.ToList();

                var filteredDetails = await journalDetailsQuery
                    .Where(jd => jd.LedgerId == openingClosingBalanceDTOs.ledgerId &&
                           jd.TransactionDate >= startDate && jd.TransactionDate <= endDate).ToListAsync();

                var openingBalanceFromJournal = filteredDetails
                    .Where(jd => jd.TransactionDate < startDate)
                    .Sum(jd => jd.DebitAmount - jd.CreditAmount);

                var periodDebit = filteredDetails
                    .Where(jd => jd.TransactionDate >= startDate && jd.TransactionDate <= endDate)
                    .Sum(jd => jd.DebitAmount);

                var periodCredit = filteredDetails
                    .Where(jd => jd.TransactionDate >= startDate && jd.TransactionDate <= endDate)
                    .Sum(jd => jd.CreditAmount);

                var manualOpeningBalance = ledger.OpeningBalance ?? 0;
                var finalOpeningBalance = openingBalanceFromJournal != 0 ? openingBalanceFromJournal : manualOpeningBalance;

                var closingBalance = finalOpeningBalance + periodDebit - periodCredit;

                var result = new OpeningClosingBalanceByLedgerResponse
                (ledger.Id,
                ledger.Name,
                finalOpeningBalance,
                closingBalance
                    );
             
                return Result<OpeningClosingBalanceByLedgerResponse>.Success(result);



            }
            catch(Exception ex)
            {
                return Result<OpeningClosingBalanceByLedgerResponse>.Failure(ex.Message);
            }
        }
    }
}
