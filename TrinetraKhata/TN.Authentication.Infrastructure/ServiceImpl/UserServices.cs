using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Application.Abstraction;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;
using TN.Authentication.Application.Authentication.Commands.ForgetPassword;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;
using TN.Authentication.Application.Authentication.Queries.AllPermission;
using TN.Authentication.Application.Authentication.Queries.AllRoles;
using TN.Authentication.Application.Authentication.Queries.AllUsers;
using TN.Authentication.Application.Authentication.Queries.AssignableRoles;
using TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId;
using TN.Authentication.Application.Authentication.Queries.FilterUserByDate;
using TN.Authentication.Application.Authentication.Queries.PermissionById;
using TN.Authentication.Application.Authentication.Queries.RoleByUserId;
using TN.Authentication.Application.Authentication.Queries.UserById;
using TN.Authentication.Application.Authentication.Queries.UserByRoleId;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Notification;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.ICryptography;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static TN.Authentication.Domain.Entities.CompanySettings;


namespace TN.Authentication.Infrastructure.ServiceImpl
{
    public class UserServices : IUserServices
    {

        private readonly IAuthenticationServices _authenticationServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtProviders _ijwtProviders;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICryptography _cryptography;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IFiscalYearService _fiscalYearService;
        

        public UserServices(ICryptography cryptography,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IAuthenticationServices authenticationServices,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtProviders jwtProviders,
            ITokenService tokenService,
            IDateConvertHelper dateConvertHelper,
            IFiscalYearService fiscalYearService
         
            
            )
        {
            _cryptography = cryptography;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _authenticationServices = authenticationServices;
            _mapper = mapper;
            _ijwtProviders = jwtProviders;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _tokenService = tokenService;
            _dateConvertHelper= dateConvertHelper;
            _fiscalYearService = fiscalYearService;



        }

        public async Task<Result<AddPermissionResponse>> AddPermission(AddPermissionCommand addPermissionCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    //check if permission with the same name already exist
                    var existingPermission = await _unitOfWork.BaseRepository<Permission>()
                        .GetConditionalAsync(x => x.Name == addPermissionCommand.name);

                    if (existingPermission.Any())
                    {
                        return Result<AddPermissionResponse>.Failure("Permission with the same name already exists.");
                    }


                    //check if Roles with the same roleId already exist
                    var existingRoles = await _unitOfWork.BaseRepository<Permission>()
                        .GetConditionalAsync(x => x.RoleId == addPermissionCommand.roleId);

                    if (existingRoles.Any())
                    {
                        return Result<AddPermissionResponse>.Failure("Roles with the same Id already exists.");
                    }



                    var newId = Guid.NewGuid().ToString();

                    var addPermission = new Permission(
                        newId,
                        addPermissionCommand.name,
                        addPermissionCommand.roleId
                        );


                    await _unitOfWork.BaseRepository<Permission>().AddAsync(addPermission);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddPermissionResponse>(addPermission);
                    return Result<AddPermissionResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Something went wrong while creating roles");
                }
            }
        }

        public async Task<Result<AddPermissionToRolesResponse>> AddPermissionToRoles(AddPermissionToRolesCommand addPermissionToRolesCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existingRolePermissions = await _unitOfWork.BaseRepository<RolePermission>()
                        .GetConditionalAsync(x => x.PermissionId == addPermissionToRolesCommand.permissionId);

                    if (existingRolePermissions.Any())
                    {
                        _unitOfWork.BaseRepository<RolePermission>().DeleteRange(existingRolePermissions.ToList());
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var newRolePermissions = addPermissionToRolesCommand.rolesId.Select(roleId => new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = addPermissionToRolesCommand.permissionId
                    }).ToList();

                    if (newRolePermissions.Any())
                    {
                        await _unitOfWork.BaseRepository<RolePermission>().AddRange(newRolePermissions);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    scope.Complete();
                    var userDisplay = _mapper.Map<AddPermissionToRolesResponse>(addPermissionToRolesCommand);
                    return Result<AddPermissionToRolesResponse>.Success(userDisplay);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Something went wrong while creating roles", ex.InnerException);
                }
            }
        }

        public async Task<Result<AddUserResponse>> AddUser(AddUserCommand addUserCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {                
                    try
                    {

                    string newId = Guid.NewGuid().ToString();

                    var emailExists = await _authenticationServices.FindByEmailAsync(addUserCommand.Email);
                    if (emailExists is not null)
                    {
                        return Result<AddUserResponse>.Failure("Conflict", "Email Already Exists");
                    }

                    var userExists = await _authenticationServices.FindByNameAsync(addUserCommand.UserName);
                    if (userExists is not null)
                    {
                        return Result<AddUserResponse>.Failure("Conflict", "User Already Exists");
                    }

                  

                    var user = _mapper.Map<ApplicationUser>(addUserCommand);
                    user.NormalizedUserName = addUserCommand.UserName.ToUpperInvariant();
                    user.NormalizedEmail = addUserCommand.Email.ToUpperInvariant();
                    user.IsDemoUser = false;



                    if (!string.IsNullOrWhiteSpace(addUserCommand.InstitutionId))
                    {
                        var institutionExists = await _unitOfWork.BaseRepository<Institution>()
                            .AnyAsync(i => i.Id == addUserCommand.InstitutionId);

                        if (!institutionExists)
                        {
                            return Result<AddUserResponse>.Failure("NotFound", "Institution does not exist.");
                        }

                        user.InstitutionId = addUserCommand.InstitutionId;
                    }



                    if (addUserCommand?.companyIds != null && addUserCommand.companyIds.Any(id => !string.IsNullOrWhiteSpace(id)))
                    {
                        var validCompanies = await _unitOfWork.BaseRepository<Company>()
                            .GetAllWithIncludeAsync(c => addUserCommand.companyIds.Contains(c.Id));

                        var userCompanies = validCompanies.Select(company => new UserCompany
                        {
                            UserId = user.Id,
                            CompanyId = company.Id
                        }).ToList();

                        user.InstitutionId = validCompanies.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.InstitutionId))?.InstitutionId;

                        if (userCompanies.Any())
                        {
                            await _unitOfWork.BaseRepository<UserCompany>().AddRange(userCompanies);
                        }
                    }





                    // Save User (after either Institution OR Company is assigned)
           
                    await _unitOfWork.BaseRepository<ApplicationUser>().AddAsync(user);

                    var result = await _authenticationServices.CreateUserAsync(user, addUserCommand.Password);
                    await _authenticationServices.AssignMultipleRoles(user, addUserCommand.rolesId);
                    //await _userManager.AddToRolesAsync(user, addUserCommand.rolesId);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var userDisplay = _mapper.Map<AddUserResponse>(addUserCommand);
                    return Result<AddUserResponse>.Success(userDisplay);
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw new Exception("An error occurred while adding user", ex);

                    }

            }
        }

        public async Task<Result<List<AssignableRolesResponse>>> AssignableRoles()
        {
            try
            {
                var user = await _authenticationServices.FindByIdAsync(_tokenService.GetUserId());
                if (user is null)
                {
                    return Result<List<AssignableRolesResponse>>.Failure("NotFound", "User not Found to assign roles");
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles is null || !userRoles.Any())
                {
                    return Result<List<AssignableRolesResponse>>.Failure("Unauthorized", "User has no roles assigned");
                }

                // Fetch assignable roles based on user roles from database

                var userRoleIds = (await _unitOfWork.BaseRepository<IdentityRole>()
                    .GetConditionalAsync(r => userRoles.Contains(r.Name)))
                    .Select(r => r.Id)
                    .ToList();

                var permissionRoles = (await _unitOfWork.BaseRepository<Permission>()
                    .GetConditionalAsync(r => userRoleIds.Contains(r.RoleId)))
                    .Select(r => r.Id)
                    .ToList();



                var assignableRoles = (await _unitOfWork.BaseRepository<RolePermission>()
                    .GetConditionalAsync(x => permissionRoles.Contains(x.PermissionId)))
                    .ToList();

                var assignableRolesResult = _mapper.Map<List<AssignableRolesResponse>>(assignableRoles);

                return Result<List<AssignableRolesResponse>>.Success(assignableRolesResult);
                

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while assigning roles");
            }
        }

        public async Task<Result<List<AssignableRolesByPermissionIdResponse>>> AssignableRolesByPermissionId(string permissionId, CancellationToken cancellationToken)
        {
            try
            {
                var rolePermissions = await _unitOfWork.BaseRepository<RolePermission>()
                    .GetConditionalAsync(x => x.PermissionId == permissionId);

                var journalEntries = rolePermissions != null
                    ? rolePermissions.DistinctBy(x => x.RoleId).ToList()
                    : new List<RolePermission>();

                var resultDisplay = _mapper.Map<List<AssignableRolesByPermissionIdResponse>>(journalEntries);


                return Result<List<AssignableRolesByPermissionIdResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<List<AssignableRolesByPermissionIdResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<AssignRolesResponse>> AssignMultipleRoles(AssignRolesCommand assignRolesCommand)
        {
            try
            {
                var user = await _authenticationServices.FindByIdAsync(assignRolesCommand.UserId);
                if (user is null)
                {
                    return Result<AssignRolesResponse>.Failure("NotFound", "User not Found to assign roles");
                }

                await _authenticationServices.AssignMultipleRoles(user, assignRolesCommand.rolesId);

                var assignRoles = _mapper.Map<AssignRolesResponse>(assignRolesCommand);

                return Result<AssignRolesResponse>.Success(assignRoles);

            }catch (Exception ex)
            {
                throw new Exception("An error occurred while assigning roles");
            }
        }

        public async Task<Result<string>> CreateRoles(string rolename)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var roleExists = await _authenticationServices.CheckRolesAsync(rolename);
                    if(roleExists)
                    {
                        return Result<string>.Failure("Conflict", "Role Already Exists");
                    }

                    var roles = await _authenticationServices.CreateRoles(rolename);
                    scope.Complete();
                    return Result<string>.Success($"Roles {rolename} successfully created");

                }
                catch(Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Something went wrong while creating roles");
                }
            }
        }

        public async Task<Result<bool>> Delete(string userId, CancellationToken cancellationToken)
        {

            try
            {
                var user = await _unitOfWork.BaseRepository<ApplicationUser>().GetByGuIdAsync(userId);
                if (user is null)
                {
                    return Result<bool>.Failure("NotFound", "user Cannot be Found");
                }

                _unitOfWork.BaseRepository<ApplicationUser>().Delete(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting user having {userId}", ex);
            }


        }

        public async Task<Result<bool>> DeletePermission(string id, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _unitOfWork.BaseRepository<Permission>().GetByGuIdAsync(id);
                if (permission is null)
                {
                    return Result<bool>.Failure("NotFound", "permission Cannot be Found");
                }
              
                _unitOfWork.BaseRepository<Permission>().Delete(permission);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting permission having {id}", ex);
            }

        }

        public async Task<Result<ForgetPasswordResponse>> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand)
        {
            try
            {
                var user = await _authenticationServices.FindByEmailAsync(forgetPasswordCommand.email);
                if(user is null)
                {
                    return Result<ForgetPasswordResponse>.Failure("NotFound", "User Not Found");
                }

                var token = await _authenticationServices.GeneratePasswordResetTokenAsync(user);

                var encodedToken = await _cryptography.Base64UrlEncoder(token);

                var request = _contextAccessor.HttpContext?.Request;

                var scheme = request.Scheme; // "http" or "https"
                var host = request.Host; // e.g., "yourdomain.com"



                var callBackUrl = $"{scheme}://{host}/api/Authentication/ResetPassword?token={encodedToken}&email={forgetPasswordCommand.email}";

                if(callBackUrl is null)
                {
                    return Result<ForgetPasswordResponse>.Failure("NotFound", "Failed to generate the reset password link");
                }

                var otpServices = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IOtpServices>();

                var emailServices = _contextAccessor.HttpContext.RequestServices.GetService<IEmailServices>();

                await emailServices.SendEmailAsync(forgetPasswordCommand.email!, "Reset Password", $"Reset your password by <a href='{callBackUrl}'>Click Here</a>.");

                return Result<ForgetPasswordResponse>.Success("Successfully sent Link url");


            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while forgeting password");
            }
        }

        public async Task<Result<PagedResult<AllPermissionResponse>>> GetAllPermission(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _unitOfWork.BaseRepository<Permission>().GetAllAsyncWithPagination();

                var rolesPagedResult = await permission.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var rolesResponse = _mapper.Map<PagedResult<AllPermissionResponse>>(rolesPagedResult.Data);
                return Result<PagedResult<AllPermissionResponse>>.Success(rolesResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch all the permission");
            }
        }

        public async Task<Result<PagedResult<AllRolesResponse>>> GetAllRoles(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _unitOfWork.BaseRepository<IdentityRole>().GetAllAsyncWithPagination();


                var user = await _authenticationServices.FindByIdAsync(_tokenService.GetUserId());
                if (user is null)
                {
                    return Result<PagedResult<AllRolesResponse>>.Failure("NotFound", "User not Found to assign roles");
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles is null || !userRoles.Any())
                {
                    return Result<PagedResult<AllRolesResponse>>.Failure("Unauthorized", "User has no roles assigned");
                }

                var isAdmin = userRoles.Any(r => r == "superadmin" || r == "developeruser");


                var filteredRoleIds = (await _unitOfWork.BaseRepository<IdentityRole>()
                    .GetConditionalAsync(r => userRoles.Contains(r.Name)))
                    .Select(r => r.Id)
                    .ToList();

                var rolesFromPermission = (await _unitOfWork.BaseRepository<Permission>()
                    .GetConditionalAsync(r => filteredRoleIds.Contains(r.RoleId)))
                    .Select(r => r.Id)
                    .ToList();

                var rolesFromRolePermission = (await _unitOfWork.BaseRepository<RolePermission>()
                    .GetConditionalAsync(r => rolesFromPermission.Contains(r.PermissionId)))
                    .Select(r => r.RoleId)
                    .ToList();


                var displayUserBasedRoles = roles.Where(r => rolesFromRolePermission.Contains(r.Id));


                var rolesPagedResult = await (isAdmin
                    ? roles.Where(x=>x.Name!= "superadmin" && x.Name != "developeruser").AsNoTracking()
                    : displayUserBasedRoles.AsNoTracking().DefaultIfEmpty()) 
                    .ToPagedResultAsync(
                        paginationRequest.pageIndex,
                        paginationRequest.pageSize,
                        paginationRequest.IsPagination
                    );

                var rolesResponse = _mapper.Map<PagedResult<AllRolesResponse>>(rolesPagedResult.Data);
                return Result<PagedResult<AllRolesResponse>>.Success(rolesResponse);

            }
            catch(Exception ex)
            {
                throw new Exception("Failed to fetch all the roles");
            }
        }

        public async Task<Result<PagedResult<AllUserResponse>>> GetAllUsers(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var userRoles = _tokenService.GetRole();
                var isSuperAdmin = userRoles == Role.SuperAdmin;

                var companyIds = await _unitOfWork.BaseRepository<Company>()
            .GetConditionalFilterType(
                x => x.InstitutionId == institutionId,
                query => query.Select(c => c.Id)
                );

               

                IQueryable<ApplicationUser> users =  await _unitOfWork.BaseRepository<ApplicationUser>().GetAllAsyncWithPagination();


                var userIdsByCompany = await _unitOfWork.BaseRepository<UserCompany>()
                    .GetConditionalFilterType(x => x.CompanyId == companyId, query => query.Select(c => c.UserId)); 

                var FilteredUser = isSuperAdmin ? users : await _unitOfWork.BaseRepository<ApplicationUser>()
                        .FindBy(x => userIdsByCompany.Contains(x.Id));



                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    FilteredUser = await _unitOfWork.BaseRepository<ApplicationUser>()
                        .FindBy(x => x.InstitutionId == institutionId);
                }


                var filterUsers = FilteredUser.Where(r => r.UserName != "superadminuser" && r.UserName != "developeruser");
                var userPagedResult = await filterUsers.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var alluserResponse = _mapper.Map<PagedResult<AllUserResponse>>(userPagedResult.Data);
                return Result<PagedResult<AllUserResponse>>.Success(alluserResponse);

            }catch(Exception ex)
            {
                throw new Exception("Failed to fetch all the users");
            }
        }

        public async Task<Result<GetPermissionByIdQueryResponse>> GetPermissionById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var permission = await _unitOfWork.BaseRepository<Permission>().GetByGuIdAsync(id);

                var permissionResponse = _mapper.Map<GetPermissionByIdQueryResponse>(permission);
                return Result<GetPermissionByIdQueryResponse>.Success(permissionResponse);

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch Permission By {id}");
            }
        }

        public async Task<Result<List<GetRolesByUserIdQueryResponse>>> GetRolesByUserId(string userId, CancellationToken cancellationToken = default)
        {
        
            try
            {
                var role = await _unitOfWork.BaseRepository<IdentityUserRole<string>>().GetConditionalAsync(x => x.UserId == userId);

                var rolesId = role.Select(x => x.RoleId);

                var rolesName = await _unitOfWork.BaseRepository<IdentityRole>()
                    .GetConditionalAsync(x => rolesId.Contains(x.Id));
                   
                if (role is null)
                {
                    return Result<List<GetRolesByUserIdQueryResponse>>.Failure("NotFound", "Roles are not found");
                }
                var roleResponse = _mapper.Map<List<GetRolesByUserIdQueryResponse>>(rolesName);

                return Result<List<GetRolesByUserIdQueryResponse>>.Success(roleResponse);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching role by user id", ex);

            }
        
        }

        public async Task<Result<GetUserByIdResponse>> GetUserById(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _unitOfWork.BaseRepository<ApplicationUser>().GetByGuIdAsync(userId);

                var userResponse = _mapper.Map<GetUserByIdResponse>(user);
                return Result<GetUserByIdResponse>.Success(userResponse);

            }catch(Exception ex )
            {
                throw new Exception($"Failed to fetch user By {userId}");
            }
        }

        public async Task<Result<List<GetUserByRoleIdQueryResponse>>> GetUserByRoleId(string roleId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(roleId))
                {
                   
                    return Result<List<GetUserByRoleIdQueryResponse>>.Failure("NotFound", "RoleId cannot be null");
                }
                var userRoles = await _unitOfWork.BaseRepository<IdentityUserRole<string>>()
                    .GetConditionalAsync(x => x.RoleId == roleId);

                if (userRoles == null || !userRoles.Any())
                {
                    return Result<List<GetUserByRoleIdQueryResponse>>.Failure("No Users Found");
               }
                 var userIds = userRoles.Select(x => x.UserId).ToList();
                               
                var users = await _unitOfWork.BaseRepository<ApplicationUser>()
                    .GetConditionalAsync(x => userIds.Contains(x.Id));

                if (users == null || !users.Any())
                {
                    return Result<List<GetUserByRoleIdQueryResponse>>.Failure("No users associated with this role");
                }
          
                var userResponses = _mapper.Map<List<GetUserByRoleIdQueryResponse>>(users);
                return Result<List<GetUserByRoleIdQueryResponse>>.Success(userResponses);

            }
            catch (Exception ex)
            {
             
                throw new Exception($"An error occurred while fetching users for Role ID {roleId}: {ex.Message}", ex);

            }
        }

        public async Task<Result<IEnumerable<FilterUserByDateQueryResponse>>> GetUserFilter(FilterUserDTOs filterUserDTOs, CancellationToken cancellationToken)
        {
            try
            {

                var filterUser = await _unitOfWork.BaseRepository<ApplicationUser>().GetConditionalAsync(
                     x =>
            
                     (string.IsNullOrEmpty(filterUserDTOs.userName) || x.UserName.Contains(filterUserDTOs.userName)) &&
                        (string.IsNullOrEmpty(filterUserDTOs.companyId) ||
                   x.UserCompany.Any(uc => uc.CompanyId == filterUserDTOs.companyId)) &&
                     (string.IsNullOrEmpty(filterUserDTOs.email) || x.Email.Contains(filterUserDTOs.email)),
                    queryModifier: q => q
                        .Include(u => u.UserCompany)
                            .ThenInclude(uc => uc.Company)

                );

                var userResponse = filterUser.Select(p => new FilterUserByDateQueryResponse(
                                 p.Id,
                                 p.FirstName,
                                 p.LastName,
                                 p.UserName,
                                 p.Address,
                                 p.Email,
                               p.UserCompany.FirstOrDefault()?.CompanyId ??""
                               ));

                return Result<IEnumerable<FilterUserByDateQueryResponse>>.Success(userResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching User by date: {ex.Message}");
            }
        }

        public async Task<Result<LoginResponse>> Login(LoginCommand loginCommand)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (loginCommand is null)
                    {
                        return Result<LoginResponse>.Failure("Invalid request");
                    }

                    var user = await _authenticationServices.FindByEmailAsync(loginCommand.username);
                    if (user is null)
                    {
                        return Result<LoginResponse>.Failure("Unauthorized", "Invalid Username");
                    }

                    if (!await _authenticationServices.CheckPasswordAsync(user, loginCommand.password))
                    {
                        return Result<LoginResponse>.Failure("Unauthorized", "Invalid Password");
                    }

                    var roles = await _authenticationServices.GetRolesAsync(user);
                    if (roles is null)
                    {
                        return Result<LoginResponse>.Failure("NotFound", "Roles are not found");
                    }

                    //List<string> InstitutionIds = (await _unitOfWork.BaseRepository<UserInstitution>()
                    //    .GetConditionalAsync(x => x.UserId == user.Id))?
                    //    .Select(x => x.InstitutionId)
                    //    .ToList() ?? new List<string>();


                    string InstitutionIds = user?.InstitutionId?.ToString() ?? string.Empty;


                    List<string> CompanyIds = (await _unitOfWork.BaseRepository<UserCompany>()
                        .GetConditionalAsync(x => x.UserId == user.Id))?
                        .Select(x => x.CompanyId)
                        .ToList() ?? new List<string>();




                    string token = "";

                    if (!roles.Contains(Role.SuperAdmin) && !roles.Contains(Role.DeveloperUser))
                    {
                        bool? isExpired = await _authenticationServices.IsTrialExpired(user);

                        if (isExpired == true && user.IsDemoUser == true && !roles.Contains(Role.DemoExpiryRole))
                        {
                            // Trial expired and not yet handled
                            await _userManager.RemoveFromRolesAsync(user, roles);
                            await _authenticationServices.AssignRoles(user, Role.DemoExpiryRole);

                            token = _ijwtProviders.Generate(user, new List<string> { Role.DemoExpiryRole }, InstitutionIds, CompanyIds, user.Email, true);
                        }
                        else
                        {
                            // Either not expired or already handled
                            token = _ijwtProviders.Generate(user, roles, InstitutionIds, CompanyIds, user.Email, isExpired ?? false);
                        }
                    }
                    else
                    {
                        // SuperAdmin or DeveloperUser – skip trial check
                        token = _ijwtProviders.Generate(user, roles, InstitutionIds, CompanyIds, user.Email, false);
                    }


                    string refreshToken = _ijwtProviders.GenerateRefreshToken();
                    user.RefreshToken = refreshToken;

                    _ = int.TryParse(_configuration["Jwt:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);




                    await _authenticationServices.UpdateUserAsync(user);
                    scope.Complete();

                    var logInToken = new LoginResponse(token, refreshToken);
                    return Result<LoginResponse>.Success(logInToken);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("something went wrong while login");
                }
            }
        }

        public async Task<Result<RegisterResponse>> RegisterUser(RegisterCommand registerCommand)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var userExists = await _authenticationServices.FindByNameAsync(registerCommand.Username);
                    if(userExists is not null)
                    {
                        return Result<RegisterResponse>.Failure("Conflict", "User Already Exists");
                    }

                    var emailExists = await _authenticationServices.FindByEmailAsync(registerCommand.Email);
                    if(emailExists is not null)
                    {
                        return Result<RegisterResponse>.Failure("Conflict", "Email Already Exists");
                    }

                    var user = _mapper.Map<ApplicationUser>(registerCommand);
                    user.IsDemoUser = true;
                    user.TrialStartedAt = DateTime.Now;
                    user.TrialExpiresAt = DateTime.Now.AddMinutes(5);

                    var result = await _authenticationServices.CreateUserAsync(user, registerCommand.Password);
         



                    var companyId = Guid.NewGuid().ToString();
                    var company = new Company
                    (
                       companyId,
                       registerCommand.CompanyName,
                       registerCommand.Address,
                       registerCommand.CompanyShortName,
                       registerCommand.Email,
                       registerCommand.ContactNumber,
                       registerCommand.ContactPerson,
                       registerCommand.PAN,
                       "",
                       true,
                       "b4e3de2f-9781-4075-9676-8e2a98c3e70e",
                       DateTime.MinValue,
                       "71824f27-1c97-460a-b583-328d7f887ae5",   
                       DateTime.MinValue,
                       "71824f27-1c97-460a-b583-328d7f887ae5",
                       false,
                      Company.BillNumberGenerationType.Automatic,
                      Company.BillNumberGenerationType.Automatic   
                       
                    );
                    await _unitOfWork.BaseRepository<Company>().AddAsync(company);
                    
                    
                    var assignUserToCompany = new UserCompany()
                    {
                        UserId = user.Id,
                        CompanyId = companyId
                    };

                    await _unitOfWork.BaseRepository<UserCompany>().AddAsync(assignUserToCompany);

                    await _authenticationServices.AssignRoles(user, Role.DemoUserRole);

                    var (FiscalYearId,FyName) = await _fiscalYearService.GetFiscalYearIdForDateAsync(DateTime.Now);

                    var companySettingsId = Guid.NewGuid().ToString();
                    var companySettings = new CompanySettings
                        (
                       companySettingsId,
                        true,
                        true,
                        CompanySettings.PurchaseReferencesType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        true,
                        CompanySettings.JournalReferencesType.Automatic,
                        CompanySettings.InventoryMethodType.FIFO,
                        companyId,
                        //"3da81047-fab3-4f8f-ae67-26f1c1bd03d8",
                        FiscalYearId,
                        false,
                        TransactionNumberType.Automatic,
                        TransactionNumberType.Automatic,
                        TransactionNumberType.Automatic,
                        TransactionNumberType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        PurchaseSalesReturnNumberType.Automatic,
                        PurchaseSalesReturnNumberType.Automatic,
                        PurchaseSalesQuotationNumberType.Automatic,
                        PurchaseSalesQuotationNumberType.Automatic,
                        user.Id
                        );

                    await _unitOfWork.BaseRepository<CompanySettings>().AddAsync(companySettings);

                    //var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>().GetByGuIdAsync(FiscalYear.fiscalYearId);


                    var companySettingsFiscalYear = new CompanySettingsFiscalYear
                        (
                        Guid.NewGuid().ToString(),
                        companySettingsId,
                        FiscalYearId,
                        false,
                        "",
                        DateTime.UtcNow,
                        true,
                        FyName,
                        companyId,
                        true
                        );
                    await _unitOfWork.BaseRepository<CompanySettingsFiscalYear>().AddAsync(companySettingsFiscalYear);






                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var userDisplay = _mapper.Map<RegisterResponse>(registerCommand);
                    return Result<RegisterResponse>.Success(userDisplay);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Something went wrong during user creation");
                }

            }
        }

        public async Task<Result<ResetPasswordResponse>> ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            try
            {
                var decodeByte = WebEncoders.Base64UrlDecode(resetPasswordCommand.token);
                var decodedToken = Encoding.UTF8.GetString(decodeByte);


                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(resetPasswordCommand.password);

                if(jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return Result<ResetPasswordResponse>.Failure("Unauthorized", "Token has expired.");
                }

                var user = await _authenticationServices.FindByEmailAsync(resetPasswordCommand.email);
                if (user is null)
                {
                    return Result<ResetPasswordResponse>.Failure("NotFound", "User maybe not registered yet!");
                }

                var result = await _authenticationServices.ResetPassword(user, resetPasswordCommand);

                if(!result.Succeeded)
                {
                    return Result<ResetPasswordResponse>.Failure("NotFound", "User send wrong credentials");
                }

                return Result<ResetPasswordResponse>.Success("Password reset successfully!");

            }
            catch(Exception ex)
            {
                throw new Exception("Something went wrong during reset password");
            }
        }

        public async Task<Result<UpdateUserResponse>> Update(string userId, UpdateUserCommand updateUserCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userToBeUpdated = await _unitOfWork.BaseRepository<ApplicationUser>().GetByGuIdAsync(userId);
                    if (userToBeUpdated is null)
                    {
                        return Result<UpdateUserResponse>.Failure("NotFound", "User are not Found");
                    }

                    _mapper.Map(updateUserCommand, userToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateUserResponse
                        (
                           updateUserCommand.FirstName,
                           updateUserCommand.LastName,
                           updateUserCommand.LastName,
                           updateUserCommand.Address,
                           updateUserCommand.Email
                           


                        );

                    return Result<UpdateUserResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating user", ex);
                }
            }

        }

        public async  Task<Result<UpdateRoleResponse>> Update(string id, UpdateRoleCommand updateRoleCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var roleToBeUpdated = await _unitOfWork.BaseRepository<IdentityRole>().GetByGuIdAsync(id);
                    if (roleToBeUpdated is null)
                    {
                        return Result<UpdateRoleResponse>.Failure("NotFound", "Role are not Found");
                    }

                    _mapper.Map(updateRoleCommand, roleToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateRoleResponse
                        (
                            id,
                           updateRoleCommand.Name
                          


                        );

                    return Result<UpdateRoleResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating role", ex);
                }
            }
        }

        public async Task<Result<UpdatePermissionResponse>> Update(string id, UpdatePermissionCommand updatePermissionCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var permissionToBeUpdated = await _unitOfWork.BaseRepository<Permission>().GetByGuIdAsync(id);
                    if (permissionToBeUpdated is null)
                    {
                        return Result<UpdatePermissionResponse>.Failure("NotFound", "Permission are not Found");
                    }

                    _mapper.Map(updatePermissionCommand, permissionToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdatePermissionResponse
                        (
                            id,
                           updatePermissionCommand.name,
                           updatePermissionCommand.roleId


                        );

                    return Result<UpdatePermissionResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating permission", ex);
                }
            }
        }
    }
}
