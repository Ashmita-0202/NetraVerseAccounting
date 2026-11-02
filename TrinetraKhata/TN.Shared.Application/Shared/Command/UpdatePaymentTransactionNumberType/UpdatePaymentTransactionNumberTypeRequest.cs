using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public record UpdatePaymentTransactionNumberTypeRequest
    (
        TransactionNumberType transactionNumberType
        );
}
