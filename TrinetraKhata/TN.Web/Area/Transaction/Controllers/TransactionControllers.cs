using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NV.Payment.Application.Payment.Command.DeletePayment;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt.RequestCommandMapper;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddExpense;
using TN.Transactions.Application.Transactions.Command.AddExpense.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.AddIncome.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.AddPayments.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Command.AddReceipt.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.AddTransactions;
using TN.Transactions.Application.Transactions.Command.AddTransactions.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.DeleteExpense;
using TN.Transactions.Application.Transactions.Command.DeleteIncome;
using TN.Transactions.Application.Transactions.Command.DeletePayment;
using TN.Transactions.Application.Transactions.Command.DeleteReceipt;
using TN.Transactions.Application.Transactions.Command.DeleteTransactions;
using TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt;
using TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Command.UpdateExpense.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdateIncome.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Command.UpdatePayment.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Command.UpdateTransactions;
using TN.Transactions.Application.Transactions.Command.UpdateTransactions.RequestCommandMapper;
using TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate;
using TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllExpense;
using TN.Transactions.Application.Transactions.Queries.GetAllIncome;
using TN.Transactions.Application.Transactions.Queries.GetAllPayments;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Transactions.Application.Transactions.Queries.GetAllTransactions;
using TN.Transactions.Application.Transactions.Queries.GetExpenseById;
using TN.Transactions.Application.Transactions.Queries.GetIncomeById;
using TN.Transactions.Application.Transactions.Queries.GetPaymentById;
using TN.Transactions.Application.Transactions.Queries.GetReceiptById;
using TN.Transactions.Application.Transactions.Queries.GetTransactionsById;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;
using TN.Web.Controllers;

namespace TN.Web.Area.Transaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionControllers : BaseController
    {
        private readonly IMediator _mediator;
        public TransactionControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }

        #region Transaction

        #region VouchersReceipt
        [HttpGet("vouchers-receipts/{id}")]
        public async Task<IActionResult> VouchersReceipt([FromRoute] string id)
        {
            var query = new ReceiptVouchersByQuery(id);
            var receiptVouchersResult = await _mediator.Send(query);
            #region Switch Statement
            return receiptVouchersResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(receiptVouchersResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = receiptVouchersResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(receiptVouchersResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddTransactions
        [HttpPost("AddTransactions")]

        public async Task<IActionResult> Add([FromBody] AddTransactionsRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addTransactionResult = await _mediator.Send(command);
            #region Switch Statement
            return addTransactionResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Add), addTransactionResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addTransactionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addTransactionResult.Errors),
                _ => BadRequest("Invalid Fields for Add transactions ")

            };

            #endregion
        }
        #endregion

        #region AllTransactions
        [HttpGet("all-Transactions")]
        public async Task<IActionResult> AllTransactions([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllTransactionsByQuery(paginationRequest);
            var transactionsResult = await _mediator.Send(query);
            #region Switch Statement
            return transactionsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(transactionsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = transactionsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(transactionsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region TransactionsById
        [HttpGet("Transactions/{id}")]
        public async Task<IActionResult> GetTransactionsById([FromRoute] string id)
        {
            var query = new GetTransactionsByIdQuery(id);
            var transactionsResult = await _mediator.Send(query);
            #region Switch Statement
            return transactionsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(transactionsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = transactionsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(transactionsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteTransactions
        [HttpDelete("DeleteTransactions/{id}")]

        public async Task<IActionResult> DeleteTransactions([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteTransactionsCommand(id);
            var deleteTransactionsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteTransactionsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteTransactionsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteTransactionsResult.Errors),
                _ => BadRequest("Invalid Fields for Delete TransactionsDetails")
            };

            #endregion
        }
        #endregion

        #region UpdateTransactions
        [HttpPatch("UpdateTransactions/{id}")]

        public async Task<IActionResult> UpdateTransactions([FromRoute] string id, [FromBody] UpdateTransactionsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateTransactionsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateTransactionsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTransactionsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateTransactionsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTransactionsResult.Errors),
                _ => BadRequest("Invalid Fields for Update transactions  result")
            };

            #endregion


        }
        #endregion

        #endregion


        #region Receipt

        #region AddReceipt
        [HttpPost("AddReceipt")]

        public async Task<IActionResult> AddReceipt([FromBody] AddReceiptRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addReceiptResult = await _mediator.Send(command);
            #region Switch Statement
            return addReceiptResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddReceipt), addReceiptResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addReceiptResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addReceiptResult.Errors),
                _ => BadRequest("Invalid Fields for Add transactions ")

            };

            #endregion
        }
        #endregion

        #region AllReceipt
        [HttpGet("all-Receipt")]
        public async Task<IActionResult> AllReceipt([FromQuery] PaginationRequest paginationRequest,string? ledgerId)
        {
            var query = new GetAllReceiptQuery(paginationRequest, ledgerId);
            var receiptResult = await _mediator.Send(query);
            #region Switch Statement
            return receiptResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(receiptResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = receiptResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(receiptResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region GetReceiptById
        [HttpGet("Receipt/{id}")]
        public async Task<IActionResult> GetReceiptById([FromRoute] string id)
        {
            var query = new GetReceiptByIdQuery(id);
            var receiptResult = await _mediator.Send(query);
            #region Switch Statement
            return receiptResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(receiptResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = receiptResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(receiptResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteReceipt
        [HttpDelete("DeleteReceipt/{id}")]

        public async Task<IActionResult> DeleteReceipt([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteReceiptCommand(id);
            var deleteReceiptResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteReceiptResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteReceiptResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteReceiptResult.Errors),
                _ => BadRequest("Invalid Fields for Delete receipt")
            };

            #endregion
        }
        #endregion

        #region UpdateReceipt
        [HttpPatch("UpdateReceipt/{id}")]

        public async Task<IActionResult> UpdateReceipt([FromRoute] string id, [FromBody] UpdateReceiptRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateReceiptResult = await _mediator.Send(command);
            #region Switch Statement
            return updateReceiptResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateReceiptResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateReceiptResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateReceiptResult.Errors),
                _ => BadRequest("Invalid Fields for Update Receipt  result")
            };

            #endregion


        }
        #endregion

        #region FilterReceiptByDate
        [HttpGet("FilterReceiptByDate")]
        public async Task<IActionResult> GetFilterReceiptByDate([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterReceiptDto filterReceiptDto)
        {
            var query = new GetFilterReceiptQuery(paginationRequest,filterReceiptDto);
            var filterReceiptResult = await _mediator.Send(query);

            #region Switch Statement
            return filterReceiptResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterReceiptResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterReceiptResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterReceiptResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region AddReceiptExcel
        [HttpPost("upload-receipt")]

        public async Task<IActionResult> AddReceiptExcel([FromForm] ReceiptExcelRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addReceiptExcelResult = await _mediator.Send(command);
            #region Switch Statement
            return addReceiptExcelResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddReceiptExcel), addReceiptExcelResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addReceiptExcelResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addReceiptExcelResult.Errors),
                _ => BadRequest("Invalid Fields for Add ReceiptExcel")
            };

            #endregion
        }
        #endregion


        #endregion

        #region Income

        #region AddIncome
        [HttpPost("AddIncome")]

        public async Task<IActionResult> AddIncome([FromBody] AddIncomeRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addIncomeResult = await _mediator.Send(command);
            #region Switch Statement
            return addIncomeResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddIncome), addIncomeResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addIncomeResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addIncomeResult.Errors),
                _ => BadRequest("Invalid Fields for Add Income Transactions ")

            };

            #endregion
        }
        #endregion

        #region AllIncome
        [HttpGet("all-Income")]
        public async Task<IActionResult> AllIncome([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllIncomeQuery(paginationRequest);
            var incomeResult = await _mediator.Send(query);
            #region Switch Statement
            return incomeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(incomeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = incomeResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(incomeResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region IncomeById
        [HttpGet("Income/{id}")]
        public async Task<IActionResult> GetIncomeById([FromRoute] string id)
        {
            var query = new GetIncomeByIdQuery(id);
            var incomeResult = await _mediator.Send(query);
            #region Switch Statement
            return incomeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(incomeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = incomeResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(incomeResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region filterIncomeBydate
        [HttpGet("FilterIncomeByDate")]
        public async Task<IActionResult> GetIncomeFilter([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterIncomeDto filterIncomeDto)
        {
            var query = new GetFilterIncomeByDateQuery(paginationRequest,filterIncomeDto);
            var filterIncomeResult = await _mediator.Send(query);

            #region Switch Statement
            return filterIncomeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterIncomeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterIncomeResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterIncomeResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region UpdateIncome
        [HttpPatch("UpdateIncome/{id}")]

        public async Task<IActionResult> UpdateIncome([FromRoute] string id, [FromBody] UpdateIncomeRequest request)
        {
          
            var command = request.ToCommand(id);
            var updateIncomeResult = await _mediator.Send(command);
            #region Switch Statement
            return updateIncomeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateIncomeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateIncomeResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateIncomeResult.Errors),
                _ => BadRequest("Invalid Fields for Update income  result")
            };

            #endregion


        }
        #endregion

        #region DeleteIncome
        [HttpDelete("DeleteIncome/{id}")]

        public async Task<IActionResult> DeleteIncome([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteIncomeCommand(id);
            var deleteResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteResult.Errors),
                _ => BadRequest("Invalid Fields for Delete income")
            };

            #endregion
        }
        #endregion


        #endregion

        #region Expense
        #region AddExpense
        [HttpPost("AddExpense")]

        public async Task<IActionResult> AddExpense([FromBody] AddExpenseRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addExpenseResult = await _mediator.Send(command);
            #region Switch Statement
            return addExpenseResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddExpense), addExpenseResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addExpenseResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addExpenseResult.Errors),
                _ => BadRequest("Invalid Fields for Add Expense Transactions ")

            };

            #endregion
        }
        #endregion

        #region AllExpense
        [HttpGet("all-Expense")]
        public async Task<IActionResult> AllExpense([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllExpenseQuery(paginationRequest);
            var expenseResult = await _mediator.Send(query);
            #region Switch Statement
            return expenseResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expenseResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expenseResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expenseResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region ExpenseById
        [HttpGet("Expense/{id}")]
        public async Task<IActionResult> GetExpenseById([FromRoute] string id)
        {
            var query = new GetExpenseByIdQuery(id);
            var expenseResponse = await _mediator.Send(query);
            #region Switch Statement
            return expenseResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expenseResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expenseResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expenseResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterExpenseBydate
        [HttpGet("FilterExpenseBydate")]
        public async Task<IActionResult> GetFilterExpense([FromQuery] PaginationRequest paginationRequest,[FromQuery] FilterExpenseDto filterExpenseDto)
        {
            var query = new GetFilterExpenseByDateQuery(paginationRequest, filterExpenseDto);
            var filterExpenseResult = await _mediator.Send(query);

            #region Switch Statement
            return filterExpenseResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterExpenseResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterExpenseResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterExpenseResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region UpdateExpense
        [HttpPatch("UpdateExpense/{id}")]

        public async Task<IActionResult> UpdateExpense([FromRoute] string id, [FromBody] UpdateExpenseRequest request)
        {

            var command = request.ToCommand(id);
            var updateExpenseResult = await _mediator.Send(command);
            #region Switch Statement
            return updateExpenseResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExpenseResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExpenseResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExpenseResult.Errors),
                _ => BadRequest("Invalid Fields for Update Expense  result")
            };

            #endregion


        }
        #endregion

        #region DeleteExpense
        [HttpDelete("DeleteExpense/{id}")]

        public async Task<IActionResult> DeleteExpense([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteExpenseCommand(id);
            var deleteResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteResult.Errors),
                _ => BadRequest("Invalid Fields for Delete expense")
            };

            #endregion
        }
        #endregion

        #endregion

        #region payment

        #region AddPayments
        [HttpPost("AddPayments")]

        public async Task<IActionResult> AddPayments([FromBody] AddPaymentsRequest request)
        {
            //Mapping command and request

            var command = request.ToCommand();
            var addPaymentsResult = await _mediator.Send(command);
            #region Switch Statement
            return addPaymentsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Add), addPaymentsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addPaymentsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addPaymentsResult.Errors),
                _ => BadRequest("Invalid Fields for Add Payments ")

            };

            #endregion
        }
        #endregion
        #region AllPayment
        [HttpGet("all-Payment")]
        public async Task<IActionResult> AllPayment([FromQuery] PaginationRequest paginationRequest, string? ledgerId)
        {
            var query = new GetAllPaymentsQuery(paginationRequest,ledgerId);
            var paymentResult = await _mediator.Send(query);
            #region Switch Statement
            return paymentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = paymentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region PaymentById
        [HttpGet("Payment/{id}")]
        public async Task<IActionResult> GetPaymentById([FromRoute] string id)
        {
            var query = new GetPaymentByIdQuery(id);
            var paymentResponse = await _mediator.Send(query);
            #region Switch Statement
            return paymentResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = paymentResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterPaymentBydate
        [HttpGet("FilterPaymentBydate")]
        public async Task<IActionResult> GetFilterPayment([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPaymentDto filterPaymentDto)
        {
            var query = new GetFilterPaymentQuery(paginationRequest, filterPaymentDto);
            var filterPaymentResult = await _mediator.Send(query);

            #region Switch Statement
            return filterPaymentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterPaymentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterPaymentResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterPaymentResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region UpdatePayment
        [HttpPatch("UpdatePayment/{id}")]

        public async Task<IActionResult> UpdatePayment([FromRoute] string id, [FromBody] UpdatePaymentRequest request)
        {
            var command = request.ToUpdatePaymentCommand(id);
            var updatePaymentResult = await _mediator.Send(command);
            #region Switch Statement
            return updatePaymentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePaymentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePaymentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePaymentResult.Errors),
                _ => BadRequest("Invalid Fields for Update payment  result")
            };

            #endregion


        }
        #endregion

        #region DeletePayment
        [HttpDelete("DeletePayment/{id}")]

        public async Task<IActionResult> DeletePayment([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeletePaymentsCommand(id);
            var deleteResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Payment")
            };

            #endregion
        }
        #endregion



        #endregion



    }
}
