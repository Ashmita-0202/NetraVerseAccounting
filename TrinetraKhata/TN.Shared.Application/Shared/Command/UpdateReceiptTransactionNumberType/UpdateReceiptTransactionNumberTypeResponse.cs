using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public record UpdateReceiptTransactionNumberTypeResponse
    (
        TransactionNumberType transactionNumberType,
            string companyId
        );
    
}
