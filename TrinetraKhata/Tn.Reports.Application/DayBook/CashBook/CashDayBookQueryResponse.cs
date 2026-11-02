using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Reports.Application.DayBook.CashBook
{
    public record  CashDayBookQueryResponse
    (
        DateTime? transactionDate,
        string ReferenceNumber,
       TransactionType transactionType,
        string LedgerId,
        string paymentMethodId,
        decimal DebitAmount,
        decimal CreditAmount,
        decimal BalanceAmount

    );
   
}
