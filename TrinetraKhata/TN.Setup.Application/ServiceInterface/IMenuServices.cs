using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.AssignMenusToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignMenu;
using TN.Setup.Application.Setup.Command.UpdateMenu;
using TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId;
using TN.Setup.Application.Setup.Queries.Menu;
using TN.Setup.Application.Setup.Queries.MenuById;
using TN.Setup.Application.Setup.Queries.MenuByRoleId;
using TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesandRolesId;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IMenuServices
    {
        Task<Result<PagedResult<GetAllMenuResponse>>> GetAllMenu(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddMenuResponse>> Add(AddMenuCommand command);
        Task<Result<UpdateMenuResponse>> Update(string menuId,UpdateMenuCommand updateMenuCommand);
        Task<Result<UpdateAssignMenuResponse>> UpdateAssignMenu(UpdateAssignMenuCommand updateAssignMenuCommand);
        Task<Result<GetMenuByIdResponse>> GetMenuById(string id, CancellationToken cancellationToken = default);

        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<List<GetMenuByRoleIdResponse>>> GetMenuByRoleId(string roleId, CancellationToken cancellationToken);
        Task<Result<AssignMenuToRolesResponse>> AssignMenuToRoles(AssignMenusToRoleCommands command, CancellationToken cancellationToken = default);
        Task<Result<List<GetMenuBySubModulesIdResponse>>> GetMenusBySubModulesId(string subModuleId, CancellationToken cancellationToken);

        Task<Result<List<MenuStatusBySubModulesAndRolesIdResponse>>> GetMenuStatusBySubModulesAndRoles(string subModuleId, string rolesId, CancellationToken cancellationToken);
        //Task<Result<IEnumerable<FilterMenuQueryResponse>>> GetMenuFilter(FilterMenuDTOs filterMenuDTOs, CancellationToken cancellationToken);


    }
}
