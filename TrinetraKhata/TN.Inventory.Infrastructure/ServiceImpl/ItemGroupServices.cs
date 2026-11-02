using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Command.AddItemGroup;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate;
using TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate;
using TN.Inventory.Application.Inventory.Queries.ItemGroup;
using TN.Inventory.Application.Inventory.Queries.ItemGroupById;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class ItemGroupServices : IItemGroupServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly ITokenService _tokenService;

        public ItemGroupServices(IUnitOfWork unitOfWork,IGetUserScopedData getUserScopedData,IMapper mapper, IDateConvertHelper dateConvertHelper,ITokenService tokenService) 
        {
            _tokenService = tokenService;
            _unitOfWork=unitOfWork;
            _mapper=mapper;
            _dateConvertHelper=dateConvertHelper;
            _getUserScopedData=getUserScopedData;
        }

        public async Task<Result<AddItemGroupResponse>> AddItemGroup(AddItemGroupCommand addItemGroupCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                    string userId = _tokenService.GetUserId();

                    bool isDuplicate = await _unitOfWork.BaseRepository<ItemGroup>()
                        .AnyAsync(x=>x.Name == addItemGroupCommand.name && x.CompanyId == companyId);

                    if(isDuplicate)
                    {
                        return Result<AddItemGroupResponse>.Failure("ItemGroup with same name already Exists");
                    }

                    var itemGroupData = new ItemGroup
                    (
                    newId,
                    addItemGroupCommand.name,
                    addItemGroupCommand.description,
                    addItemGroupCommand.isPrimary,
                    addItemGroupCommand.itemsGroupId,
                    companyId,
                     userId,
                    DateTime.UtcNow,
                    "",
                    default,
                    true

                    );

                    await _unitOfWork.BaseRepository<ItemGroup>().AddAsync(itemGroupData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var resultDTOs = _mapper.Map<AddItemGroupResponse>(itemGroupData);
                    return Result<AddItemGroupResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ItemGroup ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var itemGroup = await _unitOfWork.BaseRepository<ItemGroup>().GetByGuIdAsync(id);
                itemGroup.IsActive = false;
                if (itemGroup is null)
                {
                    return Result<bool>.Failure("NotFound", "ItemGroup Cannot be Found");
                }

                _unitOfWork.BaseRepository<ItemGroup>().Update(itemGroup);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting ItemGroup having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllItemGroupByQueryResponse>>> GetAllItemGroup(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRoles = _tokenService.GetRole();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var isSuperAdmin = userRoles == Role.SuperAdmin;

                IQueryable<ItemGroup> itemGroups = await _unitOfWork.BaseRepository<ItemGroup>().GetAllAsyncWithPagination();

                // Filtering based on role
                var filteredItemGroups = isSuperAdmin
                    ? itemGroups
                    : itemGroups.Where(x => x.IsActive && x.CompanyId == companyId || x.CompanyId == "") ;

               
                if (!string.IsNullOrEmpty(institutionId) && companyId == null)
                {
                    filteredItemGroups = await _unitOfWork.BaseRepository<ItemGroup>()
                        .FindBy(x => companyIds.Contains(x.CompanyId));
                }

                var itemGroupPagedResult = await filteredItemGroups.AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allItemGroupResponse = _mapper.Map<PagedResult<GetAllItemGroupByQueryResponse>>(itemGroupPagedResult.Data);

                return Result<PagedResult<GetAllItemGroupByQueryResponse>>.Success(allItemGroupResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Item Groups", ex);
            }
        }

        public async Task<Result<GetItemGroupByIdQueryResponse>> GetItemGroupById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var itemGroup = await _unitOfWork.BaseRepository<ItemGroup>().GetByGuIdAsync(id);

                var itemGroupResponse = _mapper.Map<GetItemGroupByIdQueryResponse>(itemGroup);

                return Result<GetItemGroupByIdQueryResponse>.Success(itemGroupResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching itemGroup by using Id", ex);
            }
        }

        public async  Task<Result<PagedResult<FilterItemGroupByDateQueryResponse>>> GetItemGroupFilter(PaginationRequest paginationRequest,FilterItemGroupDTOs filterItemGroupDTOs)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<ItemGroup>();

                var filterItemGroup = isSuperAdmin ? ledger : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );

                DateTime startEnglishDate = filterItemGroupDTOs.startDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterItemGroupDTOs.startDate);
                DateTime endEnglishDate = filterItemGroupDTOs.endDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterItemGroupDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                var userId = _tokenService.GetUserId();
                var filterUnits = await _unitOfWork.BaseRepository<ItemGroup>().GetConditionalAsync(
                x => x.IsActive &&
                x.CreatedBy == userId &&
                (string.IsNullOrEmpty(filterItemGroupDTOs.name) || x.Name.Contains(filterItemGroupDTOs.name)) &&
                (filterItemGroupDTOs.startDate == default || x.CreatedAt >= startEnglishDate) &&
                         (filterItemGroupDTOs.endDate == default || x.CreatedAt <= endEnglishDate)
                 );


                var itemGroupResponse = filterUnits.Select(ig => new FilterItemGroupByDateQueryResponse(
                   ig.Id,
                    ig.Name,
                    ig.Description,
                    ig.IsPrimary,
                    ig.ItemsGroupId
                )).ToList();


                PagedResult<FilterItemGroupByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = itemGroupResponse.Count();

                    var pagedItems = itemGroupResponse
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterItemGroupByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterItemGroupByDateQueryResponse>
                    {
                        Items = itemGroupResponse.ToList(),
                        TotalItems = itemGroupResponse.Count(),
                        PageIndex = 1,
                        pageSize = itemGroupResponse.Count() // all items in one page
                    };
                }

                return Result<PagedResult<FilterItemGroupByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Item groups by date: {ex.Message}");
            }
        }

        public async Task<Result<UpdateItemGroupResponse>> UpdateItemGroup(string id, UpdateItemGroupCommand updateItemGroupCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateItemGroupResponse>.Failure("NotFound", "Please provide valid item group id");
                    }

                    var itemGroupToBeUpdated = await _unitOfWork.BaseRepository<ItemGroup>().GetByGuIdAsync(id);
                    if (itemGroupToBeUpdated is null)
                    {
                        return Result<UpdateItemGroupResponse>.Failure("NotFound", "item group are not Found");
                    }

                    _mapper.Map(updateItemGroupCommand, itemGroupToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateItemGroupResponse
                        (
                          itemGroupToBeUpdated.Id,
                          itemGroupToBeUpdated.Name,
                          itemGroupToBeUpdated.Description,
                          itemGroupToBeUpdated.IsPrimary,
                          itemGroupToBeUpdated.ItemsGroupId

                        );

                    return Result<UpdateItemGroupResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating item group");
                }
            }
        }

       
    }
}
