using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.OrganizationById
{
    public record  GetOrganizationByIdQueryResponse
    (

            string id,
            string name,
            string address,
            string email,
            string phoneNumber,
            string mobileNumber,
            string logo,
            int provinceId

    );
}
