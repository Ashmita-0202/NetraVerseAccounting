using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType
{
    public record  GetIncomeTransactionNumberTypeQuery
   (

         string companyId
    ):IRequest<Result<GetIncomeTransactionNumberTypeResponse>>;
}
