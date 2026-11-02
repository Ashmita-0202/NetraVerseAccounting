using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public record UpdatePaymentTransactionNumberTypeCommand
    (
        TransactionNumberType transactionNumberType,
        string companyId
        ) : IRequest<Result<UpdatePaymentTransactionNumberTypeResponse>>;


}
