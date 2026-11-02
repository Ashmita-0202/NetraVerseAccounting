using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter;
using TN.Inventory.Application.Inventory.Queries.FilterStockCenter;
using TN.Inventory.Application.Inventory.Queries.StockCenters;
using TN.Inventory.Application.Inventory.Queries.StockCentersById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;



namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class StockCenterService : IStockCenterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConvertHelper;

        public StockCenterService(IUnitOfWork unitOfWork,IDateConvertHelper dateConvertHelper,IGetUserScopedData getUserScopedData, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData= getUserScopedData;
            _dateConvertHelper = dateConvertHelper;
        }

        public async Task<Result<AddStockCenterResponse>> Add(AddStockCenterCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                    var stockCenterData = new StockCenter
                   (
                        newId,
                        command.Name,
                        command.Address,
                        companyId ?? "",
                        _tokenService.GetUserId(),
                        DateTime.Now



                    );

                    await _unitOfWork.BaseRepository<StockCenter>().AddAsync(stockCenterData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddStockCenterResponse>(stockCenterData);

                    if(resultDTOs == null)
                    {
                        throw new NotFoundException("Not Found Exception");
                    }
                    return Result<AddStockCenterResponse>.Success(resultDTOs);

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
                var stockCenter = await _unitOfWork.BaseRepository<StockCenter>().GetByGuIdAsync(id);
                if (stockCenter is null)
                {
                    return Result<bool>.Failure("NotFound", "StockCenter Cannot be Found");
                }
                _unitOfWork.BaseRepository<StockCenter>().Delete(stockCenter);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Stock Center having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllStockCenterQueryResponse>>> GetAllStockCenter(PaginationRequest paginationRequest, string? name, CancellationToken cancellationToken = default)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRoles = _tokenService.GetRole();

                var isSuperAdmin = userRoles == Role.SuperAdmin;

                IQueryable<StockCenter> stock = await _unitOfWork.BaseRepository<StockCenter>().GetAllAsyncWithPagination();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                IQueryable<StockCenter> filteredStockCenter;

                if (isSuperAdmin)
                {
                    filteredStockCenter = stock;
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filteredStockCenter = stock.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filteredStockCenter = stock.Where(x => x.CompanyId == companyId);
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    var lowerName = name.ToLower();
                    filteredStockCenter = filteredStockCenter
                        .Where(x => x.Name != null && x.Name.ToLower().Contains(lowerName));
                }

                filteredStockCenter = filteredStockCenter.OrderBy(x => x.Name);

                var pagedResult = await filteredStockCenter
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allResponse = _mapper.Map<PagedResult<GetAllStockCenterQueryResponse>>(pagedResult.Data);

                return Result<PagedResult<GetAllStockCenterQueryResponse>>.Success(allResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all stock center", ex);
            }

        }

        public async Task<Result<PagedResult<FilterStockCenterQueryResponse>>> GetFilterStockCenter(PaginationRequest paginationRequest, FilterStockCenterDto filterStockCenterDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (stockCenterQueryable, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StockCenter>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                var filterStockCenter = isSuperAdmin
                    ? stockCenterQueryable
                    : stockCenterQueryable.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (!string.IsNullOrWhiteSpace(filterStockCenterDto.startDate))
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockCenterDto.startDate);

                if (!string.IsNullOrWhiteSpace(filterStockCenterDto.endDate))
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockCenterDto.endDate);

                // Default to today’s data if no date provided
                if (startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = DateTime.Today;
                    endEnglishDate = DateTime.Today.AddDays(1).AddTicks(-1); // inclusive today
                }
                else if (startEnglishDate != null && endEnglishDate == null)
                {
                    endEnglishDate = startEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else if (endEnglishDate != null && startEnglishDate == null)
                {
                    startEnglishDate = endEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else
                {

                    endEnglishDate = endEnglishDate?.Date.AddDays(1).AddTicks(-1);
                }

                var userId = _tokenService.GetUserId();


                var stockCenterResult = await _unitOfWork.BaseRepository<StockCenter>().GetConditionalAsync(
               x =>
                //x.AdjustedBy == userId &&
                //(startEnglishDate == null || x.AdjustedAt >= startEnglishDate) &&
                //(endEnglishDate == null || x.AdjustedAt <= endEnglishDate) &&
                (isSuperAdmin || x.CompanyId == currentCompanyId) &&
                (string.IsNullOrEmpty(filterStockCenterDto.name) ||
                 x.Name.ToLower().Contains(filterStockCenterDto.name.ToLower()))
        );

                // Map to response
                var responseList = stockCenterResult
                    //.OrderByDescending(sa => sa.AdjustedAt)
                    .Select(sa => new FilterStockCenterQueryResponse(
                       sa.Id,
                       sa.Name,
                       sa.Address
                    ))
                    .ToList();

                // Pagination
                PagedResult<FilterStockCenterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;
                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStockCenterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStockCenterQueryResponse>
                    {
                        Items = responseList,
                        TotalItems = responseList.Count,
                        PageIndex = 1,
                        pageSize = responseList.Count
                    };
                }

                return Result<PagedResult<FilterStockCenterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Stock Center: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetStockQueryByIdResponse>> GetStockCenterById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var stockcenter = await _unitOfWork.BaseRepository<StockCenter>().GetByGuIdAsync(id);

                var stockcenterResponse = _mapper.Map<GetStockQueryByIdResponse>(stockcenter);

                return Result<GetStockQueryByIdResponse>.Success(stockcenterResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching stock center by using Id", ex);
            }
        }

        public async Task<Result<UpdateStockCenterResponse>> Update(string id, UpdateStockCenterCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateStockCenterResponse>.Failure("NotFound", "Please provide valid stock center id");
                    }

                    var stockCenterToBeUpdated = await _unitOfWork.BaseRepository<StockCenter>().GetByGuIdAsync(id);
                    if (stockCenterToBeUpdated is null)
                    {
                        return Result<UpdateStockCenterResponse>.Failure("NotFound", "stock center are not Found");
                    }

                    _mapper.Map(command, stockCenterToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateStockCenterResponse
                        (

                            stockCenterToBeUpdated.Name,
                            stockCenterToBeUpdated.Address

                        );

                    return Result<UpdateStockCenterResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating stock center");
                }
            }
        }
    }
}
