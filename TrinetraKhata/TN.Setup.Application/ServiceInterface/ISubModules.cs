using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddSubModules;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Setup.Application.Setup.Command.UpdateAssignSubModules;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Command.UpdateSubModules;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.GetSubModulesById;
using TN.Setup.Application.Setup.Queries.GetSubModulesByModulesId;
using TN.Setup.Application.Setup.Queries.GetSubModulesByRoleId;
using TN.Setup.Application.Setup.Queries.ModulesById;
using TN.Setup.Application.Setup.Queries.SubModules;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface ISubModules
    {
        Task<Result<PagedResult<GetAllSubModulesResponse>>> GetAllSubModules(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddSubmodulesResponse>> Add(AddSubModulesCommand command);
        Task<Result<List<GetSubModulesByModulesIdResponse>>> GetSubModulesByModulesId(string modulesId, CancellationToken cancellationToken);
        Task<Result<UpdateSubModulesResponse>> Update(string subModulesId, UpdateSubModulesCommand updateSubModulesCommand);
        Task<Result<UpdateAssignSubModulesResponse>> UpdateAssignSubModules(UpdateAssignSubModulesCommand assignSubModulesCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);

        Task<Result<GetSubModulesByIdResponse>> GetSubModulesById(string id, CancellationToken cancellationToken = default);

        Task<Result<AssignSubModulesToRoleResponse>> AssignSubModulesToRole(AssignSubModulesToRoleCommand command);

        Task<Result<List<GetSubModulesByRoleIdResponse>>> GetSubModulesByRoleId(string roleId, CancellationToken cancellationToken);
    }
}
