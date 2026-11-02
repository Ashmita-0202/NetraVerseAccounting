using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.BillNumberGenerationByCompany
{
   public record BIllNumberGenerationByCompanyQuery
    (string id) :IRequest<Result<BIllNumberGenerationByCompanyQueryResponse>>;
}
