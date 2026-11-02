
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;
using TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Inventory.Application.Inventory.Queries.UnitsById;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;


namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class UnitsServices : IUnitsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConvertHelper;

        public UnitsServices(IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper,IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _getUserScopedData = getUserScopedData;
            _dateConvertHelper= dateConvertHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<AddUnitsResponse>> AddUnits(AddUnitsCommand addUnitsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();

                    if(userId is not null)
                    {
                        throw new NotFoundExceptions();
                    }
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                    string newId = Guid.NewGuid().ToString();
                    var unitsData = new Units
                   (
                        newId,
                        addUnitsCommand.name,
                        DateTime.UtcNow,
                        userId,
                        default,
                        "",
                        addUnitsCommand.isEnabled,
                        companyId
                    

                    );

                    await _unitOfWork.BaseRepository<Units>().AddAsync(unitsData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddUnitsResponse>(unitsData);
                    return Result<AddUnitsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var units = await _unitOfWork.BaseRepository<Units>().GetByGuIdAsync(id);
                if (units is null)
                {
                    return Result<bool>.Failure("NotFound", "Units Cannot be Found");
                }

                _unitOfWork.BaseRepository<Units>().Delete(units);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Units having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllUnitsByQueryResponse>>> GetAllUnits(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (units, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Units>();

                var filterUnits = isSuperAdmin ? units : units.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == " ");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filterUnits = await _unitOfWork.BaseRepository<Units>()
                        .FindBy(x => companyIds.Contains(x.CompanyId));
                }



                var unitsPagedResult = await filterUnits.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allUnitsResponse = _mapper.Map<PagedResult<GetAllUnitsByQueryResponse>>(unitsPagedResult.Data);

                return Result<PagedResult<GetAllUnitsByQueryResponse>>.Success(allUnitsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Units", ex);

            }
        }

        public async Task<Result<GetUnitsByIdQueryResponse>> GetUnitsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var units = await _unitOfWork.BaseRepository<Units>().GetByGuIdAsync(id);

                var unitsResponse = _mapper.Map<GetUnitsByIdQueryResponse>(units);

                return Result<GetUnitsByIdQueryResponse>.Success(unitsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching units by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterUnitsByDateQueryResponse>>> GetUnitsFilter(PaginationRequest paginationRequest,FilterUnitsDTOs filterUnitsDTOs)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Units>();

                var filterItems = isSuperAdmin ? ledger : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );

                DateTime startEnglishDate = filterUnitsDTOs.startDate == null
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterUnitsDTOs.startDate);
                DateTime endEnglishDate = filterUnitsDTOs.endDate == null
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterUnitsDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
              
                var filterUnits = filterItems.Where(
                     x =>
                            
                         (string.IsNullOrEmpty(filterUnitsDTOs.name) || x.Name.Contains(filterUnitsDTOs.name)) &&
                         (filterUnitsDTOs.startDate == default || x.CreatedAt >= startEnglishDate) &&
                         (filterUnitsDTOs.endDate == default || x.CreatedAt <= endEnglishDate)
                 ).OrderBy(x=>x.Name)
                 .ToList();


                var unitsResponse = filterUnits.Select(units => new FilterUnitsByDateQueryResponse(
                   units.Id,
                   units.Name,
                   units.CreatedAt,
                   units.UserId,
                   units.UpdatedAt,
                   units.UpdatedBy,
                   units.IsEnabled

                )).ToList();

                PagedResult<FilterUnitsByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = unitsResponse.Count();

                    var pagedItems = unitsResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterUnitsByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterUnitsByDateQueryResponse>
                    {
                        Items = unitsResponse.ToList(),
                        TotalItems = unitsResponse.Count(),
                        PageIndex = 1,
                        pageSize = unitsResponse.Count() // all items in one page
                    };
                }

                return Result<PagedResult<FilterUnitsByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching units by date: {ex.Message}");
            }
        }

        public async Task<Result<UpdateUnitsResponse>> UpdateUnits(string id, UpdateUnitsCommand updateUnitsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateUnitsResponse>.Failure("NotFound", "Please provide valid units id");
                    }
                    string userId = _tokenService.GetUserId();
                    var unitsToBeUpdated = await _unitOfWork.BaseRepository<Units>().GetByGuIdAsync(id);
                    unitsToBeUpdated.UpdatedBy = userId;
                    if (unitsToBeUpdated is null)
                    {
                        return Result<UpdateUnitsResponse>.Failure("NotFound", "Units are not Found");
                    }

                    
                    _mapper.Map(updateUnitsCommand, unitsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateUnitsResponse
                        (
                           unitsToBeUpdated.Id,
                           unitsToBeUpdated.Name,
                           unitsToBeUpdated.CreatedAt,
                           unitsToBeUpdated.UserId,
                           DateTime.UtcNow,
                           userId,
                           unitsToBeUpdated.IsEnabled

                        );

                    return Result<UpdateUnitsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating units");
                }
            }
        }
    }
}
