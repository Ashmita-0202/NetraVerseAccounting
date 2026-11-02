using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddInstitution
{
    public record AddInstitutionCommand
    (
          
            string name,
            string address,
            string email,
            string shortName,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            bool isDeleted,
            string organizationId

    ) :IRequest<Result<AddInstitutionResponse>>;
}
