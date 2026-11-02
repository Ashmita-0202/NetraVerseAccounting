using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddOrganization
{
    public record AddOrganizationRequest
    (
           
            string name,
            string address,
            string email,
            string phoneNumber,
            string mobileNumber,
            string logo,
            int provinceId
    );
}
