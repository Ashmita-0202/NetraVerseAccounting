using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId
{
    public record GetInstitutionByOrganizationIdResponse
   (
            string id,
            string name,
            string address,
            string email,
            string shortName,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
            string organizationId

    );
}
