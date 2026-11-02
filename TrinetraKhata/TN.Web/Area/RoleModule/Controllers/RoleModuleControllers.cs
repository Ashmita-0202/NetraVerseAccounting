using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.AddMenu.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddModule;
using TN.Setup.Application.Setup.Command.AddModule.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddSubModules;
using TN.Setup.Application.Setup.Command.AddSubModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AssignMenusToRole;
using TN.Setup.Application.Setup.Command.AssignMenusToRole.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AssignModulesToRole;
using TN.Setup.Application.Setup.Command.AssignModulesToRole.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.DeleteMenu;
using TN.Setup.Application.Setup.Command.DeleteModule;
using TN.Setup.Application.Setup.Command.DeleteSubModule;
using TN.Setup.Application.Setup.Command.UpdateAssignMenu;
using TN.Setup.Application.Setup.Command.UpdateAssignMenu.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Setup.Application.Setup.Command.UpdateAssignModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateAssignSubModules;
using TN.Setup.Application.Setup.Command.UpdateAssignSubModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateMenu;
using TN.Setup.Application.Setup.Command.UpdateMenu.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Command.UpdateModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateSubModules;
using TN.Setup.Application.Setup.Command.UpdateSubModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId;
using TN.Setup.Application.Setup.Queries.GetModulesByRoleId;
using TN.Setup.Application.Setup.Queries.GetSubModulesById;
using TN.Setup.Application.Setup.Queries.GetSubModulesByModulesId;
using TN.Setup.Application.Setup.Queries.GetSubModulesByRoleId;
using TN.Setup.Application.Setup.Queries.Menu;
using TN.Setup.Application.Setup.Queries.MenuById;
using TN.Setup.Application.Setup.Queries.MenuByRoleId;
using TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesandRolesId;
using TN.Setup.Application.Setup.Queries.Modules;
using TN.Setup.Application.Setup.Queries.ModulesById;
using TN.Setup.Application.Setup.Queries.NavigationByUser;
using TN.Setup.Application.Setup.Queries.SubModules;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.RoleModule.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleModuleControllers : BaseController
    {
        private readonly IMediator _mediator;

        public RoleModuleControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }


        #region Modules
        #region AllModule
        [HttpGet("all-modules")]
        public async Task<IActionResult> AllModules([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllModulesQuery(paginationRequest);
            var modulesResult = await _mediator.Send(query);
            #region Switch Statement
            return modulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(modulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = modulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(modulesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region GetModulesByRoleId
        [HttpGet("GetModulesByRoleId/{RoleId}")]
        public async Task<IActionResult> GetModulesByRoleId([FromRoute] string RoleId)
        {
            var query = new GetModulesByRoleIdQuery(RoleId);
            var modulesByRoleIdResult = await _mediator.Send(query);
            #region Switch Statement
            return modulesByRoleIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(modulesByRoleIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = modulesByRoleIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(modulesByRoleIdResult.Errors),
                _ => BadRequest("Invalid roleId Fields")
            };
            #endregion
        }
        #endregion
        #region AddModules
        [HttpPost("AddModules")]

        public async Task<IActionResult> AddModules([FromBody] AddModuleRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return addModulesResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddModules), addModulesResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region AssignModules
        [HttpPost("AssignModules")]

        public async Task<IActionResult> AssignModules([FromBody] AssignModulesToRoleRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var assignModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return assignModulesResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddModules), assignModulesResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Assign Modules")
            };

            #endregion


        }
        #endregion

        #region ModulesById
        [HttpGet("Modules/{ModulesId}")]
        public async Task<IActionResult> GetByModulesId([FromRoute] string ModulesId)
        {
            var query = new GetModulesByIdQuery(ModulesId);
            var modulesResult = await _mediator.Send(query);
            #region Switch Statement
            return modulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(modulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = modulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(modulesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteModules
        [HttpDelete("Delete/{Id}")]

        public async Task<IActionResult> DeleteModules([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var command = new DeleteModuleCommand(Id);
            var deleteModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteModulesResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region UpdateModules
        [HttpPatch("UpdateModules/{Id}")]

        public async Task<IActionResult> UpdateModules([FromRoute] string Id, [FromBody] UpdateModulesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return updateModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Assign Modules")
            };

            #endregion


        }
        #endregion
        #endregion

        #region UpdateAssignModulesByModules
        [HttpPatch("UpdateAssignModulesByModules/{Id}")]

        public async Task<IActionResult> UpdateAssignModules([FromRoute] string Id, [FromBody] UpdateAssignModulesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateAssignModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return updateAssignModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateAssignModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateAssignModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateAssignModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Update Modules")
            };

            #endregion


        }
        #endregion

        #region UpdateAssignSubModulesBySubModules
        [HttpPatch("UpdateAssignSubModulesBySubModules/{Id}")]

        public async Task<IActionResult> UpdateAssignSubModules([FromRoute] string Id, [FromBody] UpdateAssignSubModulesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateAssignSubModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return updateAssignSubModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateAssignSubModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateAssignSubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateAssignSubModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Update Assign SubModules")
            };

            #endregion


        }
        #endregion

        #region UpdateAssignMenusByMenus
        [HttpPatch("UpdateAssignMenusByMenus/{Id}")]

        public async Task<IActionResult> UpdateAssignMenus([FromRoute] string Id, [FromBody] UpdateAssignMenuRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateAssignMenuResult = await _mediator.Send(command);
            #region Switch Statement
            return updateAssignMenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateAssignMenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateAssignMenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateAssignMenuResult.Errors),
                _ => BadRequest("Invalid Fields for Update Assign Menu")
            };

            #endregion


        }
        #endregion

        #region SubModule
        #region AssignSubModules
        [HttpPost("AssignSubModules")]

        public async Task<IActionResult> AssignSubModules([FromBody] AssignSubModulesToRoleRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var assignSubModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return assignSubModulesResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignSubModules), assignSubModulesResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignSubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignSubModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Assign Modules")
            };

            #endregion


        }
        #endregion

        #region AddSubModule
        [HttpPost("AddSubModule")]

        public async Task<IActionResult> AddSubModule([FromBody] AddSubModulesRequest request)
        {
            //Mapping command and request
            var command =request.ToCommand();
            var addSubModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return addSubModulesResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddModules), addSubModulesResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addSubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSubModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Add SubModules")
            };

            #endregion
        }
        #endregion

        #region AllSubModules
        [HttpGet("all-submodules")]
        public async Task<IActionResult> AllSubModules([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSubModulesQuery(paginationRequest);
            var SubModulesResult = await _mediator.Send(query);
            #region Switch Statement
            return SubModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(SubModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = SubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(SubModulesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region SubModulesById
        [HttpGet("SubModules/{id}")]
        public async Task<IActionResult> GetBySubModulesId([FromRoute] string id)
        {
            var query = new GetSubModulesByIdQuery(id);
            var subModulesResult = await _mediator.Send(query);
            #region Switch Statement
            return subModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(subModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = subModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(subModulesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region GetSubModulesByRoleId
        [HttpGet("GetSubModulesByRoleId/{roleId}")]
        public async Task<IActionResult> GetSubModulesByRoleId([FromRoute] string roleId)
        {
            var query = new GetSubModulesByRoleIdQuery(roleId);
            var submodulesByRoleIdResult = await _mediator.Send(query);
            #region Switch Statement
            return submodulesByRoleIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(submodulesByRoleIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = submodulesByRoleIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(submodulesByRoleIdResult.Errors),
                _ => BadRequest("Invalid roleId Fields")
            };
            #endregion
        }
        #endregion

        #region DeleteSubModule
        [HttpDelete("DeleteSubModules/{id}")]

        public async Task<IActionResult> DeleteSubModules([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSubModuleCommand(id);
            var deleteSubModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteSubModulesResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSubModulesResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region UpdateSubModules
        [HttpPatch("UpdateSubModules/{subModulesId}")]

        public async Task<IActionResult> UpdateSubModules([FromRoute] string subModulesId, [FromBody] UpdateSubModulesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(subModulesId);
            var updateSubModulesResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSubModulesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSubModulesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSubModulesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSubModulesResult.Errors),
                _ => BadRequest("Invalid Fields for update sub Modules")
            };

            #endregion


        }
        #endregion

        #region GetSubModulesByModulesId
        [HttpGet("GetSubModulesByModulesId/{modulesId}")]
        public async Task<IActionResult> GetSubModulesByModulesId([FromRoute] string modulesId)
        {
            var query = new GetSubModulesByModulesIdQuery(modulesId);
            var subModulesByModulesIdResult = await _mediator.Send(query);
            #region Switch Statement
            return subModulesByModulesIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(subModulesByModulesIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = subModulesByModulesIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(subModulesByModulesIdResult.Errors),
                _ => BadRequest("Invalid roleId Fields")
            };
            #endregion
        }
        #endregion
        #endregion

        #region Menu
        #region AssignMenus
        [HttpPost("AssignMenus")]

        public async Task<IActionResult> AssignMenus([FromBody] AssignMenusToRoleRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var assignMenuResult = await _mediator.Send(command);
            #region Switch Statement
            return assignMenuResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignMenus), assignMenuResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignMenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignMenuResult.Errors),
                _ => BadRequest("Invalid Fields for Assign Menus")
            };

            #endregion


        }
        #endregion
        #region AddMenu
        [HttpPost("AddMenu")]

        public async Task<IActionResult> AddMenu([FromBody] AddMenuRequest request)
        {
            //Mapping command and request


            var command = request.ToCommand();
            var addMenuResult = await _mediator.Send(command);
            #region Switch Statement
            return addMenuResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddMenu), addMenuResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addMenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addMenuResult.Errors),
                _ => BadRequest("Invalid Fields for Add Menu")
            };

            #endregion
        }
        #endregion

        #region AllMenu
        [HttpGet("all-menu")]
        public async Task<IActionResult> AllMenu([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllMenuQuery(paginationRequest);
            var MenuResult = await _mediator.Send(query);
            #region Switch Statement
            return MenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(MenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = MenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(MenuResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region MenuById
        [HttpGet("Menu/{id}")]
        public async Task<IActionResult> GetByMenuId([FromRoute] string id)
        {
            var query = new GetMenuByIdQuery(id);
            var menuResult = await _mediator.Send(query);
            #region Switch Statement
            return menuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(menuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = menuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(menuResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateMenu
        [HttpPatch("UpdateMenu/{menuId}")]

        public async Task<IActionResult> UpdateMenu([FromRoute] string menuId, [FromBody] UpdateMenuRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(menuId);
            var updateMenuResult = await _mediator.Send(command);
            #region Switch Statement
            return updateMenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateMenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateMenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateMenuResult.Errors),
                _ => BadRequest("Invalid Fields for update menu")
            };

            #endregion


        }
        #endregion

        #region DeleteMenu
        [HttpDelete("DeleteMenu/{id}")]

        public async Task<IActionResult> DeleteMenu([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteMenuCommand(id);
            var deleteMenuResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteMenuResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteMenuResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteMenuResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region GetMenuBySubmodulesId
        [HttpGet("GetMenuBySubmodulesId/{subModulesId}")]
        public async Task<IActionResult> GetMenuBySubmodulesId([FromRoute] string subModulesId)
        {
            var query = new GetMenuBySubModulesIdQuery(subModulesId);
            var menubySubModulesIdResult = await _mediator.Send(query);
            #region Switch Statement
            return menubySubModulesIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(menubySubModulesIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = menubySubModulesIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(menubySubModulesIdResult.Errors),
                _ => BadRequest("Invalid subModulesId Fields")
            };
            #endregion
        }
        #endregion

        #region GetMenuStatusBySubModulesAndRoles
        [HttpGet("GetMenuStatusBySubModulesAndRoles")]
        public async Task<IActionResult> GetMenuStatusBySubModulesAndRoles([FromQuery] string subModulesId, [FromQuery] string rolesId)
        {
            var query = new MenuStatusBySubModulesAndRolesIdQuery(subModulesId, rolesId);
            var menuStatusById = await _mediator.Send(query);
            #region Switch Statement
            return menuStatusById switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(menuStatusById.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = menuStatusById.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(menuStatusById.Errors),
                _ => BadRequest("Invalid subModulesIdand RolesId Fields")
            };
            #endregion
        }
        #endregion

        #region GetMenuByRoleId
        [HttpGet("GetMenuByRoleId/{RoleId}")]
        public async Task<IActionResult> GetMenuByRoleId([FromRoute] string RoleId)
        {
            var query = new GetMenuByRoleIdQuery(RoleId);
            var menuByRoleIdResult = await _mediator.Send(query);
            #region Switch Statement
            return menuByRoleIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(menuByRoleIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = menuByRoleIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(menuByRoleIdResult.Errors),
                _ => BadRequest("Invalid roleId Fields")
            };
            #endregion
        }
        #endregion
        #endregion 

        #region GetNavigationByUserId
        [HttpGet("GetNavigationByUser/{userId}")]
        public async Task<IActionResult> GetNavigationByUser([FromRoute] string userId)
        {
            var query = new NavigationByUserQuery(userId);
            var navigationByUserResult = await _mediator.Send(query);
            #region Switch Statement
            return navigationByUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(navigationByUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = navigationByUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(navigationByUserResult.Errors),
                _ => BadRequest("Invalid roleId Fields")
            };
            #endregion
        }
        #endregion

















    }
}
