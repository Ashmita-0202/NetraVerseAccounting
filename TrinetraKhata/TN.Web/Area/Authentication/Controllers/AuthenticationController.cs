using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Drawing;
using System.Text;
using System.Text.Json;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Authentication.Application.Authentication.Commands.AddPermission.RequestMapper;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles.RequestMapper;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.Authentication.Commands.AddUser.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;
using TN.Authentication.Application.Authentication.Commands.AssignRoles.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.DeletePermission;
using TN.Authentication.Application.Authentication.Commands.DeleteRoles;
using TN.Authentication.Application.Authentication.Commands.DeleteUser;
using TN.Authentication.Application.Authentication.Commands.ForgetPassword;
using TN.Authentication.Application.Authentication.Commands.ForgetPassword.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.Authentication.Commands.Login.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.Authentication.Commands.Register.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.ResetPassword.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.Roles;
using TN.Authentication.Application.Authentication.Commands.Roles.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.UpdateDate;
using TN.Authentication.Application.Authentication.Commands.UpdateDate.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;
using TN.Authentication.Application.Authentication.Commands.UpdateUser.RequestCommandMapper;
using TN.Authentication.Application.Authentication.Queries.AllPermission;
using TN.Authentication.Application.Authentication.Queries.AllRoles;
using TN.Authentication.Application.Authentication.Queries.AllUsers;
using TN.Authentication.Application.Authentication.Queries.AssignableRoles;
using TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId;
using TN.Authentication.Application.Authentication.Queries.FilterUserByDate;
using TN.Authentication.Application.Authentication.Queries.PermissionById;
using TN.Authentication.Application.Authentication.Queries.RoleById;
using TN.Authentication.Application.Authentication.Queries.RoleByUserId;
using TN.Authentication.Application.Authentication.Queries.UserById;
using TN.Authentication.Application.Authentication.Queries.UserByRoleId;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Authentication.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]

    public class AuthenticationController : BaseController
    {
        private readonly IUserServices _accountServices;
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator,IUserServices accountServices, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager) 
        {
            _accountServices = accountServices;
            _mediator = mediator;
        }

        #region Permission
       
        #region AllPermission
        [HttpGet("all-permission")]
        public async Task<IActionResult> AllPermission([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AllPermissionQuery(paginationRequest);
            var permissionResult = await _mediator.Send(query);
            #region Switch Statement
            return permissionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(permissionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = permissionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(permissionResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };

            #endregion
        }

        #endregion
        #region DeletePermission
        [HttpDelete("DeletePermission/{id}")]

        public async Task<IActionResult> DeletePermission([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeletePermissionCommand(id);
            var deletePermissionResult = await _mediator.Send(command);
            #region Switch Statement
            return deletePermissionResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deletePermissionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deletePermissionResult.Errors),
                _ => BadRequest("Invalid Fields for delete permission")
            };

            #endregion
        }
        #endregion
        #region AddPermission
        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addPerission = await _mediator.Send(command);

            #region switch
            return addPerission switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(addPerission.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPerission.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addPerission.Errors),
                _ => BadRequest("Invalid permission name")

            };

            #endregion
        }

        #endregion
        #region UpdatePermission
        [HttpPatch("UpdatePermission/{id}")]

        public async Task<IActionResult> UpdatePermission([FromRoute] string id, [FromBody] UpdatePermissionRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updatePermissionResult = await _mediator.Send(command);
            #region Switch Statement
            return updatePermissionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePermissionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePermissionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePermissionResult.Errors),
                _ => BadRequest("Invalid Fields for Update Permission")
            };

            #endregion


        }
        #endregion

        #region PermissionById
        [HttpGet("Permission/{id}")]
        public async Task<IActionResult> GetPermissionById([FromRoute] string id)
        {
            var query = new GetPermissionByIdQuery(id);
            var permissionResult = await _mediator.Send(query);
            #region Switch Statement
            return permissionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(permissionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = permissionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(permissionResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #endregion

        #region AddPermissionToRoles
        [HttpPost("AddPermissionToRoles")]
        public async Task<IActionResult> AddPermissionToRoles([FromBody] AddPermissionToRolesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addPermissionToRoles = await _mediator.Send(command);

            #region switch
            return addPermissionToRoles switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(addPermissionToRoles.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPermissionToRoles.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addPermissionToRoles.Errors),
                _ => BadRequest("Invalid permissionId and RolesId")

            };

            #endregion
        }

        #endregion

        #region LogIn

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();

            var logInResult = await _mediator.Send(command);
            #region Switch Statement
            return logInResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(logInResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = logInResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(logInResult.Errors),
                _ => BadRequest("Invalid username and password")
            };
            #endregion
        }

        #endregion

        #region Registration
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var registrationResult = await _mediator.Send(command);
            #region Switch Statement
            return registrationResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Register), registrationResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = registrationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(registrationResult.Errors),
                _ => BadRequest("Invalid Fields for Register User")
            };

            #endregion


        }
        #endregion

        #region Password
        #region ResetPassword

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // Base64 URL-decode the token
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            return Ok(new ResetPasswordResponse (email = email, token = decodedToken,"" ));
        }

        #endregion

        #region ResetPassword
        [HttpPost("ResetPassword")]
     
        public async Task<IActionResult> ResetPassword([FromQuery]ResetPasswordRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var resetResult = await _mediator.Send(command);
            #region Switch Statement
            return resetResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Register), resetResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = resetResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(resetResult.Errors),
                _ => BadRequest("Invalid Fields for Reset Password")
            };

            #endregion


        }
        #endregion

        #region Forget Password

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest forgetPasswordRequest)
        {
            //Mapping command and request
            var command = forgetPasswordRequest.ToCommand();
            var forgetResult = await _mediator.Send(command);

            #region switch Statement
            return forgetResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(forgetResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = forgetResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(forgetResult.Errors),
                _ => BadRequest("Invalid email fields")


            };

            #endregion
        }


        #endregion
        #endregion

        #region Role
        #region AssignRoles
        [HttpPost("AssignRoles")]
        public async Task<IActionResult> AssignRoles([FromBody] AssignRolesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var assignRolesResult = await _mediator.Send(command);

            #region switch
            return assignRolesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(assignRolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignRolesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignRolesResult.Errors),
                _ => BadRequest("Invalid rolename and userId Fields")

            };

            #endregion
        }

        #endregion

        #region AssignableRolesToUser
        [HttpGet("AssignableRolesToUser")]
        public async Task<IActionResult> AssignableRolesToUser()
        {
            var query = new AssignableRolesQuery();
            var assignableRolesToUserResult = await _mediator.Send(query);
            #region Switch Statement
            return assignableRolesToUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(assignableRolesToUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignableRolesToUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignableRolesToUserResult.Errors),
                _ => BadRequest("Invalid userId")
            };

            #endregion


        }
        #endregion
        #region Create Roles
        [HttpPost("CreateRoles")]
        public async Task<IActionResult> CreateRoles([FromQuery] RolesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var roleResult = await _mediator.Send(command);
            #region Switch Statement
            return roleResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(CreateRoles), roleResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = roleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(roleResult.Errors),
                _ => BadRequest("Invalid rolename Fields")
            };
            #endregion
        }

        #endregion

        #region AllRoles
        [HttpGet("all-roles")]
        public async Task<IActionResult> AllRoles([FromQuery]PaginationRequest paginationRequest)
        {
            var query = new AllRolesQuery(paginationRequest);
            var rolesResult = await _mediator.Send(query);
            #region Switch Statement
            return rolesResult switch
            {
                { IsSuccess: true, Data: not null}=> new JsonResult(rolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = rolesResult.Message }),
                { IsSuccess: false, Errors: not null}=> HandleFailureResult(rolesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };

            #endregion
        }


        #region AssignableRolesByPermissionId
        [HttpGet("AssignableRolesByPermissionId/{permissionId}")]
        public async Task<IActionResult> AssignableRolesByPermissionId([FromRoute] string permissionId)
        {
            var query = new AssignableRolesByPermissionIdQuery(permissionId);
            var assignableRoleByPermission = await _mediator.Send(query);
            #region Switch Statement
            return assignableRoleByPermission switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(assignableRoleByPermission.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignableRoleByPermission.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignableRoleByPermission.Errors),
                _ => BadRequest($"Invalid {permissionId}")
            };

            #endregion
        }

        #endregion


        #region RoleById
        [HttpGet("Role/{RoleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute] string RoleId)
        {
            var query = new GetRolesByIdQuery(RoleId);
            var roleResult = await _mediator.Send(query);
            #region Switch Statement
            return roleResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(roleResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = roleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(roleResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region RoleByUserId
        [HttpGet("GetRole/{userId}")]
        public async Task<IActionResult> GetRoleByUserId([FromRoute] string userId)
        {
            var query = new GetRolesByUserIdQuery(userId);
            var roleResult = await _mediator.Send(query);
            #region Switch Statement
            return roleResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(roleResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = roleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(roleResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region DeleteRole
        [HttpDelete("DeleteRole/{id}")]

        public async Task<IActionResult> DeleteRole([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteRoleCommand(id);
            var deleteRoleResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteRoleResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteRoleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteRoleResult.Errors),
                _ => BadRequest("Invalid Fields for Add Role")
            };

            #endregion
        }
        #endregion
        #region UpdateRole
        [HttpPatch("UpdateRole/{id}")]

        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateRoleRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateRoleResult = await _mediator.Send(command);
            #region Switch Statement
            return updateRoleResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateRoleResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateRoleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateRoleResult.Errors),
                _ => BadRequest("Invalid Fields for Update Role")
            };

            #endregion

        }
        #endregion

        #endregion
        #endregion

        #region User

        #region AllUsers
        [HttpGet("all-users")]
        public async Task<IActionResult> AllUsers([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AllUserQuery(paginationRequest);
            var usersResult = await _mediator.Send(query);
            #region Switch Statement
            return usersResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(usersResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = usersResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(usersResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region UpdateUser
        [HttpPatch("UpdateUser/{userId}")]

        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UpdateUserRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(userId);
            var updateUserResult = await _mediator.Send(command);
            #region Switch Statement
            return updateUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateUserResult.Errors),
                _ => BadRequest("Invalid Fields for Update User")
            };

            #endregion


        }
        #endregion

        #region DeleteUser
        [HttpDelete("DeleteUser/{userId}")]

        public async Task<IActionResult> DeleteUser([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(userId);
            var deleteUserResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteUserResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteUserResult.Errors),
                _ => BadRequest("Invalid Fields for Add user")
            };

            #endregion
        }
        #endregion

        #region UserById
        [HttpGet("User/{UserId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string UserId)
        {
            var query = new GetUserByIdQuery(UserId);
            var userResult = await _mediator.Send(query);
            #region Switch Statement
            return userResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(userResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = userResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(userResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddUser
        [HttpPost("AddUser")]

        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addUserResult = await _mediator.Send(command);
            #region Switch Statement
            return addUserResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddUser), addUserResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addUserResult.Errors),
                _ => BadRequest("Invalid Fields for Add Company")
            };

            #endregion
        }
        #endregion

        #region UserByRoleId
        [HttpGet("GetUserByRole/{roleId}")]
        public async Task<IActionResult> GetUserByRoleId([FromRoute] string roleId)
        {
            var query = new GetUserByRoleIdQuery(roleId);
            var userResult = await _mediator.Send(query);

            #region Switch Statement
            return userResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(userResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),

                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = userResult.Message }),

                { IsSuccess: false, Errors: not null } => HandleFailureResult(userResult.Errors),

                _ => BadRequest("Invalid role ID or unexpected error.")
            };
            #endregion
        }
        #endregion

        #region FilterUserByDate
        [HttpGet("FilterUserByDate")]
        public async Task<IActionResult> GetUserFilter([FromQuery] FilterUserDTOs filterUserDTOs)
        {
            var query = new FilterUserByDateQuery(filterUserDTOs);
            var filterUserResult = await _mediator.Send(query);

            #region Switch Statement
            return filterUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterUserResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterUserResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region ExtendTrialPeriod
        [HttpPatch("ExtendTrialPeriod/{userId}")]
        public async Task<IActionResult> ExtendTrialPeriod([FromRoute] string userId,DateTime date, [FromBody] UpdateDateRequest request)
        {
           
            var command = request.ToCommand(userId,date);
            var result = await _mediator.Send(command);

         
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid request for extending trial period")
            };
        }
        #endregion

        #endregion
    }
}
