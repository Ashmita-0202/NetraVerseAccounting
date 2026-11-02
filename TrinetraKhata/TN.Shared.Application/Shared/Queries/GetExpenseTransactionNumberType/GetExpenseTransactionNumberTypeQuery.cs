using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType
{
    public record  GetExpenseTransactionNumberTypeQuery
    (
        string companyId
        
    ):IRequest<Result<GetExpenseTransactionNumberTypeResponse>>;
}
