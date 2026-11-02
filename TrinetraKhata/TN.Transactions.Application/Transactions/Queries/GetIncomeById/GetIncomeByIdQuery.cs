using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Transactions.Application.Transactions.Queries.GetIncomeById
{
    public record  GetIncomeByIdQuery
    (string id):IRequest<Result<GetIncomeByIdQueryResponse>>;
}
