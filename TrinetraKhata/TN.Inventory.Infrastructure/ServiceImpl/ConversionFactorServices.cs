using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Inventory.Infrastructure.ServiceImpl
{
  public class ConversionFactorServices:IConversionFactorServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;

        public ConversionFactorServices(IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper,IUnitOfWork unitOfWork,IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _dateConvertHelper = dateConvertHelper;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
        }

        public async Task<Result<AddConversionFactorResponse>> AddConversionFactor(AddConversionFactorCommand addConversionFactorCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                    string newId = Guid.NewGuid().ToString();
                    var conversionFactorData = new ConversionFactor
                    (
                         newId,
                         addConversionFactorCommand.name,
                         addConversionFactorCommand.fromUnit,
                         addConversionFactorCommand.toUnit,
                         addConversionFactorCommand.conversionFactor,
                         DateTime.UtcNow,
                         userId,
                         "",
                         default,
                         companyId,
                         true

                    );

                    await _unitOfWork.BaseRepository<ConversionFactor>().AddAsync(conversionFactorData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddConversionFactorResponse>(conversionFactorData);


                    return Result<AddConversionFactorResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding conversion factors ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var conversionFactor = await _unitOfWork.BaseRepository<ConversionFactor>().GetByGuIdAsync(id);
                conversionFactor.IsActive = false;
                if (conversionFactor is null)
                {
                    return Result<bool>.Failure("NotFound", "conversion factor Cannot be Found");
                }

                _unitOfWork.BaseRepository<ConversionFactor>().Update(conversionFactor);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting conversion factor having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllConversionFactorQueryResponse>>> GetAllConversionFactor(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var (conversionFactors, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<ConversionFactor>();

                var filterConversionFactors = isSuperAdmin ? conversionFactors : conversionFactors.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault());

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filterConversionFactors = await _unitOfWork.BaseRepository<ConversionFactor>()
                        .FindBy(x =>x.IsActive && companyIds.Contains(x.CompanyId));
                }


                var conversionFactorsPagedResult = await filterConversionFactors.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allConversionFactorResponse = _mapper.Map<PagedResult<GetAllConversionFactorQueryResponse>>(conversionFactorsPagedResult.Data);

                return Result<PagedResult<GetAllConversionFactorQueryResponse>>.Success(allConversionFactorResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all conversion factors", ex);

            }
        }

        public async Task<Result<GetConversionFactorByIdResponse>> GetConversionFactorById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var conversionFactor = await _unitOfWork.BaseRepository<ConversionFactor>().GetByGuIdAsync(id);

                var conversionFactorResponse = _mapper.Map<GetConversionFactorByIdResponse>(conversionFactor);

                return Result<GetConversionFactorByIdResponse>.Success(conversionFactorResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching conversion factor by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterConversionFactorByDateQueryResponse>>> GetConversionFactorFilter(PaginationRequest paginationRequest, FilterConversionFactorDTOs filterConversionFactorDTOs)
        {
            try
            {
                var (conversionFilter, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<ConversionFactor>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();

                var filterFactor = isSuperAdmin
                    ? conversionFilter
                    : conversionFilter.Where(x =>x.IsActive && x.CompanyId == currentCompanyId || x.CompanyId == "");

                
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (filterConversionFactorDTOs.startDate != null)
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterConversionFactorDTOs.startDate);

                if (filterConversionFactorDTOs.endDate != null)
                {
                    var endDate = await _dateConvertHelper.ConvertToEnglish(filterConversionFactorDTOs.endDate);
                    endEnglishDate = endDate.Date.AddDays(1).AddTicks(-1); // include full day
                }

              
                var filterConversionFactors = filterFactor
                    .Where(x =>
                        (string.IsNullOrEmpty(filterConversionFactorDTOs.name) ||
                         x.Name.ToLower().Contains(filterConversionFactorDTOs.name.ToLower())) &&
                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate)
                    )
                    .OrderBy(x => x.Name)
                    .ToList();

               
                var unitsResponse = filterConversionFactors.Select(c => new FilterConversionFactorByDateQueryResponse(
                    c.Id,
                    c.Name,
                    c.FromUnit,
                    c.ToUnit,
                    c.ConversionFactors,
                    c.CreatedAt,
                    c.UserId,
                    c.UpdateBy,
                    c.UpdatedAt
                )).ToList();

               
                PagedResult<FilterConversionFactorByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = unitsResponse.Count();

                    var pagedItems = unitsResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterConversionFactorByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterConversionFactorByDateQueryResponse>
                    {
                        Items = unitsResponse,
                        TotalItems = unitsResponse.Count,
                        PageIndex = 1,
                        pageSize = unitsResponse.Count
                    };
                }

                return Result<PagedResult<FilterConversionFactorByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching units by date: {ex.Message}", ex);
            }


        }

        public async Task<Result<UpdateConversionFactorResponse>> UpdateConversionFactor(string id, UpdateConversionFactorCommand updateConversionFactorCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateConversionFactorResponse>.Failure("NotFound", "Please provide valid conversion factor id");
                    }
                    var userId = _tokenService.GetUserId();
                    var conversionFactorToBeUpdated = await _unitOfWork.BaseRepository<ConversionFactor>().GetByGuIdAsync(id);
                    conversionFactorToBeUpdated.UserId = userId;
                    conversionFactorToBeUpdated.CreatedAt = default;
                    conversionFactorToBeUpdated.UpdatedAt = DateTime.UtcNow;
                    if (conversionFactorToBeUpdated is null)
                    {
                        return Result<UpdateConversionFactorResponse>.Failure("NotFound", "conversion factor are not Found");
                    }

                    _mapper.Map(updateConversionFactorCommand, conversionFactorToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateConversionFactorResponse
                        (
                           conversionFactorToBeUpdated.Id,
                           conversionFactorToBeUpdated.FromUnit,
                           conversionFactorToBeUpdated.ToUnit,
                           conversionFactorToBeUpdated.ConversionFactors,
                           conversionFactorToBeUpdated.CreatedAt,
                           conversionFactorToBeUpdated.UserId,
                           userId,
                           conversionFactorToBeUpdated.UpdatedAt

                        );

                    return Result<UpdateConversionFactorResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating conversion Factors");
                }
            }
        }
    }
}
