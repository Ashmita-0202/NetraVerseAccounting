using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddModule;
using TN.Setup.Application.Setup.Command.AssignModulesToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetModulesByRoleId;
using TN.Setup.Application.Setup.Queries.Modules;
using TN.Setup.Application.Setup.Queries.ModulesById;
using TN.Setup.Application.Setup.Queries.NavigationByUser;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IModule
    {
        Task<Result<PagedResult<GetAllModulesResponse>>> GetAllModule(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddModuleResponse>> Add(AddModuleCommand addModuleCommand);
        Task<Result<UpdateModulesResponse>> Update(string modulesId, UpdateModulesCommand modulesCommand);
        Task<Result<UpdateAssignModulesResponse>> UpdateAssignModules(UpdateAssignModulesCommand assignModulesCommand);
        Task<Result<bool>> Delete(string modulesId, CancellationToken cancellationToken);
        Task<Result<GetModulesByIdResponse>> GetModulesById(string modulesId, CancellationToken cancellationToken = default);
        Task<Result<AssignModulesToRoleResponse>> AssignModulesToRole(AssignModulesToRoleCommand assignModulesToRoleCommand);
        Task<Result<List<GetModulesByRoleIdResponse>>> GetModulesByRoleId(string roleId, CancellationToken cancellationToken);
        Task<Result<List<NavigationByUserResponse>>> GetNavigationMenuByUser(string userId);

    }
}
