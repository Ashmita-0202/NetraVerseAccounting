using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.AddCustomer.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.Account.Command.AddCustomerCategory.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntry.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.AddJournalEntryDetails.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.AddLedger.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.Account.Command.AddLedgerGroup.RequestCommandMapper;
using TN.Account.Application.Account.Command.AddSubledgerGroup;
using TN.Account.Application.Account.Command.AddSubledgerGroup.RequestCommandMapper;
using TN.Account.Application.Account.Command.BillSundry;
using TN.Account.Application.Account.Command.BillSundry.RequestCommandMapper;
using TN.Account.Application.Account.Command.DeleteBillSundry;
using TN.Account.Application.Account.Command.DeleteCustomer;
using TN.Account.Application.Account.Command.DeleteCustomerCategory;
using TN.Account.Application.Account.Command.DeleteJournalEntry;
using TN.Account.Application.Account.Command.DeleteLedger;
using TN.Account.Application.Account.Command.DeleteLedgerGroup;
using TN.Account.Application.Account.Command.DeleteMaster;
using TN.Account.Application.Account.Command.DeleteSubledgerGroup;
using TN.Account.Application.Account.Command.ImportExcelForLedgers;
using TN.Account.Application.Account.Command.ImportExcelForLedgers.RequestCommandMaper;
using TN.Account.Application.Account.Command.UpdateBillSundry;
using TN.Account.Application.Account.Command.UpdateBillSundry.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Command.UpdateCustomer.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateCustomerCategory;
using TN.Account.Application.Account.Command.UpdateCustomerCategory.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Command.UpdateJournalEntry.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Command.UpdateLedger.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.Account.Command.UpdateLedgerGroup.RequestCommandMapper;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup.RequestCommandMapper;
using TN.Account.Application.Account.Queries.AccountPayable;
using TN.Account.Application.Account.Queries.AccountReceivable;
using TN.Account.Application.Account.Queries.ARAPByLedgerId;
using TN.Account.Application.Account.Queries.ChartOfAccounts;
using TN.Account.Application.Account.Queries.CurrentJournalReferenceNumber;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.Account.Queries.CustomerCategory;
using TN.Account.Application.Account.Queries.CustomerCategoryById;
using TN.Account.Application.Account.Queries.FilterJournalByDate;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup;
using TN.Account.Application.Account.Queries.FilterLedgerGroup;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate;
using TN.Account.Application.Account.Queries.FilterSundryBill;
using TN.Account.Application.Account.Queries.GetBalance;
using TN.Account.Application.Account.Queries.GetBillSundry;
using TN.Account.Application.Account.Queries.GetBillSundryById;
using TN.Account.Application.Account.Queries.GetLedgerGroupByMasterId;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Account.Application.Account.Queries.JournalEntry;
using TN.Account.Application.Account.Queries.JournalEntryById;
using TN.Account.Application.Account.Queries.JournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetailsById;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.Account.Queries.LedgerByLedgerGroupId;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.Account.Queries.LedgerGroupById;
using TN.Account.Application.Account.Queries.master;
using TN.Account.Application.Account.Queries.OpeningClosingBalance;
using TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger;
using TN.Account.Application.Account.Queries.OpeningStockByCompanyId;
using TN.Account.Application.Account.Queries.Parties;
using TN.Account.Application.Account.Queries.SubledgerGroup;
using TN.Account.Application.Account.Queries.SubledgerGroupById;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;


namespace TN.Web.Area.Account.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountControllers : BaseController
    {
        private readonly IMediator _mediator;
        public AccountControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }
        #region BillSundry
        #region AddBillSundry
        [HttpPost("AddBillSundry")]

        public async Task<IActionResult> AddBillSundry([FromBody] AddBillSundryRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addBillSundryResult = await _mediator.Send(command);
            #region Switch Statement
            return addBillSundryResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedger), addBillSundryResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addBillSundryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addBillSundryResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger ")

            };

            #endregion
        }
        #endregion

        #region AllBillSundry
        [HttpGet("Get-BillSundry")]
        public async Task<IActionResult> AllBillSundry([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetBillSundryQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region BillSundryById
        [HttpGet("BillSundry/{Id}")]
        public async Task<IActionResult> GetBillSundryById([FromRoute] string Id)
        {
            var query = new GetBillSundryByIdQuery(Id);
            var Result = await _mediator.Send(query);
            #region Switch Statement
            return Result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(Result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = Result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(Result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteBillSundry
        [HttpDelete("DeleteBillSundry/{id}")]

        public async Task<IActionResult> DeleteBillSundry([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteBillSundryCommand(id);
            var deleteResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Bil Sundry")
            };

            #endregion
        }
        #endregion

        #region UpdateBillSundry
        [HttpPatch("UpdateBillSundry/{Id}")]

        public async Task<IActionResult> UpdateBillSundryById([FromRoute] string Id, [FromBody] UpdateBillSundryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateBillSundryResult = await _mediator.Send(command);
            #region Switch Statement
            return updateBillSundryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateBillSundryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateBillSundryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBillSundryResult.Errors),
                _ => BadRequest("Invalid Fields for update Bill sundry")
            };

            #endregion


        }
        #endregion

        #region FilterBillSundry
        [HttpGet("FilterBillSundry")]
        public async Task<IActionResult> GetFilterBillSundry([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSundryBillDto filterSundryBillDto)
        {
            var query = new FilterSundryBillQuery(paginationRequest, filterSundryBillDto);
            var filterResult = await _mediator.Send(query);
            #region Switch Statement
            return filterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion
        #region ArApByLedger
        [HttpGet("ArApByLedger/{LedgerId}")]
        public async Task<IActionResult> GetArApByLedger([FromRoute] string LedgerId)
        {
            var query = new ArApByLedgerQuery(LedgerId);
            var arapResult = await _mediator.Send(query);
            #region Switch Statement
            return arapResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(arapResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = arapResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(arapResult.Errors),
                _ => BadRequest("Invalid LedgerId")
            };
            #endregion

        }
        #endregion

        #region OpeningClosingBalance

        #region AllOpeningClosingBalance
        [HttpGet("AllOpeningClosingBalance")]
        public async Task<IActionResult> AllOpeningClosingBalance([FromQuery] string fyId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new OpeningClosingBalanceQuery(fyId, paginationRequest);
            var openingClosingBalance = await _mediator.Send(query);
            #region Switch Statement
            return openingClosingBalance switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(openingClosingBalance.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = openingClosingBalance.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region OpeningClosingBalanceByLedgerId
        [HttpGet("OpeningClosingBalanceByLedger")]
        public async Task<IActionResult> OpeningClosingBalanceByLedger([FromQuery] OpeningClosingBalanceDTOs openingClosingBalanceDTOs)
        {
            var query = new OpeningClosingBalanceByLedgerQuery(openingClosingBalanceDTOs);
            var openingClosingBalanceByLedger = await _mediator.Send(query);
            #region Switch Statement
            return openingClosingBalanceByLedger switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(openingClosingBalanceByLedger.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = openingClosingBalanceByLedger.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(openingClosingBalanceByLedger.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #endregion

        #region Chart Of Account
        #region ChartOfAccounts
        [HttpGet("ChartOfAccounts")]
        public async Task<IActionResult> ChartOfAccounts()
        {
            var query = new ChartsOfAccountsQuery();
            var chartOfAccountResult = await _mediator.Send(query);
            #region Switch Statement
            return chartOfAccountResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(chartOfAccountResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = chartOfAccountResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        #endregion

        #endregion


        #region AccountReceivable
        [HttpGet("AccountReceivable")]
        public async Task<IActionResult> AccountReceivable([FromQuery] PaginationRequest paginationRequest, string? ledgerId)
        {
            var query = new AccountReceivableQuery(paginationRequest,ledgerId);
            var accountReceivableResult = await _mediator.Send(query);
            #region Switch Statement
            return accountReceivableResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(accountReceivableResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = accountReceivableResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region AccountPayable
        [HttpGet("AccountPayable")]
        public async Task<IActionResult> AccountPayable([FromQuery] PaginationRequest paginationRequest, string? ledgerId)
        {
            var query = new AccountPayableQuery(paginationRequest,ledgerId);
            var accountPayableResult = await _mediator.Send(query);
            #region Switch Statement
            return accountPayableResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(accountPayableResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = accountPayableResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region Master
        #region AllMaster
        [HttpGet("all-master")]
        public async Task<IActionResult> AllMaster([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllMasterByQuery(paginationRequest);
            var masterResult = await _mediator.Send(query);
            #region Switch Statement
            return masterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(masterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = masterResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region MasterById
        [HttpGet("Master/{MasterId}")]
        public async Task<IActionResult> GetByMasterId([FromRoute] string MasterId)
        {
            var query = new GetMasterByIdQuery(MasterId);
            var masterResult = await _mediator.Send(query);
            #region Switch Statement
            return masterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(masterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = masterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteMaster
        [HttpDelete("DeleteMaster/{Id}")]

        public async Task<IActionResult> DeleteMaster([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var command = new DeleteMasterCommand(Id);
            var deleteMasterResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteMasterResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteMasterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteMasterResult.Errors),
                _ => BadRequest("Invalid Fields for Add Master")
            };

            #endregion
        }
        #endregion
        #endregion

        

        #region Ledger
        #region LedgerBalance
        [HttpGet("LedgerBalance/{LedgerId}")]
        public async Task<IActionResult> LedgerBalance([FromRoute] string LedgerId)
        {
            var query = new GetBalanceByQuery(LedgerId);
            var ledgerBalanceResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerBalanceResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerBalanceResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerBalanceResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(ledgerBalanceResult.Errors),
                _ => BadRequest("Invalid ledgerId Fields")
            };
            #endregion

        }
        #endregion

        #region FilterLedgerBySelectedLedgerGroup
        [HttpGet("FilterLedgerBySelectedLedgerGroup")]
        public async Task<IActionResult> FilterLedgerBySelectedLedgerGroup()
        {
            var query = new FilterLedgerBySelectedLedgerGroupQuery();
            var ledgerResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid ")
            };
            #endregion

        }

        #endregion

        #region AllLedger
        [HttpGet("all-ledger")]
        public async Task<IActionResult> AllLedger([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllLedgerByQuery(paginationRequest);
            var ledgerResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AllLedgerByLedgerGroupId
        [HttpGet("LedgerByLedgerGroupId/{ledgerGroupId}")]
        public async Task<IActionResult> AllLedgerByLedgerGroupId([FromRoute] string ledgerGroupId)
        {
            var query = new GetAllLedgerByLedgerGroupIdQuery(ledgerGroupId);
            var ledgerResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid ledgerGroupId")
            };
            #endregion

        }

        #endregion

        #region AddLedger
        [HttpPost("AddLedger")]

        public async Task<IActionResult> AddLedger([FromBody] AddLedgerRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addLedgerResult = await _mediator.Send(command);
            #region Switch Statement
            return addLedgerResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedger), addLedgerResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addLedgerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addLedgerResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger ")
            };

            #endregion
        }

        #endregion

        #region DeleteLedger
        [HttpDelete("DeleteLedger/{id}")]

        public async Task<IActionResult> DeleteLedger([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteLedgerCommand(id);
            var deleteLedgerResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteLedgerResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteLedgerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteLedgerResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region LedgerById
        [HttpGet("Ledger/{LedgerId}")]
        public async Task<IActionResult> GetByLedgerId([FromRoute] string LedgerId)
        {
            var query = new GetLedgerByIdQuery(LedgerId);
            var ledgerResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(ledgerResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateLedger
        [HttpPatch("UpdateLedger/{Id}")]

        public async Task<IActionResult> UpdateLedger([FromRoute] string Id, [FromBody] UpdateLedgerRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateLedgerResult = await _mediator.Send(command);
            #region Switch Statement
            return updateLedgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateLedgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateLedgerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateLedgerResult.Errors),
                _ => BadRequest("Invalid Fields for Assign Ledger")
            };

            #endregion


        }
        #endregion

        #region FilterLedger
        [HttpGet("GetFilterLedger")]
        public async Task<IActionResult> GetFilterLedger([FromQuery] FilterLedgerDto filterLedgerDto, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetFilterLedgerByQuery(paginationRequest, filterLedgerDto);
            var filterLedgerResult = await _mediator.Send(query);
            #region Switch Statement
            return filterLedgerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterLedgerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterLedgerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterLedgerResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddLedgerExcel
        [HttpPost("upload-ledger")]

        public async Task<IActionResult> AddLedgerExcel([FromForm] LedgerExcelRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addLedgerExcelResult = await _mediator.Send(command);
            #region Switch Statement
            return addLedgerExcelResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedgerExcel), addLedgerExcelResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addLedgerExcelResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addLedgerExcelResult.Errors),
                _ => BadRequest("Invalid Fields for Add LedgerExcel")
            };

            #endregion
        }
        #endregion

        #endregion

        #region LedgerGroup

        #region AllLedgerGroup
        [HttpGet("all-ledgerGroup")]
        public async Task<IActionResult> AllLedgerGroup([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllLedgerGroupByQuery(paginationRequest);
            var ledgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerGroupResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region LedgerGroupById
        [HttpGet("LedgerGroup/{LedgerGroupId}")]
        public async Task<IActionResult> GetByLedgerGroupId([FromRoute] string LedgerGroupId)
        {
            var query = new GetLedgerGroupByIdQuery(LedgerGroupId);
            var ledgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(ledgerGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region LedgerGroupByMasterId
        [HttpGet("LedgerGroupByMasterId/{masterId}")]
        public async Task<IActionResult> LedgerGroupByMasterId([FromRoute] string masterId)
        {
            var query = new GetLedgerGroupByMasterIdQuery(masterId);
            var ledgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(ledgerGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AddLedgerGroup
        [HttpPost("AddLedgerGroup")]

        public async Task<IActionResult> AddLedgerGroup([FromBody] AddLedgerGroupRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return addLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedgerGroup), addLedgerGroupResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger Group")
            };

            #endregion
        }

        #endregion

        #region UpdateLedgerGroup
        [HttpPatch("UpdateLedgerGroup/{Id}")]

        public async Task<IActionResult> AssignModules([FromRoute] string Id, [FromBody] UpdateLedgerGroupRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return updateLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateLedgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Assign LedgerGroup")
            };

            #endregion


        }
        #endregion

        #region DeleteLedgerGroup
        [HttpDelete("DeleteLedgerGroup/{id}")]

        public async Task<IActionResult> DeleteLedgerGroup([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteLedgerGroupCommand(id);
            var deleteLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteLedgerGroupResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region FilterLedgerGroup
        [HttpGet("GetFilterLedgerGroup")]
        public async Task<IActionResult> GetFilterLedgerGroup([FromQuery] FilterLedgerGroupDto filterLedgerGroupDto, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetFilterLedgerGroupQuery(paginationRequest, filterLedgerGroupDto);
            var filterLedgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return filterLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterLedgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterLedgerGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion
        }
        #endregion
        #endregion

        #region SubledgerGroup

        #region AddLedgerGroup
        [HttpPost("AddSubledgerGroup")]

        public async Task<IActionResult> AddSubledgerGroup([FromBody] AddSubledgerGroupRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addSubLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return addSubLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSubledgerGroup), addSubLedgerGroupResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addSubLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSubLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger Group")
            };

            #endregion
        }

        #endregion

        #region AllSubLedgerGroup
        [HttpGet("all-subLedgerGroup")]
        public async Task<IActionResult> AllSubLedgerGroup([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllSubledgerGroupQuery(paginationRequest);
            var subledgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return subledgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(subledgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = subledgerGroupResult.Message }),

                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region SubLedgerGroupById
        [HttpGet("SubLedgerGroup/{id}")]
        public async Task<IActionResult> SubLedgerGroupById([FromRoute] string id)
        {
            var query = new GetSubledgerGroupByIdQuery(id);
            var subLedgerGroupResult = await _mediator.Send(query);
            #region Switch Statement
            return subLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(subLedgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = subLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(subLedgerGroupResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteSubLedgerGroup
        [HttpDelete("DeleteSubLedgerGroup/{id}")]

        public async Task<IActionResult> DeleteSubLedgerGroup([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSubledgerGroupCommand(id);
            var deleteSubLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteSubLedgerGroupResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSubLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSubLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region UpdateSubLedgerGroup
        [HttpPatch("UpdateSubLedgerGroup/{id}")]

        public async Task<IActionResult> UpdateSubLedgerGroup([FromRoute] string id, [FromBody] UpdateSubledgerGroupRequest request)
        {

            var command = request.ToCommand(id);
            var updateSubLedgerGroupResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSubLedgerGroupResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSubLedgerGroupResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSubLedgerGroupResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSubLedgerGroupResult.Errors),
                _ => BadRequest("Invalid Fields for Assign LedgerGroup")
            };

            #endregion


        }
        #endregion

        #region FilterSubLedgerGroup
        [HttpGet("FilterSubLedgerGroup")]
        public async Task<IActionResult> FilterSubLedgerGroup([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterSubledgerGroupDto filterSubledgerGroupDto)
        {
            var query = new GetFilterSubledgerGroupQuery(paginationRequest, filterSubledgerGroupDto);
            var filterResult = await _mediator.Send(query);
            #region Switch Statement
            return filterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #endregion


        #region Customer
        #region AllCustomer
        [HttpGet("all-customer")]
        public async Task<IActionResult> AllCustomer([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllCustomerByQuery(paginationRequest);
            var customerResult = await _mediator.Send(query);
            #region Switch Statement
            return customerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(customerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = customerResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region CustomerById
        [HttpGet("Customer/{id}")]
        public async Task<IActionResult> GetByCustomerId([FromRoute] string id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customerResult = await _mediator.Send(query);
            #region Switch Statement
            return customerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(customerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = customerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(customerResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteCustomer
        [HttpDelete("DeleteCustomer/{id}")]

        public async Task<IActionResult> DeleteCustomer([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCommand(id);
            var deleteCustomerResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteCustomerResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteCustomerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCustomerResult.Errors),
                _ => BadRequest("Invalid Fields for Add CustomerCategory")

            };

            #endregion
        }
        #endregion

        #region AddCustomer
        [HttpPost("AddCustomer")]

        public async Task<IActionResult> AddCustomer([FromBody] AddCustomerRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addCustomerResult = await _mediator.Send(command);
            #region Switch Statement
            return addCustomerResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedger), addCustomerResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCustomerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCustomerResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger ")

            };

            #endregion
        }
        #endregion

        #region UpdateCustomer
        [HttpPatch("UpdateCustomer/{id}")]

        public async Task<IActionResult> UpdateCustomer([FromRoute] string id, [FromBody] UpdateCustomerRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateCustomerResult = await _mediator.Send(command);
            #region Switch Statement
            return updateCustomerResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCustomerResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateCustomerResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCustomerResult.Errors),
                _ => BadRequest("Invalid Fields for Update Customer result")
            };

            #endregion


        }
        #endregion
        #endregion

        #region CustomerCategory
        #region AllCustomerCategory
        [HttpGet("all-customercategory")]
        public async Task<IActionResult> AllCustomerCategory([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllCustomerCategoryByQuery(paginationRequest);
            var customerCategoryResult = await _mediator.Send(query);
            #region Switch Statement
            return customerCategoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(customerCategoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = customerCategoryResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region CustomerCategoryById
        [HttpGet("CustomerCategory/{id}")]
        public async Task<IActionResult> GetByCustomerCategoryId([FromRoute] string id)
        {
            var query = new GetCustomerCategoryByIdQuery(id);
            var customerCategoryResult = await _mediator.Send(query);
            #region Switch Statement
            return customerCategoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(customerCategoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = customerCategoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(customerCategoryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteCustomerCategory
        [HttpDelete("DeleteCustomerCategory/{id}")]

        public async Task<IActionResult> DeleteCustomerCategory([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCategoryCommand(id);
            var deleteCustomerCategoryResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteCustomerCategoryResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteCustomerCategoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCustomerCategoryResult.Errors),
                _ => BadRequest("Invalid Fields for Add CustomerCategory")

            };

            #endregion
        }


        #endregion

        #region AddCustomerCategory
        [HttpPost("AddCustomerCategory")]

        public async Task<IActionResult> AddCustomerCategory([FromBody] AddCustomerCategoryRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addCustomerCategoryResult = await _mediator.Send(command);
            #region Switch Statement
            return addCustomerCategoryResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedger), addCustomerCategoryResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCustomerCategoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCustomerCategoryResult.Errors),
                _ => BadRequest("Invalid Fields for Add Ledger ")

            };

            #endregion
        }
        #endregion

        #region UpdateCustomerCategory
        [HttpPatch("UpdateCustomerCategory/{id}")]

        public async Task<IActionResult> UpdateCustomerCategory([FromRoute] string id, [FromBody] UpdateCustomerCategoryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateCustomerCategoryResult = await _mediator.Send(command);
            #region Switch Statement
            return updateCustomerCategoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCustomerCategoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateCustomerCategoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCustomerCategoryResult.Errors),
                _ => BadRequest("Invalid Fields for Update Customer result")
            };

            #endregion


        }
        #endregion
        #endregion

        #region JournalEntry
        #region AddJournal Entry
        [HttpPost("AddJournal")]

        public async Task<IActionResult> AddJournal([FromBody] AddJournalEntryRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addJournalResult = await _mediator.Send(command);
            #region Switch Statement
            return addJournalResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddLedger), addJournalResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addJournalResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addJournalResult.Errors),
                _ => BadRequest("Invalid Fields for Add Journal ")

            };

            #endregion
        }
        #endregion

        #region AllJournalEntry
        [HttpGet("all-journal-entry")]
        public async Task<IActionResult> AllJournalEntry([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetallJournalEntryByQuery(paginationRequest);

            var journalEntryResult = await _mediator.Send(query);

            #region Switch Statement
            return journalEntryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalEntryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalEntryResult.Message }),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion
        }
        #endregion

        #region JournalById
        [HttpGet("Journal/{id}")]
        public async Task<IActionResult> GetByJournalId([FromRoute] string id)
        {
            var query = new GetJournalEntryByIdQuery(id);
            var journalResult = await _mediator.Send(query);
            #region Switch Statement
            return journalResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(journalResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteJournalEntry
        [HttpDelete("DeleteJournalEntry/{id}")]

        public async Task<IActionResult> DeleteJournalEntry([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteJournalEntryCommand(id);
            var deleteJournalResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteJournalResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteJournalResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteJournalResult.Errors),
                _ => BadRequest("Invalid Fields for Add journal")

            };

            #endregion
        }


        #endregion

        #region UpdateJournalEntry
        [HttpPatch("UpdateJournalEntry/{id}")]

        public async Task<IActionResult> UpdateJournalEntry([FromRoute] string id, [FromBody] UpdateJournalEntryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateJournalEntryResult = await _mediator.Send(command);
            #region Switch Statement
            return updateJournalEntryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateJournalEntryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateJournalEntryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateJournalEntryResult.Errors),
                _ => BadRequest("Invalid Fields for Update Journal result")
            };

            #endregion


        }
        #endregion

        #region FilterJournalByDate
        [HttpGet("FilterJournalByDate")]
        public async Task<IActionResult> GetJournalFilter([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterJournalDTOs filterJournalDTOs)
        {
            var query = new FilterJournalBySelectedDateQuery(paginationRequest, filterJournalDTOs);
            var journalResult = await _mediator.Send(query);

            #region Switch Statement
            return journalResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = journalResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #endregion

        #region JournalEntryDetails
        #region AllJournalEntryDetails
        [HttpGet("all-journalEntryDetails")]
        public async Task<IActionResult> GetAllJournalDetails([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllJournalEntryDetailsByQuery(paginationRequest);
            var journalResult = await _mediator.Send(query);
            #region Switch Statement
            return journalResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalResult.Message }),

                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region JournalDetailsById
        [HttpGet("JournalEntryDetails/{id}")]
        public async Task<IActionResult> GetJournalDetailsById([FromRoute] string id)
        {
            var query = new GetJournalEntryDetailsByIdQuery(id);
            var journalDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return journalDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(journalDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateJournalEntryDetails
        [HttpPatch("UpdateJournalEntryDetails/{id}")]

        public async Task<IActionResult> UpdateJournalDetails([FromRoute] string id, [FromBody] UpdateJournalDetailsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateJournalDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateJournalDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateJournalDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateJournalDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateJournalDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Update Journal details result")
            };

            #endregion


        }
        #endregion

        #region JournalEntryDetails
        [HttpPost("AddJournalEntryDetails")]

        public async Task<IActionResult> AddJournalEntryDetails([FromBody] AddJournalEntryDetailsRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addJournalDetailsResult = await _mediator.Send(command);
            #region Switch Statement
            return addJournalDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddJournalEntryDetails), addJournalDetailsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addJournalDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addJournalDetailsResult.Errors),
                _ => BadRequest("Invalid Fields for Add addJournalDetail ")

            };

            #endregion
        }
        #endregion

        #region CurrentJournalReferenceNumber
        [HttpGet("CurrentJournalReferenceNumber")]
        public async Task<IActionResult> CurrentJournalReferenceNumber()
        {
            var query = new CurrentJournalRefNoQuery();
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
    


        #region Parties

        #region AllParties
        [HttpGet("all-parties")]
        public async Task<IActionResult> AllParties([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllPartiesByQuery(paginationRequest);
            var partiesResult = await _mediator.Send(query);
            #region Switch Statement
            return partiesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(partiesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = partiesResult.Message }),
                //{ IsSuccess: false, Errors: not null } => HandleFailureResult(masterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterParties
        [HttpGet("GetFilterParties")]
        public async Task<IActionResult> GetFilterParties([FromQuery] FilterPartiesDto filterPartiesDto, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetFilterPartiesQuery(paginationRequest, filterPartiesDto);
            var filterPartiesResult = await _mediator.Send(query);
            #region Switch Statement
            return filterPartiesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterPartiesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterPartiesResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterPartiesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #endregion


        #region OpeningStockByCompanyId
        [HttpGet("OpeningStock/{companyId}")]
        public async Task<IActionResult> OpeningStockByCompanyId([FromRoute] string companyId)
        {
            var query = new GetOpeningStockQuery(companyId);
            var openingStockResult = await _mediator.Send(query);
            #region Switch Statement
            return openingStockResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(openingStockResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = openingStockResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(openingStockResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion


    }



}


#endregion