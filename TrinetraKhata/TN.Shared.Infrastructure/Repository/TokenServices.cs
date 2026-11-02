using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class TokenServices : ITokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenServices(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string GetTokenFromAuthorizationHeader()
        {
            string authorization = _contextAccessor.HttpContext.Request.Headers.Authorization;
            var token = authorization.Split(" ")[1];
            return token;
        }
        public string GetRole()
        {
            var token = GetTokenFromAuthorizationHeader();
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var role = jwt.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            return role.Value;
        }

        public string? GetUserId()
        {
            var token = GetTokenFromAuthorizationHeader();
            if (string.IsNullOrEmpty(token)) return null; // Handle missing token case

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var uid = jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            return uid; // No need for `.Value`, it's already a string
        }


        public string GetUsername()
        {
            var token = GetTokenFromAuthorizationHeader();
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var username = jwt.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            return username.Value;
        }

        public List<string> CompanyId()
        {
            var token = GetTokenFromAuthorizationHeader();
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var companyIdClaims = jwt.Claims.Where(x => x.Type == "CompanyId").Select(x => x.Value).ToList();

            return companyIdClaims ?? new List<string>();
        }

        public string InstitutionId()
        {
            var token = GetTokenFromAuthorizationHeader();
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var institutionClaim = jwt.Claims.FirstOrDefault(x => x.Type == "InstitutionId");

            return institutionClaim?.Value ?? "";
        }

        public async Task<bool> isDemoUser()
        {
            var token = GetTokenFromAuthorizationHeader();
            if (string.IsNullOrEmpty(token)) return false; // Handle missing token case

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var uid = jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            var user = await _userManager.FindByIdAsync(uid);

            return user.IsDemoUser ?? false;
        }

     
    }
}
