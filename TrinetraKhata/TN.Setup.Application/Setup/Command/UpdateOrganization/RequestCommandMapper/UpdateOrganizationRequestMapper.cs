using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Domain.Entities;

namespace TN.Setup.Application.Setup.Command.UpdateOrganization.RequestCommandMapper
{
  public static class UpdateOrganizationRequestMapper
    {
        public static UpdateOrganizationCommand ToCommand(this UpdateOrganizationRequest request, string organizationId)
        {

            return new UpdateOrganizationCommand(organizationId, request.Name, request.Address, request.Email, request.PhoneNumber, request.MobileNumber, request.Logo, request.ProvinceId);
        }
    }
}


