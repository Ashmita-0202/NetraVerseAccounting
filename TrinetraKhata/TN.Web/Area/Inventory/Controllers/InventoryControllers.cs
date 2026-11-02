using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text.Json;
using TN.Account.Application.Account.Command.DeleteStockTransferDetails;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddItemGroup;
using TN.Inventory.Application.Inventory.Command.AddItemGroup.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.AddItems.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.AddStockCenter.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.AddUnits.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.DeleteConversionFactor;
using TN.Inventory.Application.Inventory.Command.DeleteItem;
using TN.Inventory.Application.Inventory.Command.DeleteItemGroup;
using TN.Inventory.Application.Inventory.Command.DeleteStockAdjustment;
using TN.Inventory.Application.Inventory.Command.DeleteStockCenter;
using TN.Inventory.Application.Inventory.Command.DeleteUnits;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateItem;
using TN.Inventory.Application.Inventory.Command.UpdateItem.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;
using TN.Inventory.Application.Inventory.Command.UpdateUnits.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Queries.ConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.FilterStockCenter;
using TN.Inventory.Application.Inventory.Queries.FilterStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs;
using TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetAllStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQuantityByItemId;
using TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById;
using TN.Inventory.Application.Inventory.Queries.InventoriesLogsById;
using TN.Inventory.Application.Inventory.Queries.InventoryByItem;
using TN.Inventory.Application.Inventory.Queries.ItemGroup;
using TN.Inventory.Application.Inventory.Queries.ItemGroupById;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId;
using TN.Inventory.Application.Inventory.Queries.StockCenters;
using TN.Inventory.Application.Inventory.Queries.StockCentersById;
using TN.Inventory.Application.Inventory.Queries.StockExpiryNotification;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Inventory.Application.Inventory.Queries.UnitsById;
using TN.Setup.Application.Setup.Command.UpdateCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany.RequestCommandMapper;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Inventory.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryControllers : BaseController
    {
        private readonly IMediator _mediator;

        public InventoryControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator=mediator;

        }

        #region Notification for Stock Expiry

        #region StockExpiryNotification
        [HttpGet("StockExpiryNotification")]
        public async Task<IActionResult> StockExpiryNotification([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new StockExpiryNotificationQuery(paginationRequest);
            var stockExpiryResult = await _mediator.Send(query);
            #region Switch Statement
            return stockExpiryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(stockExpiryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockExpiryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockExpiryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion


        #endregion

        #region StockTransferDetails

        #region AddStockTransferDetails
        [HttpPost("AddStockTransferDetails")]

        public async Task<IActionResult> AddStockTransferDetails([FromBody] AddStockTransferDetailsRequest request)
        {
            //Mapping command and request
            var command =request.ToAddStockTransferCommand();
            var addStockTransferDetails = await _mediator.Send(command);
            #region Switch Statement
            return addStockTransferDetails switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddItemGroup), addStockTransferDetails.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addStockTransferDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addStockTransferDetails.Errors),
                _ => BadRequest("Invalid Fields for Add StockTransferDetails")
            };

            #endregion
        }
        #endregion

        #region AllStockTransferDetails
        [HttpGet("all-StokcTransferDetails")]
        public async Task<IActionResult> AllStokcTransferDetails([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllStockTransferDetailsQuery(paginationRequest);
            var allResult = await _mediator.Send(query);
            #region Switch Statement
            return allResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteStocktransferDetails
        [HttpDelete("DeleteStocktransferDetails/{id}")]

        public async Task<IActionResult> DeleteStocktransferDetails([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteStockTransferDetailsCommand(id);
            var deleteResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteResult.Errors),
                _ => BadRequest("Invalid Fields for Delete stockTransfer Details")
            };

            #endregion
        }
        #endregion

        #region StockTransferDetailsById
        [HttpGet("StockTransferDetailsBy/{id}")]
        public async Task<IActionResult> StockTransferDetailsById([FromRoute] string id)
        {
            var query = new GetStockTransferDetailsByIdQuery(id);
            var stockTransferResult = await _mediator.Send(query);
            #region Switch Statement
            return stockTransferResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(stockTransferResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockTransferResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockTransferResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterUnitsByDate
        [HttpGet("Filter-StockTransferDetails")]
        public async Task<IActionResult> FilterStockTransferDetails([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterStockTransferDetailsDto filterStockTransferDetailsDto)
        {
            var query = new FilterStockTransferDetailsQuery(paginationRequest, filterStockTransferDetailsDto);
            var filterResult = await _mediator.Send(query);

            #region Switch Statement
            return filterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region UpdateUnits
        [HttpPatch("UpdateStockTransferDetails/{id}")]

        public async Task<IActionResult> UpdateStockTransferDetails([FromRoute] string id, [FromBody] UpdateStockTransferDetailsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateResult.Errors),
                _ => BadRequest("Invalid Fields for Update StockTransfer Details")
            };

            #endregion


        }
        #endregion


        #endregion

        #region ItemByStockCenter
        [HttpGet("ItemByStockCenter/{StockCenterId}")]
        public async Task<IActionResult> ItemByStockCenter([FromRoute] string StockCenterId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetItemByStockCenterQuery(StockCenterId, paginationRequest);
            var itemByStockCenter = await _mediator.Send(query);
            #region Switch Statement
            return itemByStockCenter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemByStockCenter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemByStockCenter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(itemByStockCenter.Errors),
                _ => BadRequest("Invalid ledgerId Fields")
            };
            #endregion

        }
        #endregion
        #region Units
        #region AllUnits
        [HttpGet("all-Units")]
        public async Task<IActionResult> AllUnits([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllUnitsByQuery(paginationRequest);
            var unitsResult = await _mediator.Send(query);
            #region Switch Statement
            return unitsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(unitsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = unitsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(unitsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region UnitsById
        [HttpGet("Units/{id}")]
        public async Task<IActionResult> GetUnitsById([FromRoute] string id)
        {
            var query = new GetUnitsByIdQuery(id);
            var unitsResult = await _mediator.Send(query);
            #region Switch Statement
            return unitsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(unitsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = unitsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(unitsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddUnits
        [HttpPost("AddUnits")]

        public async Task<IActionResult> AddUnits([FromBody] AddUnitsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addUnitsResult = await _mediator.Send(command);
            #region Switch Statement
            return addUnitsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddUnits), addUnitsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addUnitsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addUnitsResult.Errors),
                _ => BadRequest("Invalid Fields for Add Company")
            };

            #endregion
        }
        #endregion

        #region DeleteUnits
        [HttpDelete("DeleteUnits/{id}")]

        public async Task<IActionResult> DeleteUnits([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteUnitsCommand(id);
            var deleteUnitsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteUnitsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteUnitsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteUnitsResult.Errors),
                _ => BadRequest("Invalid Fields for Add Units")
            };

            #endregion
        }
        #endregion

        #region UpdateUnits
        [HttpPatch("UpdateUnits/{id}")]

        public async Task<IActionResult> UpdateUnits([FromRoute] string id, [FromBody] UpdateUnitsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateUnitsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateUnitsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateUnitsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateUnitsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateUnitsResult.Errors),
                _ => BadRequest("Invalid Fields for Update Units")
            };

            #endregion


        }
        #endregion

        #region FilterUnitsByDate
        [HttpGet("Filter-UnitsByDate")]
        public async Task<IActionResult> GetUnitsFilter([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterUnitsDTOs filterUnitsDTOs)
        {
            var query = new FilterUnitsByDateQuery(paginationRequest,filterUnitsDTOs);
            var filterUnitsResult = await _mediator.Send(query);

            #region Switch Statement
            return filterUnitsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterUnitsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterUnitsResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterUnitsResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion
        #endregion

        #region ConversionFactor
        #region AllConversionFactor
        [HttpGet("all-ConversionFactor")]
        public async Task<IActionResult> AllConversionFactor([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllConversionFactorQuery(paginationRequest);
            var conversionFactorResult = await _mediator.Send(query);
            #region Switch Statement
            return conversionFactorResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(conversionFactorResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = conversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(conversionFactorResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region ConversionFactorById
        [HttpGet("ConversionFactor/{id}")]
        public async Task<IActionResult> GetByCompanyId([FromRoute] string id)
        {
            var query = new GetConversionFactorByIdQuery(id);
            var conversionFactorResult = await _mediator.Send(query);
            #region Switch Statement
            return conversionFactorResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(conversionFactorResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = conversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(conversionFactorResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddConversionFactor
        [HttpPost("AddConversionFactor")]

        public async Task<IActionResult> AddConversionFactor([FromBody] AddConversionFactorRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addConversionFactorResult = await _mediator.Send(command);
            #region Switch Statement
            return addConversionFactorResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddConversionFactor), addConversionFactorResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addConversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addConversionFactorResult.Errors),
                _ => BadRequest("Invalid Fields for Add conversionfactor")
            };

            #endregion
        }
        #endregion

        #region DeleteConversionFactor
        [HttpDelete("DeleteConversionFactor/{id}")]

        public async Task<IActionResult> DeleteConversionFactor([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteConversionFactorCommand(id);
            var deleteConversionFactorResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteConversionFactorResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteConversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteConversionFactorResult.Errors),
                _ => BadRequest("Invalid Fields for Add conversion factor")
            };

            #endregion
        }
        #endregion

        #region UpdateConversionFactor
        [HttpPatch("UpdateConversionFactor/{id}")]

        public async Task<IActionResult> UpdateConversionFactor([FromRoute] string id, [FromBody] UpdateConversionFactorRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateConversionFactorResult = await _mediator.Send(command);
            #region Switch Statement
            return updateConversionFactorResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateConversionFactorResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateConversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateConversionFactorResult.Errors),
                _ => BadRequest("Invalid Fields for Update Units")
            };

            #endregion


        }
        #endregion

        #region FilterConversionFactor
        [HttpGet("FilterConversionFactor")]
        public async Task<IActionResult> GetConversionFactorFilter([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterConversionFactorDTOs filterConversionFactorDTOs )
        {
            var query = new FilterConversionFactorByDateQuery(paginationRequest, filterConversionFactorDTOs);
            var filterConversionFactorResult = await _mediator.Send(query);

            #region Switch Statement
            return filterConversionFactorResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterConversionFactorResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterConversionFactorResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterConversionFactorResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion
        #endregion

        #region ItemGroup
        #region AllItemGroup
        [HttpGet("all-itemGroup")]
        public async Task<IActionResult> AllItemGroup([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllItemGroupByQuery(paginationRequest);
            var itemGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return itemGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(itemGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region ItemGroupById
        [HttpGet("ItemGroup/{id}")]
        public async Task<IActionResult> GetItemGroupById([FromRoute] string id)
        {
            var query = new GetItemGroupByIdQuery(id);
            var itemGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return itemGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(itemGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddItemGroup
        [HttpPost("AddItemGroup")]

        public async Task<IActionResult> AddItemGroup([FromBody] AddItemGroupRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addItemGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return addItemGroupResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddItemGroup), addItemGroupResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addItemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addItemGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add ItemsGroup")
            };

            #endregion
        }
        #endregion

        #region DeleteItemGroup
        [HttpDelete("DeleteItemGroup/{id}")]

        public async Task<IActionResult> DeleteItemGroup([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteItemGroupCommand(id);
            var deleteItemGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteItemGroupResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteItemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteItemGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add Units")
            };

            #endregion
        }
        #endregion

        #region UpdateItemGroup
        [HttpPatch("UpdateItemGroup/{id}")]

        public async Task<IActionResult> UpdateItemGroup([FromRoute] string id, [FromBody] UpdateItemGroupRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateItemGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return updateItemGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateItemGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateItemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateItemGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Update Units")
            };

            #endregion


        }
        #endregion

        #region FilterItemGroupByDate
        [HttpGet("FilterItemGroupBy-Date")]
        public async Task<IActionResult> GetItemGroupFilter([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterItemGroupDTOs filterItemGroupDTOs)
        {
            var query = new FilterItemGroupByDateQuery(paginationRequest,filterItemGroupDTOs);
            var filterItemGroupResult = await _mediator.Send(query);

            #region Switch Statement
            return filterItemGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterItemGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterItemGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterItemGroupResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion
        #endregion

        #region Item

        #region AddItemExcel
        [HttpPost("upload-items")]

        public async Task<IActionResult> AddItemExcel([FromForm] ItemsExcelRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addItemExcelResult = await _mediator.Send(command);
            #region Switch Statement
            return addItemExcelResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddItemGroup), addItemExcelResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addItemExcelResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addItemExcelResult.Errors),
                _ => BadRequest("Invalid Fields for Add ItemExcel")
            };

            #endregion
        }
        #endregion


        #region AllItem
        [HttpGet("all-item")]
        public async Task<IActionResult> AllItem([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllItemByQuery(paginationRequest);
            var itemResult = await _mediator.Send(query);
            #region Switch Statement
            return itemResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(itemResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region ItemById
        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetItemById([FromRoute] string id)
        {
            var query = new GetItemByIdQuery(id);
            var itemResult = await _mediator.Send(query);
            #region Switch Statement
            return itemResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(itemResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddItem
        [HttpPost("AddItem")]

        public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addItemResult = await _mediator.Send(command);
            #region Switch Statement
            return addItemResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddItemGroup), addItemResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addItemResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addItemResult.Errors),
                _ => BadRequest("Invalid Fields for Add Company")
            };

            #endregion
        }
        #endregion

     

        #region DeleteItem
        [HttpDelete("DeleteItem/{id}")]

        public async Task<IActionResult> DeleteItem([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteItemCommand(id);
            var deleteItemResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteItemResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteItemResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteItemResult.Errors),
                _ => BadRequest("Invalid Fields for Add item")
            };

            #endregion
        }
        #endregion

        #region UpdateItem
        [HttpPatch("UpdateItem/{id}")]

        public async Task<IActionResult> UpdateItem([FromRoute] string id, [FromBody] UpdateItemRequest request)
        {
            //Mapping command and request
            var command = request.Tocommand(id);
            var updateItemResult = await _mediator.Send(command);
            #region Switch Statement
            return updateItemResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateItemResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateItemResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateItemResult.Errors),
                _ => BadRequest("Invalid Fields for Update item")
            };

            #endregion


        }
        #endregion

        #region FilterItemsByDate
        [HttpGet("FilterItemsByDate")]
        public async Task<IActionResult> GetItemsFilter([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterItemsDTOs filterItemsDTOs)
        {
            var query = new FilterItemsByDateQuery(paginationRequest,filterItemsDTOs);
            var filterItemsResult = await _mediator.Send(query);

            #region Switch Statement
            return filterItemsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterItemsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterItemsResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterItemsResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion


        #endregion

        #region Inventory

        #region GetInventoryItem
        [HttpGet("GetInventoryItem/{itemId}")]
        public async Task<IActionResult> GetInventoryItem([FromRoute] string itemId)
        {
            var query = new InventoryByItemQuery(itemId);
            var inventoryByItem = await _mediator.Send(query);
            #region Switch Statement
            return inventoryByItem switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventoryByItem.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventoryByItem.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventoryByItem.Errors),
                _ => BadRequest("Invalid itemId")
            };
            #endregion

        }
        #endregion

        #region AllInventory
        [HttpGet("all-Inventory")]
        public async Task<IActionResult> AllInventory([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllInventoryByQuery(paginationRequest);
            var inventoryResult = await _mediator.Send(query);
            #region Switch Statement
            return inventoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventoryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region GetRemainingQtyByItemId
        [HttpGet("GetRemainingQtyByItemId/{ItemId}")]
        public async Task<IActionResult> GetRemainingQtyByItemId([FromRoute] string ItemId)
        {
            var query = new GetRemainingQtyByItemIdQuery(ItemId);
            var itemresult = await _mediator.Send(query);
            #region Switch Statement
            return itemresult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(itemresult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = itemresult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid ItemId")
            };
            #endregion

        }

        #endregion

        #region InventoriesLogsById
        [HttpGet("InventoriesLogs/{id}")]
        public async Task<IActionResult> GetInventoriesLogsById([FromRoute] string id)
        {
            var query = new GetInventoriesLogsByIdQuery(id);
            var inventoriesLogsResult = await _mediator.Send(query);
            #region Switch Statement
            return inventoriesLogsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventoriesLogsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventoriesLogsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventoriesLogsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AllInventoryLogs
        [HttpGet("all-InventoryLogs")]
        public async Task<IActionResult> GetAllInventoryLogs([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllInventoriesLogsByQuery(paginationRequest);
            var inventoryLogsResult = await _mediator.Send(query);
            #region Switch Statement
            return inventoryLogsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventoryLogsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventoryLogsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventoryLogsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterInventoryByDate
        [HttpGet("FilterInventoryByDate")]
        public async Task<IActionResult> GetFilterInventoryByDate([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterInventoryDtos filterInventoryDtos)
        {
            var query = new FilterInventoryByDateQuery(paginationRequest,filterInventoryDtos);
            var filterInventoryResult = await _mediator.Send(query);

            #region Switch Statement
            return filterInventoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterInventoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterInventoryResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterInventoryResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

      

        #region StockCenter
        #region AllStockCenter
        [HttpGet("all-StockCenter")]
        public async Task<IActionResult> AllStockCenter([FromQuery] PaginationRequest paginationRequest,string? name)
        {
            var query = new GetAllStockCenterQuery(paginationRequest,name);
            var stockCenterResult = await _mediator.Send(query);
            #region Switch Statement
            return stockCenterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(stockCenterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockCenterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockCenterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region AddStockCenter
        [HttpPost("AddStockCenter")]

        public async Task<IActionResult> AddStockCenter([FromBody] AddStockCenterRequest request)
        {
           
            var command = request.ToCommand();
            var addStockResult = await _mediator.Send(command);
            #region Switch Statement
            return addStockResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStockCenter), addStockResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addStockResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addStockResult.Errors),
                _ => BadRequest("Invalid Fields for Add Stock Center")
            };

            #endregion
        }
        #endregion

        #region UpdateStockCenter
        [HttpPatch("UpdateStockCenter/{id}")]

        public async Task<IActionResult> UpdateStockCenter([FromRoute] string id, [FromBody] UpdateStockCenterRequest request)
        {
            //Mapping command and request
            var command = request.ToUpdateStockCenterCommand(id);
            var updateStockCenterResult = await _mediator.Send(command);
            #region Switch Statement
            return updateStockCenterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateStockCenterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateStockCenterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateStockCenterResult.Errors),
                _ => BadRequest("Invalid Fields for Update stockCenter")
            };

            #endregion


        }
        #endregion

        #region DeleteStockCenter
        [HttpDelete("DeleteStockCenter/{Id}")]

        public async Task<IActionResult> DeleteStockCenter([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var command = new DeleteStockCenterCommand(Id);
            var deleteStockCenterResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteStockCenterResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteStockCenterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteStockCenterResult.Errors),
                _ => BadRequest("Invalid Fields for Add StockCenter")
            };

            #endregion
        }
        #endregion

        #region FilterStockCenter
        [HttpGet("FilterStockCenter")]
        public async Task<IActionResult> GetFilterStockCenter([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterStockCenterDto filterStockCenterDto)
        {
            var query = new FilterStockCenterQuery(paginationRequest, filterStockCenterDto);
            var filterResult = await _mediator.Send(query);

            #region Switch Statement
            return filterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region StockCenterById
        [HttpGet("GetStockCenter/{id}")]
        public async Task<IActionResult> StockCEnterById([FromRoute] string id)
        {
            var query = new GetStockCenterByIdQuery(id);
            var StockCenterResult = await _mediator.Send(query);
            #region Switch Statement
            return StockCenterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(StockCenterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = StockCenterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(StockCenterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #endregion

        #region Stock Adjustment

        #region UpdateStockAdjustment
        [HttpPatch("UpdateStockAdjustment/{id}")]

        public async Task<IActionResult> UpdateStockAdjustment([FromRoute] string id, [FromBody] UpdateStockAdjustmentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateStockAdjustmentResult = await _mediator.Send(command);
            #region Switch Statement
            return updateStockAdjustmentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateStockAdjustmentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateStockAdjustmentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateStockAdjustmentResult.Errors),
                _ => BadRequest("Invalid Fields for Update stock adjustment")
            };

            #endregion


        }
        #endregion

        #region AddStockAdjustment
        [HttpPost("AddStockAdjustment")]

        public async Task<IActionResult> AddStockAdjustment([FromBody] AddStockAdjustmentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var stockAdjustmentResult = await _mediator.Send(command);
            #region Switch Statement
            return stockAdjustmentResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStockAdjustment), stockAdjustmentResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockAdjustmentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockAdjustmentResult.Errors),
                _ => BadRequest("Invalid Fields for add stock Adjustment")
            };

            #endregion
        }
        #endregion

        #region AllStockAdjustment
        [HttpGet("all-StockAdjustment")]
        public async Task<IActionResult> AllStockAdjustment([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllStockAdjustmentQuery(paginationRequest);
            var stockAdjustmentResult = await _mediator.Send(query);
            #region Switch Statement
            return stockAdjustmentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(stockAdjustmentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockAdjustmentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockAdjustmentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteStockAdjustment
        [HttpDelete("DeleteStockAdjustment/{id}")]

        public async Task<IActionResult> DeleteStockAdjustment([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteStockAdjustmentCommand(id);
            var deleteStockAdjustmentResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteStockAdjustmentResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteStockAdjustmentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteStockAdjustmentResult.Errors),
                _ => BadRequest("Invalid Fields for Add StockAdjustment")
            };

            #endregion
        }
        #endregion

        #region FilterStockAdjustment
        [HttpGet("FilterStockAdjustment")]
        public async Task<IActionResult> GetFilterStockAdjustment([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterStockAdjustmentDto filterStockAdjustmentDto)
        {
            var query = new FilterStockAdjustmentQuery(paginationRequest, filterStockAdjustmentDto);
            var filterResult = await _mediator.Send(query);

            #region Switch Statement
            return filterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion


        #endregion


        #endregion

    }
}
