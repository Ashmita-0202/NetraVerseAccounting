using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Application.Purchase.Queries.CurrentPurchaseBillNumber;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.AddSalesDetails.RequestCommandMapper;
using TN.Sales.Application.Sales.Command.DeleteSalesDetails;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.Sales.Command.QuotationToSales.RequestCommandMapper;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany.RequestCommandMapper;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails.RequestCommandMapper;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.BillNumberGenerationByCompany;
using TN.Sales.Application.Sales.Queries.CurrentSalesBillNumber;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.Sales.Queries.GetAllSalesItems;
using TN.Sales.Application.Sales.Queries.GetSalesDetailsByRefNo;
using TN.Sales.Application.Sales.Queries.GetSalesQuotationById;
using TN.Sales.Application.Sales.Queries.SalesDetailsById;
using TN.Sales.Application.Sales.Queries.SalesItemByItemId;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails.RequestCommandMapper;
using TN.Sales.Application.SalesReturn.Command.DeleteSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails.RequestCommandMapper;
using TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems;
using TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Sales.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesControllers : BaseController
    {
        private readonly IMediator _mediator;

        public SalesControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }

        #region SalesQuotation

        #region QuotationToSales
        [HttpPost("QuotationToSales")]
        public async Task<IActionResult> QuotationToSales([FromBody] QuotationToSalesRequest request)
        {
            var command = request.ToCommand();
            var addQuotationToSales = await _mediator.Send(command);

            #region Switch Statement
            return addQuotationToSales switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(QuotationToSales), addQuotationToSales.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addQuotationToSales.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addQuotationToSales.Errors),
                _ => BadRequest("Invalid Fields for Add Quotation To Sales ")

            };

            #endregion
        }
        #endregion

        #region SalesQuotationById
        [HttpGet("SalesQuotationBy/{id}")]
        public async Task<IActionResult> GetSalesQuotationById([FromRoute] string id)
        {
            var query = new GetSalesQuotationByIdQuery(id);
            var quotationResult = await _mediator.Send(query);
            #region Switch Statement
            return quotationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(quotationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { quotationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(quotationResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion


        #region FilterSalesQuotationByDate
        [HttpGet("FilterSalesQuotationByDate")]
        public async Task<IActionResult> GetFilterSalesQuotation([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSalesDetailsDTOs filterSalesDetailsDTOs)
        {
            var query = new FilterSalesQuotationQuery(paginationRequest, filterSalesDetailsDTOs);
            var salesQuotationResult = await _mediator.Send(query);

            #region Switch Statement
            return salesQuotationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesQuotationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesQuotationResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { salesQuotationResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #endregion

        #region SalesDetails

        #region SalesDetailsItemsByItemsId
        [HttpGet("SalesDetailsItems/{itemsId}")]
        public async Task<IActionResult> GetSalesDetailsItems([FromRoute] string itemsId)
        {
            var query = new GetSalesItemDetailsByItemIdQuery(itemsId);
            var salesDetailsItemsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesDetailsItemsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesDetailsItemsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesDetailsItemsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesDetailsItemsResult.Errors),
                _ => BadRequest($"Invalid Id{itemsId}")
            };
            #endregion

        }
        #endregion


        #region AddSalesDetails
        [HttpPost("AddSalesDetails")]

        public async Task<IActionResult> AddSalesDetails([FromBody] AddSalesDetailsRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addSalesDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return addSalesDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSalesDetails), addSalesDetailsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addSalesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSalesDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Add salesDetails ")

            };

            #endregion
        }
        #endregion

        #region UpdateSalesDetails
        [HttpPatch("UpdateSalesDetails/{id}")]

        public async Task<IActionResult> UpdateSalesDetails([FromRoute] string id, [FromBody] UpdateSalesDetailsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateSalesDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSalesDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSalesDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { updateSalesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSalesDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Update Sales Details result")
            };

            #endregion


        }
        #endregion

        #region AllSalesDetails
        [HttpGet("all-SalesDetails")]
        public async Task<IActionResult> AllSalesDetails([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSalesDetailsByQuery(paginationRequest);
            var salesDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region SalesDetailsById
        [HttpGet("SalesDetails/{id}")]
        public async Task<IActionResult> GetSalesDetailsById([FromRoute] string id)
        {
            var query = new GetSalesDetailsByIdQuery(id);
            var salesDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AllSalesItems
        [HttpGet("all-SalesItems")]
        public async Task<IActionResult> AllSalesItems([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSalesItemsByQuery(paginationRequest);
            var salesItemsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesItemsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesItemsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesItemsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesItemsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        //Unused Api
        #region SalesDetailsByReferenceNumber
        [HttpGet("Sales-Details/{referenceNumber}")]
        public async Task<IActionResult> GetSalesDetailsByReferenceNumber([FromRoute] string referenceNumber)
        {
            var query = new GetSalesDetailsQuery(referenceNumber);
            var salesResult = await _mediator.Send(query);
            #region Switch Statement
            return salesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region BillNumberGenerationTypeByCompany
        [HttpGet("BillNumberGenerationSales/{id}")]
        public async Task<IActionResult> BillNumberGenerationSales([FromRoute] string id)
        {
            var query = new BIllNumberGenerationByCompanyQuery(id);
            var billNumberGenerationStatus = await _mediator.Send(query);
            #region Switch Statement
            return billNumberGenerationStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(billNumberGenerationStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { billNumberGenerationStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(billNumberGenerationStatus.Errors),
                _ => BadRequest("Invalid request")
            };
            #endregion

        }
        #endregion

        #region DeleteSalesDetails
        [HttpDelete("DeleteSalesDetails/{id}")]

        public async Task<IActionResult> DeleteSalesDetails([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSalesDetailsCommand(id);
            var deleteSalesDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteSalesDetailsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { deleteSalesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSalesDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Add SalesDetails")
            };

            #endregion
        }
        #endregion

        #region UpdateSalesBillStatusByCompany
        [HttpPatch("UpdateSalesBillStatusByCompany/{id}")]

        public async Task<IActionResult> UpdateSalesBillStatusByCompany([FromRoute] string id, [FromBody] UpdateBillNumberGenerationByCompanyRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateBillNumberStatus = await _mediator.Send(command);
            #region Switch Statement
            return updateBillNumberStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateBillNumberStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { updateBillNumberStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBillNumberStatus.Errors),
                _ => BadRequest("Invalid Fields for UpdateBillNumberGeneratorStatus")
            };

            #endregion


        }
        #endregion

        #region FilterSalesDetailsByDate
        [HttpGet("FilterSalesDetailsByDate")]
        public async Task<IActionResult> GetFilterSalesDetails([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSalesDetailsDTOs filterSalesDetailsDTOs)
        {
            var query = new FilterSalesDetailsByDateQuery(paginationRequest, filterSalesDetailsDTOs);
            var salesDetailsResult = await _mediator.Send(query);

            #region Switch Statement
            return salesDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { salesDetailsResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region SalesReturnDetails

        #region AddSalesReturnDetails
        [HttpPost("AddSalesReturnDetails")]

        public async Task<IActionResult> AddSalesReturnDetails([FromBody] AddSalesReturnDetailsRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addSalesReturnDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return addSalesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSalesReturnDetails), addSalesReturnDetailsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addSalesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSalesReturnDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Add sales Return Details ")

            };

            #endregion
        }
        #endregion

        #region AllSalesDetailsReturn
        [HttpGet("all-SalesDetailsReturn")]
        public async Task<IActionResult> AllSalesDetailsReturn([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSalesReturnDetailsByQuery(paginationRequest);
            var salesReturnDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesReturnDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region SalesReturnDetailsById
        [HttpGet("SalesReturnDetails/{id}")]
        public async Task<IActionResult> GetSalesReturnDetailsById([FromRoute] string id)
        {
            var query = new GetSalesReturnDetailsByIdQuery(id);
            var salesReturnDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesReturnDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateSalesReturnDetails
        [HttpPatch("UpdateSalesReturnDetails/{id}")]

        public async Task<IActionResult> UpdateSalesReturnDetails([FromRoute] string id, [FromBody] UpdateSalesReturnDetailsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateSalesReturnDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSalesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSalesReturnDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { updateSalesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSalesReturnDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Update Sales Return Details result")
            };

            #endregion


        }
        #endregion

        #region DeleteSalesReturnDetails
        [HttpDelete("DeleteSalesReturnDetails/{id}")]

        public async Task<IActionResult> DeleteSalesReturnDetails([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSalesReturnDetailsCommand(id);
            var deleteSalesReturnDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteSalesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { deleteSalesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSalesReturnDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Add SalesReturn Details")
            };

            #endregion
        }
        #endregion

        #region FilterSalesReturnDetailsByDate
        [HttpGet("FilterSalesReturnDetailsByDate")]
        public async Task<IActionResult> GetFilterSalesReturnDetailsByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSalesReturnDetailsDTOs filterSalesReturnDetailsDTOs)
        {
            var query = new GetSalesReturnDetailsFilterQuery(paginationRequest, filterSalesReturnDetailsDTOs);
            var salesReturnDetailsResult = await _mediator.Send(query);

            #region Switch Statement
            return salesReturnDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesReturnDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { salesReturnDetailsResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion



        #endregion

        #region SalesReturnItems

        #region AllSalesReturnItems
        [HttpGet("all-SalesReturnItems")]
        public async Task<IActionResult> AllSalesReturnItems([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSalesReturnItemsByQuery(paginationRequest);
            var salesReturnItemsResult = await _mediator.Send(query);
            #region Switch Statement
            return salesReturnItemsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnItemsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { salesReturnItemsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesReturnItemsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion

        #endregion

        #region CurrentBillNumber
        [HttpGet("CurrentBillNumber")]
        public async Task<IActionResult> CurrentBillNumber()
        {
            var query = new CurrentSalesBillNumbersQuery();
            var currentBillNumber = await _mediator.Send(query);
            #region Switch Statement
            return currentBillNumber switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(currentBillNumber.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { currentBillNumber.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(currentBillNumber.Errors),
                _ => BadRequest("Invalid request")
            };
            #endregion

        }
        #endregion
    }
}
