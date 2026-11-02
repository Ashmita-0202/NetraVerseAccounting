using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType
{
    public record ReceiptTransactionNumberTypeResponse
    (
          TransactionNumberType transactionNumberType = default,
            string companyId = "",
            string? receiptTransactionNoType = null
        );
}
