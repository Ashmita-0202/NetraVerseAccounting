using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.UpdateDate;
using TN.Authentication.Application.Authentication.Queries.RoleById;
using TN.Authentication.Application.Authentication.Queries.RoleByUserId;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.ServiceInterface
{
    public interface IAuthenticationServices
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateRoles(string roles);
        Task<IdentityResult> AssignRoles(ApplicationUser user, string roleName);

        Task<IdentityResult> AssignMultipleRoles(ApplicationUser user, List<string> rolesId);
        Task<bool> CheckRolesAsync(string roleName);
        Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser);
        Task UpdateUserAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPassword(ApplicationUser user, ResetPasswordCommand resetPasswordCommand);
        Task<Result<GetRolesByIdResponse>> GetRolesById(string Id, CancellationToken cancellationToken = default);

        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<bool> IsTrialExpired(ApplicationUser user);

        Task<Result<UpdateDateResponse>> ExtendTrialPeriodAsync(string userId,DateTime date);

    }
}
