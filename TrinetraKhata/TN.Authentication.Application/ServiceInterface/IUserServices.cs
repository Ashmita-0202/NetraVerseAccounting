using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;
using TN.Authentication.Application.Authentication.Commands.ForgetPassword;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;
using TN.Authentication.Application.Authentication.Queries.AllPermission;
using TN.Authentication.Application.Authentication.Queries.AllRoles;
using TN.Authentication.Application.Authentication.Queries.AllUsers;
using TN.Authentication.Application.Authentication.Queries.AssignableRoles;
using TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId;
using TN.Authentication.Application.Authentication.Queries.FilterUserByDate;
using TN.Authentication.Application.Authentication.Queries.PermissionById;
using TN.Authentication.Application.Authentication.Queries.RoleByUserId;
using TN.Authentication.Application.Authentication.Queries.UserById;
using TN.Authentication.Application.Authentication.Queries.UserByRoleId;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.ServiceInterface
{
    public interface IUserServices
    {
        Task<Result<RegisterResponse>> RegisterUser(RegisterCommand registerCommand);
        Task<Result<LoginResponse>> Login(LoginCommand loginCommand);

        Task<Result<string>> CreateRoles(string rolename);
        Task<Result<AssignRolesResponse>> AssignMultipleRoles(AssignRolesCommand assignRolesCommand);

        Task<Result<ForgetPasswordResponse>> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand);
        Task<Result<ResetPasswordResponse>> ResetPassword(ResetPasswordCommand resetPasswordCommand);
        Task<Result<PagedResult<AllUserResponse>>> GetAllUsers(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<Result<List<AssignableRolesByPermissionIdResponse>>> AssignableRolesByPermissionId(string permissionId, CancellationToken cancellationToken);

        Task<Result<PagedResult<AllRolesResponse>>> GetAllRoles(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<Result<PagedResult<AllPermissionResponse>>> GetAllPermission(PaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task<Result<List<GetRolesByUserIdQueryResponse>>> GetRolesByUserId(string userId, CancellationToken cancellationToken = default);
        Task<Result<GetUserByIdResponse>> GetUserById(string userId, CancellationToken cancellationToken = default);

        Task<Result<UpdateUserResponse>> Update(string userId, UpdateUserCommand updateUserCommand);

        Task<Result<UpdateRoleResponse>> Update(string id, UpdateRoleCommand updateRoleCommand);
        Task<Result<bool>> Delete(string userId, CancellationToken cancellationToken);

        Task<Result<AddUserResponse>> AddUser(AddUserCommand addUserCommand);

        Task<Result<AddPermissionResponse>> AddPermission(AddPermissionCommand addPermissionCommand);
        Task<Result<bool>> DeletePermission(string id, CancellationToken cancellationToken);
        Task<Result<AddPermissionToRolesResponse>> AddPermissionToRoles(AddPermissionToRolesCommand addPermissionToRolesCommand);
        Task<Result<GetPermissionByIdQueryResponse>> GetPermissionById(string id, CancellationToken cancellationToken = default);
        Task<Result<List<AssignableRolesResponse>>> AssignableRoles();
        Task<Result<List<GetUserByRoleIdQueryResponse>>> GetUserByRoleId(string roleId, CancellationToken cancellationToken = default);
        Task<Result<UpdatePermissionResponse>> Update(string id, UpdatePermissionCommand updatePermissionCommand);
        Task<Result<IEnumerable<FilterUserByDateQueryResponse>>> GetUserFilter(FilterUserDTOs filterUserDTOs, CancellationToken cancellationToken);
    }
}
