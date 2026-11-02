using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.UpdateDate;
using TN.Authentication.Application.Authentication.Queries.RoleById;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TN.Authentication.Infrastructure.ServiceImpl
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationServices(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            
        }

        public async Task<IdentityResult> AssignRoles(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task<bool> CheckRolesAsync(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateRoles(string roles)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roles));
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = _userManager.FindByNameAsync(userName);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }

       

        public async Task<IdentityResult> ResetPassword(ApplicationUser user, ResetPasswordCommand resetPasswordCommand)
        {
            return await _userManager.ResetPasswordAsync(user, resetPasswordCommand.token,resetPasswordCommand.password);
        }

        public Task UpdateUserAsync(ApplicationUser user)
        {
            return _userManager.UpdateAsync(user);
        }

        public async Task<Result<GetRolesByIdResponse>> GetRolesById(string Id, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _unitOfWork.BaseRepository<IdentityRole>().GetByGuIdAsync(Id);

                var roleResponse = _mapper.Map<GetRolesByIdResponse>(role);
                return Result<GetRolesByIdResponse>.Success(roleResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching company by id", ex);

            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {

            try
            {                                
                var role = await _unitOfWork.BaseRepository<IdentityRole>().GetByGuIdAsync(id);
                if (role is null)
                {
                    return Result<bool>.Failure("NotFound", "Company Cannot be Found");
                }

                _unitOfWork.BaseRepository<IdentityRole>().Delete(role);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting role having {id}", ex);
            }

        }

        public async Task<IdentityResult> AssignMultipleRoles(ApplicationUser user, List<string> rolesId)
        {
            var rolesName = await _roleManager.Roles
               .Where(r => rolesId.Contains(r.Id))
               .Select(x => x.Name).ToListAsync() ?? new List<string?>();

            #region Assign roles only from List of rolesId
            ////current rolesName from currently passing list of rolesId
            //var rolesName = await _roleManager.Roles
            //   .Where(r => rolesId.Contains(r.Id))
            //   .Select(x => x.Name).ToListAsync() ?? new List<string?>();


            //var existingRoles = await _userManager.GetRolesAsync(user);

            //// Roles to remove (that are assigned but not in the new list)
            //var rolesToRemove = existingRoles.Except(rolesName).ToList();
            //if (rolesToRemove.Any())
            //{
            //    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            //}

            //var rolesToAdd = rolesName.Except(existingRoles).ToList();
            //if (rolesToAdd.Any())
            //{
            //    return await _userManager.AddToRolesAsync(user, rolesToAdd);
            //}

            #endregion
            return await _userManager.AddToRolesAsync(user, rolesName);
        }

        public Task<bool> IsTrialExpired(ApplicationUser user)
        {
            if (user?.TrialExpiresAt == null)
                return Task.FromResult(false); // No expiry set, assume not expired

            DateTime trialExpiresUtc = TimeZoneInfo.ConvertTimeToUtc(user.TrialExpiresAt.Value, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));
            bool isExpired = DateTime.UtcNow >= trialExpiresUtc;
            return Task.FromResult(isExpired);
        }

        

        public async Task<Result<UpdateDateResponse>> ExtendTrialPeriodAsync(string userId, DateTime date)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result<UpdateDateResponse>.Failure("User not found.");
                }

              
                user.TrialExpiresAt = date;
                //user.TrialExpiresAt ??= DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<UpdateDateResponse>.Failure(errors);
                }

                var response = new UpdateDateResponse(
                    userId,
                    date
                );

                return Result<UpdateDateResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while extending trial period for userId {userId}", ex);
            }
        }
    }
}
