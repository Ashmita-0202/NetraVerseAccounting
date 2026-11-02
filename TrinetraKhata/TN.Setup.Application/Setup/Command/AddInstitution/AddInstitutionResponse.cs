using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddInstitution
{
   public record AddInstitutionResponse
    (
            string name="",
            string address = "",
            string email = "",
            string shortName = "",
            string contactNumber = "",
            string contactPerson="",
            string pan = "",
            string imageUrl="",
            //bool isEnabled,
            string createdDate = "",
            string createdBy="",
            string modifiedDate = "",
            string modifiedBy="",
            bool isDeleted= true,
            string organizationId = ""

       );
}
