using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;

namespace TN.Authentication.Application.Abstraction
{
    public interface IJwtProviders
    {
        string Generate(ApplicationUser user, IList<string> roles, string institutionIds, IList<string> companyIds, string email, bool isDemoUser);

        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
