using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Queries.FiscalYearStartDate;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class FiscalYearServices : IFiscalYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly FiscalContext _fiscalContext;
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;


        public FiscalYearServices(IUnitOfWork unitOfWork, ITokenService tokenService, FiscalContext fiscalContext,IMapper mapper, ISettingServices settingServices)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
        }

        public async Task<Result<CloseFiscalYearResponse>> CloseFiscalYear(CloseFiscalYearCommand request)
        {

           using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
        

                    var companyId = _tokenService.CompanyId().FirstOrDefault();

                    #region Running CompanyDate and doesnot give access when closed fyId beyond the currentDate
                    var currentFiscalYearsToBeClosed = (await _unitOfWork.BaseRepository<FiscalYears>()
                   .GetConditionalAsync(
                       predicate: fy => fy.Id == request.closedFiscalId,
                       includes: fy => fy.CompanySettingFiscalYears
                   )).FirstOrDefault();

                    var companySettingFiscalYear = currentFiscalYearsToBeClosed.CompanySettingFiscalYears?
                        .FirstOrDefault(x => x.CompanyId == companyId)
                        ?? throw new Exception("No CompanySettingsFiscalYear linked to this fiscal year.");

                    var runningCompanySettingFiscalYear = (await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>()
                        .FindBy(fy => fy.CompanySettingId == companySettingFiscalYear.CompanySettingId && !fy.IsClosed))
                        .FirstOrDefault()
                        ?? throw new Exception("No current CompanySettingsFiscalYear found.");

                    var runningFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                        .GetSingleAsync(fy => fy.Id == runningCompanySettingFiscalYear.FiscalYearId)
                        ?? throw new Exception("Running FiscalYear not found.");

                    var runningFiscalYearStartDate = runningFiscalYear.StartDate.Date;
                    var currentDate = DateTime.Now.Date;
                    
                    if (runningFiscalYearStartDate < currentDate)
                    {
                        throw new NotFoundExceptions("You won't go beyond the current date.");
                        //return Result<List<GetSelectedFiscalYearQueryResponse>>.Failure("");
                    }

                    #endregion

                    var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                       .GetConditionalAsync(
                           predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                           includes: fy => fy.CompanySettingFiscalYears
                       )).FirstOrDefault();

                    if (currentFiscalYears == null)
                        throw new Exception("Fiscal year not found.");

                    if (currentFiscalYears.CompanySettingFiscalYears == null || !currentFiscalYears.CompanySettingFiscalYears.Any())
                        throw new Exception("No CompanySettingsFiscalYear linked to this fiscal year.");
              

                    var companySettingsId = currentFiscalYears.CompanySettingFiscalYears
                        .Where(x=>x.CompanyId ==companyId)
                        .Select(x => x.CompanySettingId)
                        .FirstOrDefault();
     
                    var fiscalYear = await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>()
                    .FirstOrDefaultAsync(x => x.FiscalYearId == request.closedFiscalId && x.CompanySettingId == companySettingsId && x.CompanyId == companyId);

                    if (fiscalYear == null || fiscalYear.IsClosed)
                        throw new Exception("Fiscal year not found or already closed.");


                    var ledgerBalances = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                          .GetConditionalFilterType(
                              x => x.FiscalId == request.closedFiscalId,
                              query => query
                                  .GroupBy(g => g.LedgerId)
                                  .Select(g => new
                                  {
                                      LedgerId = g.Key,
                                      Balance = g.Sum(x => x.CreditAmount - x.DebitAmount)
                                  })
                          );

                    var closingBalances = ledgerBalances.Select(b =>
                        new ClosingBalance(
                            Guid.NewGuid().ToString(),
                            b.LedgerId,
                            request.closedFiscalId,
                            b.Balance,
                            _tokenService.GetUserId(),
                            DateTime.UtcNow
                        )
                    ).ToList();



        



                    await _unitOfWork.BaseRepository<ClosingBalance>().AddRange(closingBalances);

                    fiscalYear.IsClosed = true;
                    fiscalYear.UserId = _tokenService.GetUserId();
                    fiscalYear.ClosedAt = DateTime.UtcNow;

                    _unitOfWork.BaseRepository<CompanySettingsFiscalYear>().Update(fiscalYear);

                    var currentFiscalYear = (await _unitOfWork.BaseRepository<FiscalYears>()
                          .GetConditionalAsync(fy => fy.Id == _fiscalContext.CurrentFiscalYearId))
                          .SingleOrDefault();


                    var nextFiscalYear = (await _unitOfWork.BaseRepository<FiscalYears>()
                       .GetConditionalAsync(
                           fy => fy.StartDate > currentFiscalYear.StartDate,
                           q => q.OrderBy(fy => fy.StartDate)
                       ))
                       .FirstOrDefault();



                    var companySettingsFiscalYear = new CompanySettingsFiscalYear
                        (
                        Guid.NewGuid().ToString(),
                        fiscalYear.CompanySettingId,
                        nextFiscalYear.Id,
                        false,
                        "",
                        DateTime.UtcNow,
                        true,
                        nextFiscalYear.FyName,
                        companyId,
                        false
                        );
                    await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>().AddAsync(companySettingsFiscalYear);


                    #region Update IsUpToCurrentFiscalYear
                    fiscalYear.IsUpToCurrentFiscalYear = false;
                    _unitOfWork.BaseRepository<CompanySettingsFiscalYear>().Update(fiscalYear);

                    #endregion


                    #region Update CurrentFyId
                    var updateFiscalYear = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);
                    updateFiscalYear.CurrentFiscalYearId = nextFiscalYear.Id;

                    _unitOfWork.BaseRepository<CompanySettings>().Update(updateFiscalYear);
                    #endregion

                    var openingBalances = closingBalances.Select(cb => new OpeningBalance
                    (
                        Guid.NewGuid().ToString(),
                        cb.LedgerId,
                        nextFiscalYear.Id,
                        cb.Balance
                    )).ToList();

                    await _unitOfWork.BaseRepository<OpeningBalance>().AddRange(openingBalances);


                    //if (request.autoOpenNext)
                    //{


                    //}
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var response = new CloseFiscalYearResponse
                    (fiscalYear.FiscalYearId,
                        "Fiscal year closed and balances carried forward"
                    );

                    return Result<CloseFiscalYearResponse>.Success(response);

                }
       

            
        }

        public async Task<FiscalYears?> GetCurrentFiscalYearFromSettingsAsync()
        {

            var companyId = _tokenService.CompanyId().FirstOrDefault();

            var settingsList = await _unitOfWork.BaseRepository<CompanySettings>()
                .GetConditionalAsync(
                    x => x.CompanyId == companyId,
                    null,
                    x => x.CurrentFiscalYear
                );

            var settings = settingsList.FirstOrDefault();
            return settings?.CurrentFiscalYear;

        }

        public async Task<(string Id, string FyName)> GetFiscalYearIdForDateAsync(DateTime date)
        {
            var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
            .GetSingleAsync(fy => date >= fy.StartDate && date <= fy.EndDate);

            if (fiscalYear == null)
                throw new InvalidOperationException("No fiscal year found for the given date.");

            return (fiscalYear.Id, fiscalYear.FyName);
        }

        public async Task<Result<FiscalYearStartDateResponse>> GetFiscalYearStartDate()
        {

            var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                      .GetConditionalAsync(
                          predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                          includes: fy => fy.CompanySettingFiscalYears
                      )).FirstOrDefault();

            if (currentFiscalYears == null)
                throw new Exception("Fiscal year not found.");

            if (currentFiscalYears.CompanySettingFiscalYears == null || !currentFiscalYears.CompanySettingFiscalYears.Any())
                throw new Exception("No CompanySettingsFiscalYear linked to this fiscal year.");
            var companyId = _tokenService.CompanyId().FirstOrDefault();
            // Now safe to extract
            var companySettingsId = currentFiscalYears.CompanySettingFiscalYears
                .Where(x => x.CompanyId == companyId)
                .Select(x => x.CompanySettingId)
                .FirstOrDefault();

            var runningFiscalYearInCompanySettingFy = (await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>()
                .FindBy(fy => fy.CompanySettingId == companySettingsId && !fy.IsClosed))
                .FirstOrDefault();

            if (runningFiscalYearInCompanySettingFy == null)
                throw new Exception("No Current CompanySettingsFiscalYear found.");


            var runningFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == runningFiscalYearInCompanySettingFy.FiscalYearId);

            var runningFiscalYearStartDate = runningFiscalYear?.StartDate.ToString("yyyy-MM-dd");

            var openingCompanyFiscalYearInCompanySettingsFy = (await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>()
                .FindBy(fy => fy.CompanySettingId == companySettingsId && fy.IsFiscalYearStarted))
                .FirstOrDefault(fy=>fy.IsFiscalYearStarted);

            if (openingCompanyFiscalYearInCompanySettingsFy == null)
                throw new Exception("No open CompanySettingsFiscalYear found.");


            var openingCompanyFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == openingCompanyFiscalYearInCompanySettingsFy.FiscalYearId);


            var openingCompanyFiscalYearStartDate = openingCompanyFiscalYear?.StartDate.ToString("yyyy-MM-dd");
            return Result<FiscalYearStartDateResponse>.Success(
                new FiscalYearStartDateResponse
                (
                    runningFiscalYearStartDate: runningFiscalYearStartDate,
                    openingCompanyFiscalYearStartDate: openingCompanyFiscalYearStartDate
                )
            );
   
        }

        public async Task<Result<List<GetSelectedFiscalYearQueryResponse>>> GetSelectedFiscalYear(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
         
                var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                      .GetConditionalAsync(
                          predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                          includes: fy => fy.CompanySettingFiscalYears
                      )).FirstOrDefault();

                if (currentFiscalYears == null)
                    throw new Exception("Fiscal year not found.");

                if (currentFiscalYears.CompanySettingFiscalYears == null || !currentFiscalYears.CompanySettingFiscalYears.Any())
                    throw new Exception("No CompanySettingsFiscalYear linked to this fiscal year.");
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                // Now safe to extract
                var companySettingsId = currentFiscalYears.CompanySettingFiscalYears
                    .Where(x => x.CompanyId == companyId)
                    .Select(x => x.CompanySettingId)
                    .FirstOrDefault();




         




                var selectedFiscalYear = await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>()
                    .FindBy(fy =>
                        fy.CompanySettingId == companySettingsId);

                var selectedFiscalYearList = await selectedFiscalYear
                    .Select(x => new GetSelectedFiscalYearQueryResponse(x.FiscalYearId, x.FyName))
                    .ToListAsync();


                var response = _mapper.Map<List<GetSelectedFiscalYearQueryResponse>>(selectedFiscalYearList);

                return Result<List<GetSelectedFiscalYearQueryResponse>>.Success(response);

  

        }

        public bool IsDateInFiscalYear(DateTime date, FiscalYears fiscalYear)
        {
            return date >= fiscalYear.StartDate && date <= fiscalYear.EndDate;
        }
    }
}
