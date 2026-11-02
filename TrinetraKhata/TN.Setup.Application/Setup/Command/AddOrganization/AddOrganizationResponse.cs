using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddOrganization
{
  public record AddOrganizationResponse
    (
           
            string name,
            string address,
            string phoneNumber,
            string mobileNumber,
            string logo,
            int provinceId
   );
}
