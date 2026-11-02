using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddSubModules;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Setup.Application.Setup.Command.UpdateAssignSubModules;
using TN.Setup.Application.Setup.Command.UpdateSubModules;
using TN.Setup.Application.Setup.Queries.GetSubModulesById;
using TN.Setup.Application.Setup.Queries.GetSubModulesByModulesId;
using TN.Setup.Application.Setup.Queries.GetSubModulesByRoleId;
using TN.Setup.Application.Setup.Queries.SubModules;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class SubModulesServices : ISubModules
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public SubModulesServices(IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _mapper = mapper;
            
        }
        public async Task<Result<AddSubmodulesResponse>> Add(AddSubModulesCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //await _memoryCacheRepository.RemoveAsync(CacheKeys.SubModules);
                    string newId = Guid.NewGuid().ToString();
                    var subModulesData = new SubModules(
                        newId,
                        command.name,
                        command.iconUrl,
                        command.TargetUrl,
                        command.modulesId,
                        command.rank,
                        command.isActive
                        );

                    await _unitOfWork.BaseRepository<SubModules>().AddAsync(subModulesData );
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSubmodulesResponse>(subModulesData);
                    return Result<AddSubmodulesResponse>.Success( resultDTOs );

                }catch(Exception ex)
                {
                    throw new Exception("An error occurred while adding sub modules");
                }
            }
        }

        public async Task<Result<AssignSubModulesToRoleResponse>> AssignSubModulesToRole(AssignSubModulesToRoleCommand command)
        {
            using(var scope = new TransactionScope( TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var submodulesId in command.submodulesId)
                    {
                        var rolesSubModulesExists = await _unitOfWork.BaseRepository<RoleSubModules>()
                            .AnyAsync(rm => rm.RoleId == command.roleId && rm.SubModulesId == submodulesId);

                        if (!rolesSubModulesExists)
                        {
                            var roleSubModules = new RoleSubModules()
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = command.roleId,
                                SubModulesId = submodulesId,
                                IsActive = command.isActive,
                                IsAssigned = true

                            };
                            await _unitOfWork.BaseRepository<RoleSubModules>().AddAsync(roleSubModules);

                        }

                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new AssignSubModulesToRoleResponse
                        (
                        roleId: command.roleId,
                        submodulesId: command.submodulesId.Select(id => id.ToString()).ToList(),
                        isActive: command.isActive,
                        isAssigned: true

                        );

                    return Result<AssignSubModulesToRoleResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while assign subModules", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.SubModules);
                var subModule = await _unitOfWork.BaseRepository<SubModules>().GetByGuIdAsync(id);
                if (subModule is null)
                {
                    return Result<bool>.Failure("NotFound", "SubModule Cannot be Found");
                }

                _unitOfWork.BaseRepository<SubModules>().Delete(subModule);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting submodules having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSubModulesResponse>>> GetAllSubModules(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = CacheKeys.SubModules;
                //var cacheData= await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllSubModulesResponse>>(cachekeys);
                //if(cacheData is not null) 
                //{
                //     return Result<PagedResult<GetAllSubModulesResponse>>.Success(cacheData);
                
                //}

                var subModules = await _unitOfWork.BaseRepository<SubModules>().GetAllAsyncWithPagination();
                var subModulesPagedResult = await subModules.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allSubModulesResponse = _mapper.Map<PagedResult<GetAllSubModulesResponse>>(subModulesPagedResult.Data);
                
                //await _memoryCacheRepository.SetAsync(cachekeys, allSubModulesResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)},cancellationToken);
                return Result<PagedResult<GetAllSubModulesResponse>>.Success(allSubModulesResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the SUbMOdules", ex);

            }
        }

        public async Task<Result<GetSubModulesByIdResponse>> GetSubModulesById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = $"GetSubModulesById{CacheKeys.SubModules}";
                //var cacheData = await _memoryCacheRepository.GetCacheKey <GetSubModulesByIdResponse>(cachekeys);

                var subModules = await _unitOfWork.BaseRepository<SubModules>().GetByGuIdAsync(id);

                var subModulesResponse = _mapper.Map<GetSubModulesByIdResponse>(subModules);

                //await _memoryCacheRepository.SetAsync(cachekeys,subModulesResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration= DateTimeOffset.Now.AddMinutes(30)}, cancellationToken);
                return Result<GetSubModulesByIdResponse>.Success(subModulesResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching SubModules  by using Id", ex);
            }
        }

        public async Task<Result<List<GetSubModulesByModulesIdResponse>>> GetSubModulesByModulesId(string modulesId, CancellationToken cancellationToken=default)
        {
            try
            {
                //var cachekeys= $"GetSubModulesByModulesId{CacheKeys.SubModules}";
                //var cacheData = await _memoryCacheRepository.GetCacheKey<List<GetSubModulesByModulesIdResponse>>(cachekeys);
                //if (cacheData is not null)
                //{ 
                //    return Result<List<GetSubModulesByModulesIdResponse>>.Success(cachekeys);
                //}
               

                var subModulesByMasterId = await _unitOfWork.BaseRepository<SubModules>()
                    .GetConditionalAsync(x => x.ModulesId == modulesId);

                if (subModulesByMasterId is null)
                {
                    return Result<List<GetSubModulesByModulesIdResponse>>.Failure("Not Found", " SubModules does not Found");

                }
                var resultDTOs = subModulesByMasterId.Select(x =>
                new GetSubModulesByModulesIdResponse(
                    x.Id,
                    x.Name,
                    x.IconUrl,
                    x.TargetUrl,
                    x.ModulesId,
                    x.Rank,
                    x.IsActive
                    )
                ).ToList();
                //await _memoryCacheRepository.SetAsync(cachekeys, resultDTOs,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)},cancellationToken);
                return Result<List<GetSubModulesByModulesIdResponse>>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching SubModules by {modulesId}", ex);
            }
        }

        public async Task<Result<List<GetSubModulesByRoleIdResponse>>> GetSubModulesByRoleId(string roleId, CancellationToken cancellationToken)
        {
            try
            {
                var subModulesWithRoles = await _unitOfWork.BaseRepository<RoleSubModules>()
                    .GetConditionalAsync(
                    x => x.RoleId == roleId,
                    query => query.Include(rm => rm.SubModules).Where(x=>x.IsAssigned== true)
                    );

                var resultsDTOs = subModulesWithRoles
                    .Where(rm => rm.SubModules is not null)
                    .Select(rm => new GetSubModulesByRoleIdResponse
                    (
                       
                        rm.SubModules.Id,
                        rm.SubModules.Name,
                        rm.SubModules.IconUrl,
                        rm.SubModules.TargetUrl,
                        rm.SubModules.ModulesId,
                        rm.SubModules.Rank,
                        rm.IsActive
                        )).ToList();
              

                return Result<List<GetSubModulesByRoleIdResponse>>.Success(resultsDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting submodules by RoleId");
            }
        }

        public async Task<Result<UpdateSubModulesResponse>> Update(string subModulesId, UpdateSubModulesCommand updateSubModulesCommand)
        {
            try
            {
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.SubModules);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrEmpty(subModulesId))
                    {
                        return Result<UpdateSubModulesResponse>.Failure("NotFound", "Please provide valid subModulesId");
                    }

                    var subModulesToBeUpdated = await _unitOfWork.BaseRepository<SubModules>().GetByGuIdAsync(subModulesId);


                    if (subModulesToBeUpdated is null)
                    {
                        return Result<UpdateSubModulesResponse>.Failure("NotFound", "Modules are not Found");
                    }

                    subModulesToBeUpdated.ModulesId = updateSubModulesCommand.moduleId; 

                    _mapper.Map(updateSubModulesCommand, subModulesToBeUpdated);

                    _unitOfWork.BaseRepository<SubModules>().Update(subModulesToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSubModulesResponse
                        (
                        subModulesId,
                        subModulesToBeUpdated.Name,
                        subModulesToBeUpdated.IconUrl,
                        subModulesToBeUpdated.TargetUrl,
                        subModulesToBeUpdated.ModulesId,
                        subModulesToBeUpdated.Rank,
                        subModulesToBeUpdated.IsActive
                        );

                    return Result<UpdateSubModulesResponse>.Success(resultResponse);

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating modules", ex);
            }
        }

        public async Task<Result<UpdateAssignSubModulesResponse>> UpdateAssignSubModules(UpdateAssignSubModulesCommand assignSubModulesCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(assignSubModulesCommand.subModulesId) && (string.IsNullOrEmpty(assignSubModulesCommand.roleId)))
                    {
                        return Result<UpdateAssignSubModulesResponse>.Failure("NotFound", $"Please provide valid {assignSubModulesCommand.roleId} and {assignSubModulesCommand.subModulesId}");
                    }

                    var assignSubModulesToBeUpdate = await _unitOfWork.BaseRepository<RoleSubModules>()
                        .GetSingleAsync(x => x.SubModulesId == assignSubModulesCommand.subModulesId && x.RoleId == assignSubModulesCommand.roleId);

                    assignSubModulesToBeUpdate.IsActive = assignSubModulesCommand.isActive;
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateAssignSubModulesResponse
                      (
                      assignSubModulesToBeUpdate.SubModulesId,
                      assignSubModulesToBeUpdate.RoleId,
                      assignSubModulesToBeUpdate.IsActive
                      );

                    return Result<UpdateAssignSubModulesResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception($"An error occurred while Update AssignModule by {assignSubModulesCommand.subModulesId} and {assignSubModulesCommand.roleId}", ex);
                };
            }
        }
    }
    
}
