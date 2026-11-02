using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.UpdateOrganization;

namespace TN.Setup.Application.Setup.Command.UpdateInstitution.RequestCommandMapper
{
    public static class UpdateInstituttionRequestMapper
    {
        public static UpdateInstitutionCommand ToCommand(this UpdateInstitutionRequest request, string institutionId)
        {

            return new UpdateInstitutionCommand
                (
                  institutionId,
                  request.name,
                  request.address,
                  request.email,
                  request.shortName,
                  request.contactNumber,
                  request.contactPerson,
                  request.pan,
                  request.imageUrl,
                  request.isDeleted,
                  request.organizationId

                );
        }
    }
}
