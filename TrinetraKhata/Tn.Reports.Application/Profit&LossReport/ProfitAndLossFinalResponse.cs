using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  ProfitAndLossFinalResponse
    (
         List<MasterLevelProfitAndLoss> Income,
        List<MasterLevelProfitAndLoss> Expenses,
        decimal GrossProfitOrLoss,
        decimal NetProfitOrLoss,
        List<DutiesAndTaxesDto> DutiesAndTaxes,
        decimal TotalDutiesAndTaxes,
        decimal NetProfitAfterTax
    );
    public record DutiesAndTaxesDto
        (
            string LedgerName,
            decimal Amount,
            string BalanceType

        );
}

