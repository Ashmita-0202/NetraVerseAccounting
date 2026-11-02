using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddOrganization
{
public record AddOrganizationCommand
    (
         
            string name,
            string address,
            string email,
            string phoneNumber,
            string mobileNumber,
            string logo,
            int provinceId
   ): IRequest<Result<AddOrganizationResponse>>;
}
