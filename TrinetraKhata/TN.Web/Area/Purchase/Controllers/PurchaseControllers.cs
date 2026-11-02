using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails.RequestCommandMapper;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems.RequestCommandMapper;
using TN.Purchase.Application.Purchase.Command.DeletePurchaseDetails;
using TN.Purchase.Application.Purchase.Command.QuotationToPurchase;
using TN.Purchase.Application.Purchase.Command.QuotationToPurchase.RequestCommandMapper;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany.RequestCommandMapper;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails.RequestCommandMapper;
using TN.Purchase.Application.Purchase.Queries.BillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Queries.CurrentPurchaseBillNumber;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate;
using TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems;
using TN.Purchase.Application.Purchase.Queries.GetCurrentPurchaseReferenceNumber;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails.RequestCommandMapper;
using TN.Purchase.Application.PurchaseReturn.Command.DeletePurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails.RequestCommandMapper;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate;
using TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.Sales.Queries.GetSalesQuotationById;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Purchase.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseControllers : BaseController
    {
        private readonly IMediator _mediator;

        public PurchaseControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }



        #region PurchaseQuotation

        #region QuotationToPurchase
        [HttpPost("QuotationToPurchase")]
        public async Task<IActionResult> QuotationToPurchase([FromBody] QuotationToPurchaseRequest request)
        {
            var command = request.ToCommand();
            var addQuotationToPurchase = await _mediator.Send(command);

            #region Switch Statement
            return addQuotationToPurchase switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(QuotationToPurchase), addQuotationToPurchase.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addQuotationToPurchase.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addQuotationToPurchase.Errors),
                _ => BadRequest("Invalid Fields for Add Quotation To Purchase ")

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
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesQuotationResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = salesQuotationResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #endregion

        #region PurchaseQuotation
        #region FilterPurchaseQuotationByDate
        [HttpGet("FilterPurchaseQuotationByDate")]
        public async Task<IActionResult> GetFilterPurchaseQuotation([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs)
        {
            var query = new FilterPurchaseQuotationQuery(paginationRequest, filterPurchaseDetailsDTOs);
            var purchaseQuotationResult = await _mediator.Send(query);

            #region Switch Statement
            return purchaseQuotationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseQuotationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseQuotationResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseQuotationResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region PurchaseQuotationById
        [HttpGet("PurchaseQuotationBy/{id}")]
        public async Task<IActionResult> GetPurchaseQuotationById([FromRoute] string id)
        {
            var query = new GetPurchaseQuotationByIdQuery(id);
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

            #region PurchaseItems
            #region AddPurchaseItems
            [HttpPost("AddPurchaseItems")]

            public async Task<IActionResult> AddPurchaseItems([FromBody] AddPurchaseItemsRequest request)
            {
                //Mapping command and request

                var command = request.ToCommand();
                var addPurchaseResult = await _mediator.Send(command);
                #region Switch Statement
                return addPurchaseResult switch
                {
                    { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddPurchaseItems), addPurchaseResult.Data),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPurchaseResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(addPurchaseResult.Errors),
                    _ => BadRequest("Invalid Fields for Add PurchaseItems ")

                };

                #endregion
            }
            #endregion

            #region AllPurchaseItems
            [HttpGet("all-PurchaseItems")]
            public async Task<IActionResult> AllPurchaseItems([FromQuery] PaginationRequest paginationRequest)
            {
                var query = new GetAllPurchaseItemsByQuery(paginationRequest);
                var purchaseItemsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseItemsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseItemsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseItemsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseItemsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }

            #endregion

            #endregion

            #region PurchaseDetails
            #region AddPurchaseDetails
            [HttpPost("AddPurchaseDetails")]

            public async Task<IActionResult> AddPurchaseDetails([FromBody] AddPurchaseDetailsRequest request)
            {
                //Mapping command and request

                var command = request.ToCommand();
                var addPurchaseDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return addPurchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddPurchaseDetails), addPurchaseDetailsResult.Data),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPurchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(addPurchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Add purchaseDetails ")

                };

                #endregion
            }
            #endregion


            #region AllPurchaseDetails
            [HttpGet("all-PurchaseDetails")]
            public async Task<IActionResult> AllPurchaseDetails([FromQuery] PaginationRequest paginationRequest)
            {
                var query = new GetAllPurchaseDetailsByQueries(paginationRequest);
                var purchaseDetailsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }

            #endregion

            #region PurchaseDetailsById
            [HttpGet("PurchaseDetails/{id}")]
            public async Task<IActionResult> GetPurchaseDetailsById([FromRoute] string id)
            {
                var query = new GetPurchaseDetailsByIdQuery(id);
                var purchaseDetailsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }
            #endregion

            #region DeletePurchaseDetails
            [HttpDelete("DeletePurchaseDetails/{id}")]

            public async Task<IActionResult> DeletePurchaseDetails([FromRoute] string id, CancellationToken cancellationToken)
            {
                var command = new DeletePurchaseDetailsCommand(id);
                var deletePurchaseDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return deletePurchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: true } => NoContent(),
                    { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deletePurchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(deletePurchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Add purchaseDetails")
                };

                #endregion
            }
            #endregion

            #region UpdatePurchaseDetails
            [HttpPatch("UpdatePurchaseDetails/{id}")]

            public async Task<IActionResult> UpdatePurchaseDetails([FromRoute] string id, [FromBody] UpdatePurchaseDetailsRequest request)
            {
                //Mapping command and request
                var command = request.ToCommand(id);
                var updatePurchaseDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return updatePurchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(updatePurchaseDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePurchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePurchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Update purchase Details result")
                };

                #endregion


            }
            #endregion

            #region UpdatePurchaseBillStatusByCompany
            [HttpPatch("UpdatePurchaseBillStatusByCompany/{id}")]

            public async Task<IActionResult> UpdatePurchaseBillStatusByCompany([FromRoute] string id, [FromBody] UpdateBillNumberGeneratorByCompanyRequest request)
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
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateBillNumberStatus.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBillNumberStatus.Errors),
                    _ => BadRequest("Invalid Fields for UpdateBillNumberGeneratorStatus")
                };

                #endregion


            }
            #endregion

            #region FilterPurchaseDetailsByDate
            [HttpGet("FilterPurchaseDetailsByDate")]
            public async Task<IActionResult> GetPurchaseDetailsFilter([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs)
            {
                var query = new FilterPurchaseDetailsByDateQuery(paginationRequest, filterPurchaseDetailsDTOs);
                var purchaseDetailsResult = await _mediator.Send(query);

                #region Switch Statement
                return purchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseDetailsResult.Errors }),
                    _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
                };
                #endregion
            }
            #endregion

            #region PurchaseDetailsByReferenceNo
            [HttpGet("Purchase-Details/{referenceNumber}")]
            public async Task<IActionResult> GetPurchaseDetailsByRefNo([FromRoute] string referenceNumber)
            {
                var query = new GetPurchaseDetailsQuery(referenceNumber);
                var purchaseDetailsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseDetailsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }
            #endregion

            #endregion

            #region PurchaseReturnDetails
            #region AllPurchaseReturnDetails
            [HttpGet("all-purchasereturndetails")]
            public async Task<IActionResult> AllPurchaseReturnDetails([FromQuery] PaginationRequest paginationRequest)
            {
                var query = new PurchaseReturnDetailsByQueries(paginationRequest);
                var purchaseReturnDetailsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseReturnDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseReturnDetailsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }

            #endregion

            #region PurchaseReturnDetailsById
            [HttpGet("PurchaseReturnDetails/{id}")]
            public async Task<IActionResult> GetPurchaseReturnDetailsById([FromRoute] string id)
            {
                var query = new PurchaseReturnDetailsByIdQueries(id);
                var purchaseReturnDetailsResult = await _mediator.Send(query);
                #region Switch Statement
                return purchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseReturnDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseReturnDetailsResult.Errors),
                    _ => BadRequest("Invalid page and pageSize Fields")
                };
                #endregion

            }
            #endregion

            #region AddPurchaseReturnDetails
            [HttpPost("AddPurchaseReturnDetails")]

            public async Task<IActionResult> AddPurchaseReturnDetails([FromBody] AddPurchaseReturnDetailsRequest request)
            {
                //Mapping command and request

                var command = request.ToCommand();
                var addPurchaseReturnDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return addPurchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddPurchaseDetails), addPurchaseReturnDetailsResult.Data),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPurchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(addPurchaseReturnDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Add purchaseReturnDetails ")

                };

                #endregion
            }
            #endregion

            #region DeletePurchaseReturnDetails
            [HttpDelete("DeletePurchaseReturnDetails/{id}")]

            public async Task<IActionResult> DeletePurchaseReturnDetails([FromRoute] string id, CancellationToken cancellationToken)
            {
                var command = new DeletePurchaseReturnDetailsCommand(id);
                var deletePurchaseReturnDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return deletePurchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: true } => NoContent(),
                    { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deletePurchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(deletePurchaseReturnDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Delete PurchaseReturn Details")
                };

                #endregion
            }
            #endregion

            #region UpdatePurchaseReturnDetails
            [HttpPatch("UpdatePurchaseReturnDetails/{id}")]

            public async Task<IActionResult> UpdatePurchaseReturnDetails([FromRoute] string id, [FromBody] UpdatePurchaseReturnDetailsRequest request)
            {
                //Mapping command and request
                var command = request.ToCommand(id);
                var updatePurchaseReturnDetailsResult = await _mediator.Send(command);
                #region Switch Statement
                return updatePurchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(updatePurchaseReturnDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePurchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePurchaseReturnDetailsResult.Errors),
                    _ => BadRequest("Invalid Fields for Update purchase Return Details result")
                };

                #endregion


            }
            #endregion

            #region FilterPurchaseReturnDetailsByDate
            [HttpGet("FilterPurchaseReturnDetailsByDate")]
            public async Task<IActionResult> GetFilterPurchaseReturnDetails([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPurchaseReturnDetailsDtos filterPurchaseReturnDetailsDtos)
            {
                var query = new GetFilterPurchaseReturnDetailsQuery(paginationRequest, filterPurchaseReturnDetailsDtos);
                var purchaseReturnDetailsResult = await _mediator.Send(query);

                #region Switch Statement
                return purchaseReturnDetailsResult switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(purchaseReturnDetailsResult.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReturnDetailsResult.Message }),
                    { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseReturnDetailsResult.Errors }),
                    _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
                };
                #endregion
            }
            #endregion


            #region CurrentPurchaseReferenceNumber
            [HttpGet("CurrentPurchaseReferenceNumber")]
            public async Task<IActionResult> CurrentPurchaseReferenceNumber()
            {
                var query = new CurrentPurchaseRefNoQuery();
                var currentRefNo = await _mediator.Send(query);
                #region Switch Statement
                return currentRefNo switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(currentRefNo.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = currentRefNo.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(currentRefNo.Errors),
                    _ => BadRequest("Invalid request")
                };
                #endregion

            }
            #endregion



            #endregion

            #region BillNumberGenerationTypeByCompany
            [HttpGet("BillNumberGenerationForPurchase/{id}")]
            public async Task<IActionResult> BillNumberGenerationForPurchase([FromRoute] string id)
            {
                var query = new BillNumberGenerationByCompanyQueries(id);
                var billNumberGenerationStatus = await _mediator.Send(query);
                #region Switch Statement
                return billNumberGenerationStatus switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(billNumberGenerationStatus.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = billNumberGenerationStatus.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(billNumberGenerationStatus.Errors),
                    _ => BadRequest("Invalid request")
                };
                #endregion

            }
            #endregion


            #region CurrentBillNumber
            [HttpGet("CurrentBillNumber")]
            public async Task<IActionResult> CurrentBillNumber()
            {
                var query = new CurrentPurchaseBillNumberQuery();
                var currentBillNumber = await _mediator.Send(query);
                #region Switch Statement
                return currentBillNumber switch
                {
                    { IsSuccess: true, Data: not null } => new JsonResult(currentBillNumber.Data, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    }),
                    { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = currentBillNumber.Message }),
                    { IsSuccess: false, Errors: not null } => HandleFailureResult(currentBillNumber.Errors),
                    _ => BadRequest("Invalid request")
                };
                #endregion

            }
            #endregion



        
    }
    #endregion
}
