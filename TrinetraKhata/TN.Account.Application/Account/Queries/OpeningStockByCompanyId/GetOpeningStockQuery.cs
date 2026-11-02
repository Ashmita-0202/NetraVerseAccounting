using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.OpeningStockByCompanyId
{
    public record GetOpeningStockQuery
    (
        string companyId

    ) : IRequest<Result<GetOpeningStockQueryResponse>>;
}
