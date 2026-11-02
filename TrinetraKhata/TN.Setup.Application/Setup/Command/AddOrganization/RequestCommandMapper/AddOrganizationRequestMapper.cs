using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddModule;

namespace TN.Setup.Application.Setup.Command.AddOrganization.RequestCommandMapper
{
    public static class AddOrganizationRequestMapper
    {
        public static AddOrganizationCommand ToCommand(this AddOrganizationRequest request)
        {
            return new AddOrganizationCommand(request.name,request.address, request.email,request.phoneNumber,request.mobileNumber,request.logo,request.provinceId);
        }
    }
}

