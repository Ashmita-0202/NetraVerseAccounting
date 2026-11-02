using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.AssignMenusToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignMenu;
using TN.Setup.Application.Setup.Command.UpdateMenu;
using TN.Setup.Application.Setup.Queries.FilterMenuByDate;
using TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId;
using TN.Setup.Application.Setup.Queries.Menu;
using TN.Setup.Application.Setup.Queries.MenuById;
using TN.Setup.Application.Setup.Queries.MenuByRoleId;
using TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesandRolesId;
using TN.Setup.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class MenuServices : IMenuServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDateConvertHelper _dateConvertHelper;

        public MenuServices(IMapper mapper, IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, RoleManager<IdentityRole> roleManager, IDateConvertHelper dateConvertHelper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _roleManager = roleManager;
            _dateConvertHelper= dateConvertHelper;


        }
        public async Task<Result<AddMenuResponse>> Add(AddMenuCommand addMenuCommand)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();

                    var menuData = new Menu(
                        newId,
                        addMenuCommand.Name,
                        addMenuCommand.TargetUrl,
                        addMenuCommand.IconUrl,
                        addMenuCommand.SubModulesId,
                        addMenuCommand.Rank,
                        addMenuCommand.IsActive
                        );

                    await _unitOfWork.BaseRepository<Menu>().AddAsync( menuData );
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddMenuResponse>(menuData);
                    return Result<AddMenuResponse>.Success(resultDTOs );


                }catch(Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding menu", ex);
                }
            }
        }

        public async Task<Result<AssignMenuToRolesResponse>> AssignMenuToRoles(AssignMenusToRoleCommands command, CancellationToken cancellationToken = default)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach(var menuId in command.menusId)
                    {
                        var rolesMenuExists = await _unitOfWork.BaseRepository<RoleMenus>()
                            .AnyAsync(rm=>rm.RoleId == command.roleId && rm.MenusId == menuId);

                        if(!rolesMenuExists)
                        {
                            var roleMenu = new RoleMenus()
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = command.roleId,
                                MenusId = menuId,
                                IsActive = command.isActive,
                                IsAssigned = true
                            };
                            await _unitOfWork.BaseRepository<RoleMenus>().AddAsync(roleMenu);

                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new AssignMenuToRolesResponse
                        (
                        roleId: command.roleId,
                        menusId: command.menusId.Select(id => id.ToString()).ToList(),
                        isActive: command.isActive,
                        isAssigned: true

                        );

                    return Result<AssignMenuToRolesResponse>.Success(resultDTOs);

                }
                catch(Exception ex)
                {
                    scope.Dispose();
                    throw new Exception($"An error occured while assign men to {command.roleId}", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.Menu);
                var menu = await _unitOfWork.BaseRepository<Menu>().GetByGuIdAsync(id);
                if (menu is null)
                {
                    return Result<bool>.Failure("NotFound", "Module Cannot be Found");
                }

                _unitOfWork.BaseRepository<Menu>().Delete(menu);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting menu having {id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllMenuResponse>>> GetAllMenu(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = CacheKeys.Menu;
                //var cacheData= await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllMenuResponse>>(cachekeys);
                //if (cacheData == null) 
                //{
                //    return Result<PagedResult<GetAllMenuResponse>>.Success(cacheData);
                //}
                var menu = await _unitOfWork.BaseRepository<Menu>().GetAllAsyncWithPagination();
                var menuPagedResult = await menu.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allMenuResponse = _mapper.Map<PagedResult<GetAllMenuResponse>>(menuPagedResult.Data);
      
                //await _memoryCacheRepository.SetAsync(cachekeys, allMenuResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                //{
                //    AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)
                //},cancellationToken);

                return Result<PagedResult<GetAllMenuResponse>>.Success(allMenuResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the Menu", ex);

            }
        }

        public async Task<Result<GetMenuByIdResponse>> GetMenuById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = $"GetMenuById{CacheKeys.Menu}";
                //var cacheData=await _memoryCacheRepository.GetCacheKey<GetMenuByIdResponse>(cachekeys);
                //if(cacheData is not null) 
                //{
                //    return Result<GetMenuByIdResponse>.Success(cacheData);
                //}
                var menu= await _unitOfWork.BaseRepository<Menu>().GetByGuIdAsync(id);
                
                var menuResponse = _mapper.Map<GetMenuByIdResponse>(menu);

                //await _memoryCacheRepository.SetAsync(cachekeys,menuResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)},cancellationToken);
                return Result<GetMenuByIdResponse>.Success(menuResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Menu  by using Id", ex);
            }
        }

        public async Task<Result<List<GetMenuByRoleIdResponse>>> GetMenuByRoleId(string roleId, CancellationToken cancellationToken)
        {
            try
            {
                var menuWithRoles = await _unitOfWork.BaseRepository<RoleMenus>()
                    .GetConditionalAsync(
                    x => x.RoleId == roleId,
                    query => query.Include(rm => rm.Menu).Where(x=>x.IsAssigned==true)
                    );

                var resultsDTOs = menuWithRoles
                    .Where(rm => rm.Menu is not null)
                    .Select(rm => new GetMenuByRoleIdResponse
                    (
                       rm.Menu.Id,
                       rm.Menu.Name,
                       rm.Menu.TargetUrl,
                       rm.Menu.IconUrl,
                       rm.Menu.SubModulesId,
                       rm.Menu.Rank,
                       rm.IsActive
                        )).ToList();


                return Result<List<GetMenuByRoleIdResponse>>.Success(resultsDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting submodules by RoleId");
            }
        }

        //public async Task<Result<IEnumerable<FilterMenuQueryResponse>>> GetMenuFilter(FilterMenuDTOs filterMenuDTOs, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        DateTime startEnglishDate = filterMenuDTOs.startDate == default
        //        ? DateTime.Today
        //            : await _dateConvertHelper.ConvertToEnglish(filterMenuDTOs.startDate);
        //        DateTime endEnglishDate = filterMenuDTOs.endDate == default
        //        ? DateTime.Today
        //            : await _dateConvertHelper.ConvertToEnglish(filterMenuDTOs.endDate);

        //        endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
        //        var filterMenu = await _unitOfWork.BaseRepository<Menu>().GetConditionalAsync
        //        (
        //          x => (string.IsNullOrEmpty(filterMenuDTOs.name) || x.Name.Contains(filterMenuDTOs.name)) &&
        //                x.CreatedDate >= startEnglishDate && x.CreatedDate <= endEnglishDate

        //       );


        //        var menuResponse = filterMenu.Select(comp => new FilterMenuQueryResponse(
                     
        //          comp.name,
        //          comp.targetUrl,
        //          comp.iconUrl,
        //          comp.role,
        //          comp.subModuleId,
        //          comp.rank,
        //          comp.isActiove


        //        ));


        //        return Result<IEnumerable<FilterMenuQueryResponse>>.Success(menuResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred while fetching menu by date: {ex.Message}");
        //    }

        //}

        public async Task<Result<List<GetMenuBySubModulesIdResponse>>> GetMenusBySubModulesId(string subModuleId, CancellationToken cancellationToken)
        {
            try
            {
                //var cachekeys = $"GetMenuBySubModulesId{CacheKeys.Menu}";
                //var cacheData= await _memoryCacheRepository.GetCacheKey<List<GetMenuBySubModulesIdResponse>>(cachekeys);
                //if (cacheData is not null) 
                //{ 
                //  return Result<List<GetMenuBySubModulesIdResponse>>.Success(cacheData);
                //}

                var menus = await _unitOfWork.BaseRepository<Menu>()
                    .GetConditionalAsync(
                    x=>x.SubModulesId == subModuleId,
                    menu => menu.Include(sm=>sm.SubModules)
                    );

                var resultDTOs = menus.Select(menu => new GetMenuBySubModulesIdResponse
                (
                    menu.Id,
                    menu.Name,
                    menu.TargetUrl,
                    menu.IconUrl,
                    menu.SubModulesId,
                    menu.Rank,
                    menu.IsActive

                 )).ToList();

                //await _memoryCacheRepository.SetAsync(cachekeys, resultDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                //{
                //    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                //}, cancellationToken);
                return Result<List<GetMenuBySubModulesIdResponse>>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting Menus By {subModuleId}",ex);
            }
        }

        public async Task<Result<List<MenuStatusBySubModulesAndRolesIdResponse>>> GetMenuStatusBySubModulesAndRoles(string subModuleId, string rolesId, CancellationToken cancellationToken)
        {
            try
            {
                var menuWithRoles = await _unitOfWork.BaseRepository<RoleMenus>()
                    .GetConditionalAsync(
                    x => x.RoleId == rolesId,
                    query => query.Include(rm => rm.Menu).Where(x => x.IsAssigned == true)
                    );

                var resultsDTOs = menuWithRoles
                    .Where(rm => rm.Menu is not null && rm.Menu.SubModulesId == subModuleId)
                    .Select(rm => new MenuStatusBySubModulesAndRolesIdResponse
                    (
                        rm.IsActive,
                       rm.Menu.Name
                        )).ToList();


                return Result<List<MenuStatusBySubModulesAndRolesIdResponse>>.Success(resultsDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting submodules by RoleId");
            }
        }

        public async Task<Result<UpdateMenuResponse>> Update(string menuId, UpdateMenuCommand updateMenuCommand)
        {
            try
            {
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.Menu);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrEmpty(menuId))
                    {
                        return Result<UpdateMenuResponse>.Failure("NotFound", "Please provide valid menuId");
                    }

                    var menuToBeUpdated = await _unitOfWork.BaseRepository<Menu>().GetByGuIdAsync(menuId);
                    if (menuToBeUpdated is null)
                    {
                        return Result<UpdateMenuResponse>.Failure("NotFound", "Menu are not Found");
                    }

                    _mapper.Map(updateMenuCommand, menuToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateMenuResponse
                        (
                                menuId,
                                menuToBeUpdated.Name,
                                 menuToBeUpdated.TargetUrl,
                                  menuToBeUpdated.IconUrl,
                                 menuToBeUpdated.SubModulesId,
                               menuToBeUpdated.Rank,
                                menuToBeUpdated.IsActive
                        );

                    return Result<UpdateMenuResponse>.Success(resultResponse);

                }

            } catch(Exception ex)
           
            {
                throw new Exception($"An error occurred while updating menu", ex);
            }
        }

        public async Task<Result<UpdateAssignMenuResponse>> UpdateAssignMenu(UpdateAssignMenuCommand updateAssignMenuCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(updateAssignMenuCommand.menuId) && (string.IsNullOrEmpty(updateAssignMenuCommand.roleId)))
                    {
                        return Result<UpdateAssignMenuResponse>.Failure("NotFound", $"Please provide valid {updateAssignMenuCommand.roleId} and {updateAssignMenuCommand.menuId}");
                    }

                    var assignMenusToBeUpdate = await _unitOfWork.BaseRepository<RoleMenus>()
                        .GetSingleAsync(x => x.MenusId == updateAssignMenuCommand.menuId && x.RoleId == updateAssignMenuCommand.roleId);

                    assignMenusToBeUpdate.IsActive = updateAssignMenuCommand.isActive;
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateAssignMenuResponse
                      (
                      assignMenusToBeUpdate.MenusId,
                      assignMenusToBeUpdate.RoleId,
                      assignMenusToBeUpdate.IsActive
                      );

                    return Result<UpdateAssignMenuResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception($"An error occurred while Update AssignMenu by {updateAssignMenuCommand.menuId} and {updateAssignMenuCommand.roleId}", ex);
                };
            }
        }
    }
    
}
