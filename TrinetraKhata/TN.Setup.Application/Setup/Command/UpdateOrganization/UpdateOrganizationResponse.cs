using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateOrganization
{
public record  UpdateOrganizationResponse
    (
            string Name,
            string Address,
            string Email,
            string PhoneNumber,
            string MobileNumber,
            string Logo,
            int? ProvinceId


    );
}
