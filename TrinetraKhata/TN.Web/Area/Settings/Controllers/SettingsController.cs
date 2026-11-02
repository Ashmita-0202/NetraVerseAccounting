using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusByCompany;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Command.CloseFiscalYear.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear.NewFolder;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales.RequestCommandMapper;
using TN.Shared.Application.Shared.Queries.FiscalYearStartDate;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYearByCompany;
using TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetInventoryMethodByCompany;
using TN.Shared.Application.Shared.Queries.GetJournalRefByCompany;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInSales;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Settings.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }
        #region SelectedFiscalYear
        [HttpGet("FiscalYearStartedDate")]
        public async Task<IActionResult> SelectedFiscalYear()
        {
            var query = new FiscalYearStartDateQuery();
            var fiscalYearStartedDateResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearStartedDateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearStartedDateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearStartedDateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearStartedDateResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region SelectedFiscalYear
        [HttpGet("SelectedFiscalYear")]
        public async Task<IActionResult> SelectedFiscalYear([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetSelectedFiscalYearQuery(paginationRequest);
            var selectedfiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return selectedfiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(selectedfiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = selectedfiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(selectedfiscalYearResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region GetTaxStatusInPurchase
        [HttpGet("GetTaxStatusInPurchase/{id}")]
        public async Task<IActionResult> GetTaxStatusInPurchase([FromRoute] string id)
        {
            var query = new GetTaxStatusInPurchaseQuery(id);
            var taxStatusInPurchase = await _mediator.Send(query);
            #region Switch Statement
            return taxStatusInPurchase switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(taxStatusInPurchase.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = taxStatusInPurchase.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(taxStatusInPurchase.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetTaxStatusInSales
        [HttpGet("GetTaxStatusInSales/{id}")]
        public async Task<IActionResult> GetTaxStatusInSales([FromRoute] string id)
        {
            var query = new GetTaxStatusInSalesQuery(id);
            var taxStatusInSales = await _mediator.Send(query);
            #region Switch Statement
            return taxStatusInSales switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(taxStatusInSales.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = taxStatusInSales.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(taxStatusInSales.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetPurchaseReferenceNumberByCompanyId
        [HttpGet("GetPurchaseReferenceNumberByCompany/{id}")]
        public async Task<IActionResult> GetPurchaseReferenceNumberByCompany([FromRoute] string id)
        {
            var query = new GetPurchaseReferenceNumberQuery(id);
            var purchaseReferenceNumbers = await _mediator.Send(query);
            #region Switch Statement
            return purchaseReferenceNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseReferenceNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReferenceNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseReferenceNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesReferenceNumberByCompanyId
        [HttpGet("GetSalesReferenceNumberByCompany/{id}")]
        public async Task<IActionResult> GetSalesReferenceNumberByCompany([FromRoute] string id)
        {
            var query = new GetSalesReferenceNumberQuery(id);
            var salesReferenceNumbers = await _mediator.Send(query);
            #region Switch Statement
            return salesReferenceNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReferenceNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesReferenceNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesReferenceNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetItemStatusByCompanyId
        [HttpGet("GetItemStatusByCompany/{id}")]
        public async Task<IActionResult> GetItemStatusByCompany([FromRoute] string id)
        {
            var query = new GetItemStatusByCompanyQuery(id);
            var expiredTimeItemStatus = await _mediator.Send(query);
            #region Switch Statement
            return expiredTimeItemStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expiredTimeItemStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expiredTimeItemStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expiredTimeItemStatus.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetJournalRefByCompanyId
        [HttpGet("GetJournalRefByCompany/{id}")]
        public async Task<IActionResult> GetJournalRefByCompany([FromRoute] string id)
        {
            var query = new GetJournalRefByCompanyQuery(id);
            var journalRef = await _mediator.Send(query);
            #region Switch Statement
            return journalRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(journalRef.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateItemStatusByCompany
        [HttpPatch("UpdateItemStatusByCompany/{id}")]

        public async Task<IActionResult> UpdateStatusByCompany([FromRoute] string id, [FromBody] UpdateItemStatusByCompanyRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateExpiredDateItemStatus = await _mediator.Send(command);
            #region Switch Statement
            return updateExpiredDateItemStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExpiredDateItemStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExpiredDateItemStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExpiredDateItemStatus.Errors),
                _ => BadRequest("Invalid Fields for Update ExpiredDateItem status")
            };

            #endregion


        }
        #endregion

        #region UpdateJournalRefByCompany
        [HttpPatch("UpdateJournalRefByCompany/{companyId}")]

        public async Task<IActionResult> UpdateJournalRefByCompany([FromRoute] string companyId, [FromBody] UpdateJournalRefByCompanyRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateJournalRef = await _mediator.Send(command);
            #region Switch Statement
            return updateJournalRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateJournalRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateJournalRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateJournalRef.Errors),
                _ => BadRequest("Invalid Fields for Update journal reference status")
            };

            #endregion


        }
        #endregion

        #region GetInventoryMethodByCompanyId
        [HttpGet("GetInventoryMethodByCompany/{companyId}")]
        public async Task<IActionResult> GetInventoryMethodByCompany([FromRoute] string companyId)
        {
            var query = new GetInventoryByCompanyIdQuery(companyId);
            var inventory = await _mediator.Send(query);
            #region Switch Statement
            return inventory switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventory.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventory.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventory.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateInventoryMethodByCompany
        [HttpPatch("UpdateInventoryMethod/{companyId}")]

        public async Task<IActionResult> UpdateInventoryMethod([FromRoute] string companyId, [FromBody] UpdateInventoryMethodRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateInventory = await _mediator.Send(command);
            #region Switch Statement
            return updateInventory switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateInventory.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateInventory.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateInventory.Errors),
                _ => BadRequest("Invalid Fields for Update Inventory Method")
            };

            #endregion


        }
        #endregion

        #region GetSerialNumberForPurchase
        [HttpGet("GetSerialNumberForPurchase/{companyId}")]
        public async Task<IActionResult> GetSerialNumberForPurchase([FromRoute] string companyId)
        {
            var query = new GetSerialNumberForPurchaseQuery(companyId);
            var serialNumber = await _mediator.Send(query);
            #region Switch Statement
            return serialNumber switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(serialNumber.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = serialNumber.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(serialNumber.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateSalesRefByCompany
        [HttpPatch("UpdateSalesRefByCompany/{companyId}")]

        public async Task<IActionResult> UpdateSalesRefByCompany([FromRoute] string companyId, [FromBody] UpdateSalesReferenceNumberRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateSalesRef = await _mediator.Send(command);
            #region Switch Statement
            return updateSalesRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSalesRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSalesRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSalesRef.Errors),
                _ => BadRequest("Invalid Fields for Update sales reference status")
            };

            #endregion


        }
        #endregion

        #region UpdatePurchaseRefByCompany
        [HttpPatch("UpdatePurchaseRefByCompany/{companyId}")]

        public async Task<IActionResult> UpdatePurchaseRefByCompany([FromRoute] string companyId, [FromBody] UpdatePurchaseReferenceNumberRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updatePurchaseRef = await _mediator.Send(command);
            #region Switch Statement
            return updatePurchaseRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePurchaseRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePurchaseRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePurchaseRef.Errors),
                _ => BadRequest("Invalid Fields for Update purchase reference status")
            };

            #endregion


        }
        #endregion

        #region UpdateTaxStatusInPurchaseByCompany
        [HttpPatch("UpdateTaxStatusInPurchase/{companyId}")]

        public async Task<IActionResult> UpdateTaxStatusInPurchase([FromRoute] string companyId, [FromBody] UpdateTaxStatusInPurchaseRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateTaxStatus = await _mediator.Send(command);
            #region Switch Statement
            return updateTaxStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTaxStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateTaxStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTaxStatus.Errors),
                _ => BadRequest("Invalid Fields for Update taxStatus in Purchase")
            };

            #endregion


        }
        #endregion

        #region UpdateTaxStatusInSalesByCompany
        [HttpPatch("UpdateTaxStatusInSales/{companyId}")]

        public async Task<IActionResult> UpdateTaxStatusInSales([FromRoute] string companyId, [FromBody] UpdateTaxStatusInSalesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateTaxStatusInSales = await _mediator.Send(command);
            #region Switch Statement
            return updateTaxStatusInSales switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTaxStatusInSales.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateTaxStatusInSales.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTaxStatusInSales.Errors),
                _ => BadRequest("Invalid Fields for Update taxStatus in Sales")
            };

            #endregion


        }
        #endregion

        #region GetFiscalYearByCompapnyId
        [HttpGet("GetCurrentFiscalYearBy/{companyId}")]
        public async Task<IActionResult> GetCurrentFiscalYearBy([FromRoute] string companyId)
        {
            var query = new GetCurrentFiscalYearByQuery(companyId);
            var fiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearResult.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region AllFiscalYear
        [HttpGet("all-FiscalYear")]
        public async Task<IActionResult> AllFiscalYear([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllFiscalYearQuery(paginationRequest);
            var fiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region UpdateFiscalYearByCompanyId
        [HttpPatch("UpdateCurrentFiscalYearBy/{companyId}")]

        public async Task<IActionResult> UpdateCurrentFiscalYear([FromRoute] string companyId, [FromBody] UpdateFiscalYearRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateFiscalYear = await _mediator.Send(command);
            #region Switch Statement
            return updateFiscalYear switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateFiscalYear.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateFiscalYear.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateFiscalYear.Errors),
                _ => BadRequest("Invalid Fields for Update fisclay year in Sales")
            };

            #endregion


        }
        #endregion


        #region ClosedFiscalYear
        [HttpPost("ClosedFiscalYear")]

        public async Task<IActionResult> ClosedFiscalYear( [FromBody] CloseFiscalYearRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addClosedFiscalYear = await _mediator.Send(command);
            #region Switch Statement
            return addClosedFiscalYear switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(addClosedFiscalYear.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addClosedFiscalYear.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addClosedFiscalYear.Errors),
                _ => BadRequest("Invalid Fields for add closed fiscal year")
            };

            #endregion


        }
        #endregion

        #region GetReceiptTransactionNumberType
        [HttpGet("GetReceiptTransactionNumberType/{companyId}")]
        public async Task<IActionResult> GetReceiptTransactionNumberType([FromRoute] string companyId)
        {
            var query = new ReceiptTransactionNumberTypeQuery(companyId);
            var receiptTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return receiptTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(receiptTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = receiptTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(receiptTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetPaymentsTransactionNumberType
        [HttpGet("GetPaymentsTransactionNumberType/{companyId}")]
        public async Task<IActionResult> GetPaymentsTransactionNumberType([FromRoute] string companyId)
        {
            var query = new GetPaymentTransactionNumberTypeQuery(companyId);
            var paymentsTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return paymentsTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentsTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = paymentsTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentsTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateReceiptTransactionType
        [HttpPatch("UpdateReceiptTransactionType/{companyId}")]

        public async Task<IActionResult> UpdateReceiptTransactionType([FromRoute] string companyId, [FromBody] UpdateReceiptTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateReceiptTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateReceiptTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateReceiptTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateReceiptTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateReceiptTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Receipt Transaction Status")
            };

            #endregion


        }
        #endregion


        #region UpdatePaymentTransactionType
        [HttpPatch("UpdatePaymentTransactionType/{companyId}")]

        public async Task<IActionResult> UpdatePaymentTransactionType([FromRoute] string companyId, [FromBody] UpdatePaymentTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updatePaymentTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updatePaymentTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePaymentTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePaymentTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePaymentTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Payment Transaction status")
            };

            #endregion


        }
        #endregion

        #region GetIncomeTransactionNumberType
        [HttpGet("GetIncomeTransactionNumberType/{companyId}")]
        public async Task<IActionResult> GetIncomeTransactionNumberType([FromRoute] string companyId)
        {
            var query = new GetIncomeTransactionNumberTypeQuery(companyId);
            var incomeTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return incomeTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(incomeTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = incomeTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(incomeTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetExpenseTransactionNumberType
        [HttpGet("GetExpenseTransactionNumberType/{companyId}")]
        public async Task<IActionResult> GetExpenseTransactionNumberType([FromRoute] string companyId)
        {
            var query = new GetExpenseTransactionNumberTypeQuery(companyId);
            var expenseTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return expenseTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expenseTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expenseTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expenseTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateExpenseTransactionType
        [HttpPatch("UpdateExpenseTransactionType/{companyId}")]

        public async Task<IActionResult> UpdateExpenseTransactionType([FromRoute] string companyId, [FromBody] UpdateExpenseTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateExpenseTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateExpenseTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExpenseTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExpenseTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExpenseTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Expense Transaction Status")
            };

            #endregion


        }
        #endregion

        #region UpdateIncomeTransactionType
        [HttpPatch("UpdateIncomeTransactionType/{companyId}")]

        public async Task<IActionResult> UpdateIncomeTransactionType([FromRoute] string companyId, [FromBody] UpdateIncomeTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
            var updateIncomeTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateIncomeTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateIncomeTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateIncomeTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateIncomeTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Income Transaction Status")
            };

            #endregion


        }
        #endregion

        #region GetPurchaseReturnNumberByCompanyId
        [HttpGet("GetPurchaseReturnNumber/{companyId}")]
        public async Task<IActionResult> GetPurchaseReturnNumber([FromRoute] string companyId)
        {
            var query = new GetPurchaseReturnNumberQuery(companyId);
            var returnNumbers = await _mediator.Send(query);
            #region Switch Statement
            return returnNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(returnNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = returnNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(returnNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesReturnNumberByCompanyId
        [HttpGet("GetSalesReturnNumber/{companyId}")]
        public async Task<IActionResult> GetSalesReturnNumber([FromRoute] string companyId)
        {
            var query = new GetSalesReturnNumberQuery(companyId);
            var returnNumbers = await _mediator.Send(query);
            #region Switch Statement
            return returnNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(returnNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = returnNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(returnNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdatePurchaseReturnTypeByCompany
        [HttpPatch("UpdatePurchaseReturnType/{companyId}")]

        public async Task<IActionResult> UpdatePurchaseReturnType([FromRoute] string companyId, [FromBody] UpdatePurchaseReturnTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
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
                _ => BadRequest("Invalid Fields for Update purchase Return Number")
            };

            #endregion


        }
        #endregion

        #region UpdateSalesReturnTypeByCompany
        [HttpPatch("UpdateSalesReturnType/{companyId}")]

        public async Task<IActionResult> UpdateSalesReturnType([FromRoute] string companyId, [FromBody] UpdateSalesReturnTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
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
                _ => BadRequest("Invalid Fields for Update sales  return number")
            };

            #endregion


        }
        #endregion

        #region GetPurchaseQuotationNumberByCompanyId
        [HttpGet("GetPurchaseQuotationNumber/{companyId}")]
        public async Task<IActionResult> GetPurchaseQuotationNumber([FromRoute] string companyId)
        {
            var query = new GetPurchaseQuotationNumberQuery(companyId);
            var quotationNumbers = await _mediator.Send(query);
            #region Switch Statement
            return quotationNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(quotationNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = quotationNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(quotationNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesQuotationNumberByCompanyId
        [HttpGet("GetSalesQuotationNumber/{companyId}")]
        public async Task<IActionResult> GetSalesQuotationNumber([FromRoute] string companyId)
        {
            var query = new GetSalesQuotationTypeQuery(companyId);
            var quotationNumbers = await _mediator.Send(query);
            #region Switch Statement
            return quotationNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(quotationNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = quotationNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(quotationNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdatePurchaseQuotationTypeByCompany
        [HttpPatch("UpdatePurchaseQuotationType/{companyId}")]

        public async Task<IActionResult> UpdatePurchaseQuotationType([FromRoute] string companyId, [FromBody] UpdatePurchaseQuotationTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
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
                _ => BadRequest("Invalid Fields for Update purchase Quotation Number")
            };

            #endregion


        }
        #endregion

        #region UpdateSalesQuotationTypeByCompany
        [HttpPatch("UpdateSalesQuotationType/{companyId}")]

        public async Task<IActionResult> UpdateSalesQuotationType([FromRoute] string companyId, [FromBody] UpdateSalesQuotationTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(companyId);
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
                _ => BadRequest("Invalid Fields for Update sales Quotation Number")
            };

            #endregion


        }
        #endregion

    }
}
