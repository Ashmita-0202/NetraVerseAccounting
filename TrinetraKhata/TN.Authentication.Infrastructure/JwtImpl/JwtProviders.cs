using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Abstraction;
using TN.Authentication.Domain.Entities;

namespace TN.Authentication.Infrastructure.JwtImpl
{
    public class JwtProviders : IJwtProviders
    {
        private readonly IConfiguration _configuration;

        public JwtProviders(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public string Generate(ApplicationUser user, IList<string> roles, string institutionId, IList<string> companyIds, string email, bool isDemoUser = false)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("InstitutionId",institutionId ?? string.Empty),
                new Claim("email",email ?? string.Empty),


            };
            // Add Company IDs as claims
            if (companyIds is not null && companyIds.Count > 0)
            {
                foreach (var companyId in companyIds)
                {
                    claims.Add(new Claim("CompanyId", companyId));
                }
            }

            #region if we need multiple institution id and company id by single user

            //if (institutionIds is not null && institutionIds.Count > 0)
            //{
            //    foreach (var institutionId in institutionIds)
            //    {
            //        claims.Add(new Claim("InstitutionId", institutionId));
            //    }
            //}

            //// Add Company IDs as claims
            //if (companyIds is not null && companyIds.Count > 0)
            //{
            //    foreach (var companyId in companyIds)
            //    {
            //        claims.Add(new Claim("CompanyId", companyId));
            //    }
            //}

            #endregion

            if (roles is not null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256
                );

            //_ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            int tokenValidityInMinutes = ResolveTokenValidityInMinutes(isDemoUser);


            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                null,
                DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials
                );

            

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;

        }

        private int ResolveTokenValidityInMinutes(bool isDemoUser)
        {
            if (isDemoUser)
                return 5; // Fixed for demo users

            if (int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes))
                return tokenValidityInMinutes;

            // Fallback value if config is missing or invalid
            return 120;
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey= true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false,
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }
    }
}
