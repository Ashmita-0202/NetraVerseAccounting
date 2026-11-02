using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.AllUsers
{
    public record AllUserResponse
        (
            string Id="",
            string FirstName = "",
            string LastName = "",
            string UserName="",
            string Address = "",
            string Email = "",
            //string CompanyName="",
            //string CompanyShortName = "",
            //string ContactNumber="",
            //string ContactPerson = "",
            //string PAN="",
            DateTime? TrialExpiresAt= default
        );

    
}
