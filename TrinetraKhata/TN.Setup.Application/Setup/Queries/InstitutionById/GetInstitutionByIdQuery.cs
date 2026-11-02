using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.InstitutionById
{
   public record GetInstitutionByIdQuery(string Id):IRequest<Result<GetInstitutionByIdResponse>>;
    
}
