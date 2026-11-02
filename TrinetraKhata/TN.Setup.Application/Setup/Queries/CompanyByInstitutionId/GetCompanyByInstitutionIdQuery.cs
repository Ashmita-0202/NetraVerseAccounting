using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.CompanyByInstitutionId
{
    public record GetCompanyByInstitutionIdQuery(string institutionId): IRequest<Result<List<GetCompanyByInstitutionIdResponse>>>;
   
}
