using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json;
using System.Text.Json.Serialization;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.AccountBook.Queries.JournalRegister;
using TN.Reports.Application.AccountBook.Queries.PurchaseRegister;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Reports.Application.DayBook.CashBook;
using TN.Reports.Application.DayBook.FilterPurchaseDayBook;
using TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook;
using TN.Reports.Application.DayBook.FilterSalesDayBook;
using TN.Reports.Application.DayBook.FilterSalesReturnDayBook;
using TN.Reports.Application.ItemRateHistory;
using TN.Reports.Application.ItemwiseProfitReport;
using TN.Reports.Application.ItemwisePurchaseByExpireDate;
using TN.Reports.Application.ItemwisePurchaseReport;
using TN.Reports.Application.ItemwiseSalesReport;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.LedgerBalance.Queries.LedgerSummary;
using TN.Reports.Application.Parties_Statements.Queries;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilter;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate;
using TN.Reports.Application.Profit_LossReport;
using TN.Reports.Application.PurchaseReport;
using TN.Reports.Application.PurchaseReturnReport;
using TN.Reports.Application.PurchaseSummaryReport;
using TN.Reports.Application.SalesReport;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.SalesSummaryReport;
using TN.Reports.Application.StockDetailReport.Queries;
using TN.Reports.Application.StockReport.Queries;
using TN.Reports.Application.TradingAccount;
using TN.Reports.Application.TrialBalance;
using TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Report.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]

    public class ReportController : BaseController
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
        }

        #region AccountBook

        #region SalesRegister
        [HttpGet("GetSalesRegister")]
        public async Task<IActionResult> GetSalesRegister([FromQuery] SalesRegisterDTOs salesRegisterDTOs,[FromQuery] PaginationRequest paginationRequest)
        {
            var query = new SalesRegisterQueries(paginationRequest, salesRegisterDTOs);
            var salesRegisterResult = await _mediator.Send(query);
            #region Switch Statement
            return salesRegisterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesRegisterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesRegisterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesRegisterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region PurchaseRegister
        [HttpGet("GetPurchaseRegister")]
        public async Task<IActionResult> GetPurchaseRegister([FromQuery] PurchaseRegisterDTOs purchaseRegisterDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new PurchaseRegisterQueries(paginationRequest, purchaseRegisterDTOs);
            var purchaseRegisterResult = await _mediator.Send(query);
            #region Switch Statement
            return purchaseRegisterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseRegisterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseRegisterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseRegisterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region JournalRegister
        [HttpGet("GetJournalRegisterByLedger")]
        public async Task<IActionResult> GetJournalRegisterByLedger([FromQuery] JournalRegisterDTOs journalRegisterDTOs , [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new JournalRegisterQuery(paginationRequest, journalRegisterDTOs);
            var journalRegisterResult = await _mediator.Send(query);
            #region Switch Statement
            return journalRegisterResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalRegisterResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalRegisterResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(journalRegisterResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion

        #region BalanceSheet
        [HttpGet("BalanceSheet")]
        public async Task<IActionResult> BalanceSheet([FromQuery] string? id, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new BalanceSheetQuery(paginationRequest, id);
            var balanceSheetResult = await _mediator.Send(query);
            #region Switch Statement
            return balanceSheetResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(balanceSheetResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = balanceSheetResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(balanceSheetResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AnnexReport
        [HttpGet("AnnexReport")]
        public async Task<IActionResult> AnnexReport([FromQuery] AnnexReportDTOs annexReportDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AnnexReportQuery(paginationRequest, annexReportDTOs);
            var annexReportResult = await _mediator.Send(query);
            #region Switch Statement
            return annexReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(annexReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = annexReportResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(annexReportResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region LedgerBalance
        #region LedgerBalance
        [HttpGet("GetLedgerBalanceReport")]
        public async Task<IActionResult> GetLedgerBalanceReport([FromQuery] LedgerBalanceDTOs ledgerBalanceDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new LedgerBalanceReportQueries(paginationRequest, ledgerBalanceDTOs);
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
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region Ledger Summary
        [HttpGet("GetLedgerSummaryByLedger")]
        public async Task<IActionResult> GetLedgerSummaryByLedger([FromQuery] string ledgerId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new LedgerSummaryQueries(paginationRequest, ledgerId);
            var ledgerSummaryResult = await _mediator.Send(query);
            #region Switch Statement
            return ledgerSummaryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ledgerSummaryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ledgerSummaryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(ledgerSummaryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region TrialBalance
        [HttpGet("GetTrialBalance")]
        public async Task<IActionResult> GetTrialBalance([FromQuery] PaginationRequest paginationRequest, [FromQuery] string? companyId)
        {
            var query = new TrialBalanceQuery(paginationRequest,companyId);
            var trialbalanceResult = await _mediator.Send(query);
            #region Switch Statement
            return trialbalanceResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(trialbalanceResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = trialbalanceResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(trialbalanceResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #endregion

        #region VAT Details
        #region PurchaseAndSalesDetails
        [HttpGet("GetVATDetails")]
        public async Task<IActionResult> GetVATDetails([FromQuery] PaginationRequest paginationRequest, [FromQuery] string? companyId )
        {
            var query = new PurchaseAndSalesVATQueries(paginationRequest,companyId);
            var vatDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return vatDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(vatDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = vatDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(vatDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion

        #region TradingAccount
        [HttpGet("GetTradingReport")]
        public async Task<IActionResult> GetTradingReport([FromQuery] string? startDate, string? endDate,string? companyId)
        {
            var query = new GetTradingAccountQuery(startDate,endDate,companyId);
            var tradingAccountResult = await _mediator.Send(query);
            #region Switch Statement
            return tradingAccountResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(tradingAccountResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = tradingAccountResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(tradingAccountResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region ItemwisePurchaseSerialNumber
        [HttpGet("ItemwisePurchaseSerialNumber")]
        public async Task<IActionResult> GetItemwisePurchaseSerialNumber(
          [FromQuery] PaginationRequest paginationRequest,
          [FromQuery] PurchaseReportDtos purchaseReportDtos)
        {
            var query = new GetPurchaseReportQuery(paginationRequest, purchaseReportDtos);
            var result = await _mediator.Send(query);

            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),

                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),

                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),

                _ => BadRequest("Invalid pageIndex or pageSize values.")
            };
        }

        #endregion

        #region ItemwiseSalesSerialNumber
        [HttpGet("ItemwiseSalesSerialNumber")]
        public async Task<IActionResult> GetItemwiseSalesSerialNumber(
          [FromQuery] PaginationRequest paginationRequest,
          [FromQuery] SalesReportDtos salesReportDtos)
        {
            var query = new GetSalesReportQuery(paginationRequest, salesReportDtos);
            var result = await _mediator.Send(query);

            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),

                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),

                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),

                _ => BadRequest("Invalid pageIndex or pageSize values.")
            };
        }

        #endregion

        #region DayBook
        #region FilterSalesDayBookByDate
        [HttpGet("FilterSalesDayBookByDate")]
        public async Task<IActionResult> GetFilterSalesDayBookByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSalesDayBookDto filterSalesDayBookDto)
        {
            var query = new FilterSalesDayBookQuery(paginationRequest, filterSalesDayBookDto);
            var salesDayBookResult = await _mediator.Send(query);

            #region Switch Statement
            return salesDayBookResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesDayBookResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesDayBookResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = salesDayBookResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region FilterSalesReturnDayBookByDate
        [HttpGet("FilterSalesReturnDayBookByDate")]
        public async Task<IActionResult> GetFilterSalesReturnDayBookByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterSalesReturnDayBookDto filterSalesReturnDayBookDto)
        {
            var query = new FilterSalesReturnDayBookQuery(paginationRequest, filterSalesReturnDayBookDto);
            var salesReturnDayBookResult = await _mediator.Send(query);

            #region Switch Statement
            return salesReturnDayBookResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnDayBookResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesReturnDayBookResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = salesReturnDayBookResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region FilterPurchaseDayBookByDate
        [HttpGet("FilterPurchaseDayBookByDate")]
        public async Task<IActionResult> FilterPurchaseDayBookByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterPurchaseDayBookDto filterPurchaseDayBookDto)
        {
            var query = new FilterPurchaseDayBookQuery(paginationRequest, filterPurchaseDayBookDto);
            var purchaseDayBookResult = await _mediator.Send(query);

            #region Switch Statement
            return purchaseDayBookResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseDayBookResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseDayBookResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseDayBookResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region FilterPurchaseReturnDayBookByDate
        [HttpGet("FilterPurchaseReturnDayBookByDate")]
        public async Task<IActionResult> FilterPurchaseReturnDayBookByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] PurchaseReturnDayBookDto purchaseReturnDayBookDto)
        {
            var query = new PurchaseReturnDayBookQuery(paginationRequest, purchaseReturnDayBookDto);
            var purchaseReturnDayBookResult = await _mediator.Send(query);

            #region Switch Statement
            return purchaseReturnDayBookResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseReturnDayBookResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReturnDayBookResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseReturnDayBookResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region FilterCashDayBookByDate
        [HttpGet("FilterCashDayBookByDate")]
        public async Task<IActionResult> FilterCashDayBookByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] CashDayBookDto cashDayBookDto)
        {
            var query = new CashDayBookQuery(paginationRequest, cashDayBookDto);
            var dayBookResult = await _mediator.Send(query);

            #region Switch Statement
            return dayBookResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(dayBookResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = dayBookResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = dayBookResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion


        #endregion


        #region PartyStatement
        [HttpGet("PartyStatement/{partyId}")]
        public async Task<IActionResult> PartyStatement([FromRoute] string partyId)
        {
            var query = new PartyStatementQuery(partyId);
            var partyStatementResult = await _mediator.Send(query);
            #region Switch Statement
            return partyStatementResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(partyStatementResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = partyStatementResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(partyStatementResult.Errors),
                _ => BadRequest("Invalid list of data")
            };
            #endregion

        }
        #region FilterPartyStatement
        [HttpGet("FilterPartyStatement")]
        public async Task<IActionResult> getfilterPartyStatement([FromQuery] PaginationRequest paginationRequest, [FromQuery] PartyStatementDto partyStatementDto)
        {
            var query = new GetPartyStatementFilterQuery(paginationRequest, partyStatementDto);
            var result = await _mediator.Send(query);

            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = result.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion


        #endregion






        [HttpGet("StockReport")]
        public async Task<IActionResult> StockReports([FromQuery] PaginationRequest paginationRequest,
             [FromQuery] StockReportDtos stockReportDto)
        {
            var query = new GetStockReportQuery(paginationRequest, stockReportDto);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }










        #region ProfitAndLossReport
        [HttpGet("GetProfit&LossReport")]
        public async Task<IActionResult> GetProfitAndLossReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] string? CompanyId)
        {
            var query = new GetProfitLossReportQuery(paginationRequest, CompanyId);
            var profitLossResult = await _mediator.Send(query);
            #region Switch Statement
            return profitLossResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(profitLossResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = profitLossResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(profitLossResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region StockDetailReport
        [HttpGet("StockDetailReport")]
        public async Task<IActionResult> StockDetailReport([FromQuery] string companyId, [FromQuery] PaginationRequest paginationRequest, [FromQuery] FilterStockDetailReportDto filterStockDetailReportDto)
        {
            var query = new GetStockDetailReportQuery(companyId,paginationRequest,filterStockDetailReportDto);
            var stockDetailReportResult = await _mediator.Send(query);
            #region Switch Statement
            return stockDetailReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(stockDetailReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = stockDetailReportResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(stockDetailReportResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region SalesReturnReport
        [HttpGet("SalesReturnReport")]
        public async Task<IActionResult> GetSalesReturnReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] SalesReturnReportDto salesReturnReportDto)
        {
            var query = new GetSalesReturnReportQuery(paginationRequest, salesReturnReportDto);
            var salesReturnReportResult = await _mediator.Send(query);

            #region Switch Statement
            return salesReturnReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReturnReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesReturnReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = salesReturnReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region PurchaseReturnReport
        [HttpGet("PurchaseReturnReport")]
        public async Task<IActionResult> GetPurchaseReturnReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] PurchaseReturnReportDto purchaseReturnReportDto)
        {
            var query = new GetPurchaseReturnReportQuery(paginationRequest, purchaseReturnReportDto);
            var purchaseReturnReportResult = await _mediator.Send(query);

            #region Switch Statement
            return purchaseReturnReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseReturnReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReturnReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = purchaseReturnReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region ItemwiseSalesReport
        [HttpGet("ItemwiseSalesReport")]
        public async Task<IActionResult> GetItemwiseSalesReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] ItemwiseSalesReportDto itemwiseSalesReportDto)
        {
            var query = new ItemwiseSalesReportQuery(paginationRequest, itemwiseSalesReportDto);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region ItemwisePurchaseReport
        [HttpGet("ItemwisePurchaseReport")]
        public async Task<IActionResult> GetItemwisePurchaseReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] ItemwisePurchaseReportDto itemwisePurchaseReportDto)
        {
            var query = new ItemwisePurchaseReportQuery(paginationRequest, itemwisePurchaseReportDto);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion
        #region ItemwisePurchaseExpireDate
        [HttpGet("ItemwisePurchaseExpireDate")]
        public async Task<IActionResult> GetItemwisePurchaseExpireDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] ItemwisePurchaseExpireDateDtos itemwisePurchaseExpireDateDtos)
        {
            var query = new GetItemwisePurchaseExpireDateQuery(paginationRequest, itemwisePurchaseExpireDateDtos);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region PurchaseSummaryReport
        [HttpGet("PurchaseSummaryReport")]
        public async Task<IActionResult> GetPurchaseSummaryReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] PurchaseSummaryDtos purchaseSummaryDtos)
        {
            var query = new GetPurchaseSummaryQuery(paginationRequest, purchaseSummaryDtos);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region SalesSummaryReport
        [HttpGet("SalesSummaryReport")]
        public async Task<IActionResult> GetSalesSummaryReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] SalesSummaryDtos salesSummaryDtos)
        {
            var query = new GetSalesSummaryQuery(paginationRequest, salesSummaryDtos);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region ItemwiseProfitReport
        [HttpGet("ItemwiseProfitReport")]
        public async Task<IActionResult> GetItemwiseProfitReport([FromQuery] PaginationRequest paginationRequest, [FromQuery] ItemwiseProfitDtos itemwiseProfitDtos)
        {
            var query = new GetItemwiseProfitQuery(paginationRequest, itemwiseProfitDtos);
            var ReportResult = await _mediator.Send(query);

            #region Switch Statement
            return ReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(ReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = ReportResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = ReportResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion

        #region ItemRateHistoryReport
        [HttpGet("ItemRateHistoryReport")]
        public async Task<IActionResult> GetItemRateHistoryReport(
          [FromQuery] PaginationRequest paginationRequest,
          [FromQuery] ItemRateHistoryDtos itemRateHistoryDtos)
        {
            var query = new GetItemRateHistoryQuery(paginationRequest, itemRateHistoryDtos);
            var result = await _mediator.Send(query);

            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),

                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),

                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),

                _ => BadRequest("Invalid pageIndex or pageSize values.")
            };
        }

    }





}

#endregion





