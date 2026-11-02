using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.AddOrganization;

namespace TN.Setup.Application.Setup.Command.AddInstitution.RequestCommandMapper
{
    public static class AddInstitutionRequestMapper
    {

        public static AddInstitutionCommand ToCommand(this AddInstitutionRequest request)
        {
            return new AddInstitutionCommand
                (

                 request.name,
                 request.address,
                request.email,
                request.shortName,
                request.contactNumber,
                request.contactPerson,
                request.pan,
                request.imageUrl,
                request.isEnabled,
                request.isDeleted,
                request.organizationId
                );
        }

    }
}

