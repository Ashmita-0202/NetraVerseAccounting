using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NV.Payment.Application.Payment.Command.AddPayment;
using NV.Payment.Application.Payment.Command.AddPayment.RequestCommandMapper;
using NV.Payment.Application.Payment.Command.DeletePayment;
using NV.Payment.Application.Payment.Command.UpdatePayment;
using NV.Payment.Application.Payment.Command.UpdatePayment.RequestCommandMapper;
using NV.Payment.Application.Payment.Queries.FilterPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPayment;
using NV.Payment.Application.Payment.Queries.GetPaymentMethodById;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Payment.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentControllers : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
        }
        #region PaymentMethod

        #region AddPaymentMethod
        [HttpPost("AddPaymentMethod")]

        public async Task<IActionResult> AddPaymentMethod([FromBody] AddPaymentMethodRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addPaymentMethodResult = await _mediator.Send(command);
            #region Switch Statement
            return addPaymentMethodResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddPaymentMethod), addPaymentMethodResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addPaymentMethodResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addPaymentMethodResult.Errors),
                _ => BadRequest("Invalid Fields for Add PaymentMethod")
            };

            #endregion
        }
        #endregion

        #region AllPaymentMethod
        [HttpGet("all-PaymentMethod")]
        public async Task<IActionResult> AllPaymentMethod([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllPaymentMethodQuery(paginationRequest);
            var paymentResult = await _mediator.Send(query);
            #region Switch Statement
            return paymentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { paymentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeletePaymentMethod
        [HttpDelete("DeletePaymentMethod/{id}")]

        public async Task<IActionResult> DeletePaymentMethod([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeletePaymentCommand(id);
            var deletePaymentResult = await _mediator.Send(command);
            #region Switch Statement
            return deletePaymentResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { deletePaymentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deletePaymentResult.Errors),
                _ => BadRequest("Invalid Fields for Add Payment Method")
            };

            #endregion
        }
        #endregion

        #region UpdatePaymentMethod
        [HttpPatch("UpdatePaymentMethod/{id}")]

        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] string id, [FromBody] UpdatePaymentMethodRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updatePaymentResult = await _mediator.Send(command);
            #region Switch Statement
            return updatePaymentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePaymentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { updatePaymentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePaymentResult.Errors),
                _ => BadRequest("Invalid Fields for Update PaymentMethod")
            };

            #endregion


        }
        #endregion

        #region PaymentMethodById
        [HttpGet("PaymentMethod/{id}")]
        public async Task<IActionResult> GetPaymentMethodById([FromRoute] string id)
        {
            var query = new GetPaymentMethodByIdQuery(id);
            var paymentMethodResult = await _mediator.Send(query);
            #region Switch Statement
            return paymentMethodResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentMethodResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { paymentMethodResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentMethodResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterPaymentMethodByDate
        [HttpGet("Filter-PaymentMethod")]
        public async Task<IActionResult> GetPaymentMethodFilter([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPaymentMethodDto filterPaymentMethodDto)
        {
            var query = new GetFilterPaymentMethodQuery(paginationRequest, filterPaymentMethodDto);
            var filterPaymentMethodResult = await _mediator.Send(query);

            #region Switch Statement
            return filterPaymentMethodResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterPaymentMethodResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { filterPaymentMethodResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { filterPaymentMethodResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #endregion

    }
}
