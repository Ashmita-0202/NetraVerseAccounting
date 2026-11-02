using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateOrganization
{
    public record  UpdateOrganizationCommand
    (
            string Id,
            string Name,
            string Address,
            string Email,
            string PhoneNumber,
            string MobileNumber,
            string Logo,
            int ProvinceId

    ) :IRequest<Result<UpdateOrganizationResponse>>;
}
