using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.CompanyById;
using TN.Setup.Application.Setup.Queries.CompanyByInstitutionId;
using TN.Setup.Application.Setup.Queries.FilterCompanyByDate;
using TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class CompanyServices : ICompanyServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IFiscalYearService _fiscalYearService;

        public CompanyServices(IFiscalYearService fiscalYearService,IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper ,IUnitOfWork unitOfWork,IMapper mapper,ITokenService tokenService, IMemoryCacheRepository memoryCacheRepository ) 
        { 
            _memoryCacheRepository = memoryCacheRepository;
            _dateConvertHelper = dateConvertHelper;
            _unitOfWork =unitOfWork;
            _mapper=mapper;
            _tokenService=tokenService;
            _getUserScopedData=getUserScopedData;
            _fiscalYearService = fiscalYearService;
        }

        public async Task<Result<AddCompanyResponse>> Add(AddCompanyCommand addCompanyCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //await _memoryCacheRepository.RemoveAsync(CacheKeys.Company);
                    string userId = _tokenService.GetUserId();
                    string newId = Guid.NewGuid().ToString();
                    var companyData = new Company
                    (
                         newId,
                          addCompanyCommand.name,
                          addCompanyCommand.address,
                          addCompanyCommand.shortName,
                          addCompanyCommand.email,
                          addCompanyCommand.contactNumber,
                          addCompanyCommand.contactPerson,
                          addCompanyCommand.pan,
                          addCompanyCommand.imageUrl,
                          addCompanyCommand.isEnabled,
                          addCompanyCommand.institutionId,
                          DateTime.UtcNow,
                          userId,
                          default,
                          "",
                          addCompanyCommand.isDeleted,
                          addCompanyCommand.billNumberGenerationTypeForPurchase,
                          addCompanyCommand.billNumberGenerationTypeForSales

                     );

                    await _unitOfWork.BaseRepository<Company>().AddAsync(companyData);

                    //DateTime currentDate = DateTime.UtcNow;
                    //var currentFiscalYearId = await _fiscalYearService.GetFiscalYearIdForDateAsync( currentDate );

             
                    var companySettingsId = Guid.NewGuid().ToString();
                    var companySettings = new CompanySettings
                        (
                       companySettingsId,
                        true,
                        true,
                        CompanySettings.PurchaseReferencesType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        true,
                        CompanySettings.JournalReferencesType.Automatic,
                        CompanySettings.InventoryMethodType.FIFO,
                        newId,
                        addCompanyCommand.fiscalYearId,
                        false,
                        CompanySettings.TransactionNumberType.Automatic,
                        CompanySettings.TransactionNumberType.Automatic,
                        CompanySettings.TransactionNumberType.Automatic,
                        CompanySettings.TransactionNumberType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        CompanySettings.PurchaseSalesReturnNumberType.Automatic,
                        CompanySettings.PurchaseSalesReturnNumberType.Automatic,
                        CompanySettings.PurchaseSalesQuotationNumberType.Automatic,
                        CompanySettings.PurchaseSalesQuotationNumberType.Automatic,
                        null


                        );

                    await _unitOfWork.BaseRepository<CompanySettings>().AddAsync(companySettings);

                    var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>().GetByGuIdAsync(addCompanyCommand.fiscalYearId);

                    await _unitOfWork.SaveChangesAsync();
                    var companySettingsFiscalYear = new CompanySettingsFiscalYear
                        (
                        Guid.NewGuid().ToString(),
                        companySettingsId,
                        addCompanyCommand.fiscalYearId,
                        false,
                        "",
                        DateTime.UtcNow,
                        true,
                        currentFiscalYear.FyName,
                        newId,
                        true
                        );
                    await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>().AddAsync(companySettingsFiscalYear);


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCompanyResponse>(companyData);
                    return Result<AddCompanyResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Company", ex);

                }
            }


        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Company);

                var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(id);
                if (company is null)
                {
                    return Result<bool>.Failure("NotFound", "Company Cannot be Found");
                }

                _unitOfWork.BaseRepository<Company>().Delete(company);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting company having {id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllCompanyQueryResponse>>> GetAllCompany(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var institutionId = _tokenService.InstitutionId();

                var companiesQuery = await _unitOfWork.BaseRepository<Company>().FindBy(c => c.InstitutionId == institutionId);

                var companiesWithUsersQuery = companiesQuery.Include(c => c.UserCompany);

                    var companiesList = await companiesWithUsersQuery.ToListAsync(cancellationToken);

                    var result = companiesList.Select(c =>
                    {
                        var users = c.UserCompany?.Select(uc => new CompanyUserDto(uc.UserId)).ToList() ?? new List<CompanyUserDto>();

                        return new GetAllCompanyQueryResponse(
                            id: c.Id,
                            name: c.Name,
                            address: c.Address,
                            shortName: c.ShortName,
                            email: c.Email,
                            contactNumber: c.ContactNumber,
                            contactPerson: c.ContactPerson,
                            pan: c.PAN,
                            imageUrl: c.ImageUrl,
                            isEnabled: c.IsEnabled,
                            institutionId: c.InstitutionId,
                            createdDate: c.CreatedDate,
                            createdBy: c.CreatedBy,
                            modifiedDate: c.ModifiedDate,
                            modifiedBy: c.ModifiedBy ?? "",
                            isDeleted: c.IsDeleted,
                            billNumberGenerationTypeForPurchase: c.BillNumberGenerationTypeForPurchase,
                            billNumberGenerationTypeForSales: c.BillNumberGenerationTypeForSales,
                            Users: users
                        );
                    }).ToList();

                    int totalItems = result.Count;
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    var pagedItems = result
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var pagedResult = new PagedResult<GetAllCompanyQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };

                    return Result<PagedResult<GetAllCompanyQueryResponse>>.Success(pagedResult);
            }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching all companies with users", ex);
                }
              
        }

        public async Task<Result<GetCompanyByIdResponse>> GetCompanyById(string companyId, CancellationToken cancellationToken = default)
        {
            try
            {
               

                var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);

                var companyResponse = _mapper.Map<GetCompanyByIdResponse>(company);

                var currentFiscalYear = await _fiscalYearService.GetCurrentFiscalYearFromSettingsAsync();
                var updatedCompanyResponse = companyResponse with
                {
                    fyName = currentFiscalYear.FyName,
                };

                return Result<GetCompanyByIdResponse>.Success(updatedCompanyResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching company by id", ex);
            
            }
        }

        public async Task<Result<List<GetCompanyByInstitutionIdResponse>>> GetCompanyByInstitutionId(string institutionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var company = await _unitOfWork.BaseRepository<Company>().GetConditionalAsync(x => x.InstitutionId == institutionId);
                if (company is null)
                {
                    return Result<List<GetCompanyByInstitutionIdResponse>>.Failure("NotFound", "Company Data are not found");
                }

                var companyResponse = _mapper.Map<List<GetCompanyByInstitutionIdResponse>>(company);

                return Result<List<GetCompanyByInstitutionIdResponse>>.Success(companyResponse);


            }
            catch (Exception ex) 
            {

                throw new Exception("An error occurred while fetching Company By Institution Id", ex);
            
            }
        }

        public async Task<Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>> GetCompanyDetailsByInstitutionId(string institutionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var companies = (await _unitOfWork.BaseRepository<Company>().GetAllAsync())
                    .Where(c => c.InstitutionId == institutionId && !c.IsDeleted)
                    .ToList();

                if (!companies.Any())
                    return Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>.Failure("NotFound", "No companies found under this institution.");

                var purchases = await _unitOfWork.BaseRepository<PurchaseDetails>().GetAllAsync();
                var sales = await _unitOfWork.BaseRepository<SalesDetails>().GetAllAsync();

                var result = companies.Select(company =>
                {
                    var companyPurchases = purchases
                        .Where(p => p.CompanyId == company.Id && !p.IsDeleted)
                        .ToList();

                    var companySales = sales
                        .Where(s => s.CompanyId == company.Id)
                        .ToList();

                    var mapped = _mapper.Map<GetCompanyDetailsByCompanyIdQueryResponse>(company);

                    return mapped with
                    {
                        totalPurchaseBills = companyPurchases.Count(),
                        totalSalesBills = companySales.Count(),
                        totalPurchaseAmount = companyPurchases.Sum(p => p.GrandTotalAmount),
                        totalSalesAmount = companySales.Sum(s => s.GrandTotalAmount),
                        totalVatPurchase = companyPurchases.Sum(p => p.VatAmount ?? 0),
                        totalVatSales = companySales.Sum(s => s.VatAmount ?? 0)
                    };
                }).ToList();

                return Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company summaries by institution", ex);
            }

        }

        public async Task<Result<IEnumerable<FilterCompanyByDateQueryResponse>>> GetCompanyFilter(FilterCompanyDTOs filterCompanyDTOs, CancellationToken cancellationToken)
        {
            try
            {
                DateTime startEnglishDate = filterCompanyDTOs.startDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterCompanyDTOs.startDate);
                DateTime endEnglishDate = filterCompanyDTOs.endDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterCompanyDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                var filterCompany = await _unitOfWork.BaseRepository<Company>().GetConditionalAsync
                (
                x => (string.IsNullOrEmpty(filterCompanyDTOs.name) || x.Name.Contains(filterCompanyDTOs.name)) &&
                        x.CreatedDate >= startEnglishDate && x.CreatedDate <= endEnglishDate

               );


                var companyResponse = filterCompany.Select(comp => new FilterCompanyByDateQueryResponse(
                     comp.Id,
                     comp.Name,
                     comp.Address,
                     comp.ShortName,
                     comp.Email,
                     comp.ContactNumber,
                     comp.ContactPerson,
                     comp.PAN,
                     comp.ImageUrl,
                     comp.IsEnabled,
                     comp.InstitutionId


                ));


                return Result<IEnumerable<FilterCompanyByDateQueryResponse>>.Success(companyResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching company by date: {ex.Message}");
            }
        }

        public async Task<Result<UpdateCompanyResponse>> Update(string companyId, UpdateCompanyCommand updateCompanyCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Company);
                    if (companyId == null)
                    {
                        return Result<UpdateCompanyResponse>.Failure("NotFound", "Please provide valid organizationId");
                    }

                    var companyToBeUpdated = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);
                    if (companyToBeUpdated is null)
                    {
                        return Result<UpdateCompanyResponse>.Failure("NotFound", "LedgerGroup are not Found");
                    }

                    companyToBeUpdated.ModifiedBy = userId;
                    companyToBeUpdated.ModifiedDate = DateTime.UtcNow;
                    _mapper.Map(companyToBeUpdated, companyToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateCompanyResponse
                        (
                            companyId,
                            companyToBeUpdated.Name,
                            companyToBeUpdated.Address,
                            companyToBeUpdated.ShortName,
                            companyToBeUpdated.Email,
                            companyToBeUpdated.ContactNumber,
                            companyToBeUpdated.ContactPerson,
                            companyToBeUpdated.PAN,
                            companyToBeUpdated.ImageUrl,
                            companyToBeUpdated.IsEnabled,
                            companyToBeUpdated.InstitutionId,
                            companyToBeUpdated.CreatedDate,
                            companyToBeUpdated.CreatedBy,
                            companyToBeUpdated.ModifiedDate,
                            companyToBeUpdated.ModifiedBy,
                            companyToBeUpdated.IsDeleted,
                            companyToBeUpdated.BillNumberGenerationTypeForPurchase,
                            companyToBeUpdated.BillNumberGenerationTypeForSales

                        );

                    return Result<UpdateCompanyResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating organization", ex);
                }
            }

        }

        public async Task<Result<UpdateBillNumberStatusForPurchaseResponse>> Update(string id, Company.BillNumberGenerationType type)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                   
                    if (id == null)
                    {
                        return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure("NotFound", "Please provide valid companyId");
                    }

                    var billNumberToBeUpdated = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(id);
                    if (billNumberToBeUpdated is null)
                    {
                        return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure("NotFound", "BillNumbers are not Found");
                    }

                    billNumberToBeUpdated.ModifiedBy = userId;
                    billNumberToBeUpdated.ModifiedDate = DateTime.UtcNow;
                    _mapper.Map(billNumberToBeUpdated, billNumberToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateBillNumberStatusForPurchaseResponse
                        (
                            id,
                            billNumberToBeUpdated.BillNumberGenerationTypeForPurchase
                          
                        );

                    return Result<UpdateBillNumberStatusForPurchaseResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating Bill Number for Purchase", ex);
                }
            }
        }
   
    

    }
}
