using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.FilterUserByDate
{
    public record FilterUserByDateQueryResponse
    (
            string Id,
            string? FirstName,
            string? LastName,
            string? UserName,
            string? Address,
            string? Email,
             string? CompanyId
    );
}
