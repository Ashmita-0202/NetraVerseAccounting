using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;
using TN.Authentication.Application.Authentication.Commands.DeletePermission;
using TN.Authentication.Application.Authentication.Commands.DeleteRoles;
using TN.Authentication.Application.Authentication.Commands.DeleteUser;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.Authentication.Commands.UpdateDate;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;
using TN.Authentication.Application.Authentication.Queries.AllPermission;
using TN.Authentication.Application.Authentication.Queries.AllRoles;
using TN.Authentication.Application.Authentication.Queries.AllUsers;
using TN.Authentication.Application.Authentication.Queries.AssignableRoles;
using TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId;
using TN.Authentication.Application.Authentication.Queries.PermissionById;
using TN.Authentication.Application.Authentication.Queries.RoleById;
using TN.Authentication.Application.Authentication.Queries.RoleByUserId;
using TN.Authentication.Application.Authentication.Queries.UserById;
using TN.Authentication.Application.Authentication.Queries.UserByRoleId;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;


namespace TN.Authentication.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region UserServices
            CreateMap<RegisterCommand, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // if you want to map password too
            // Map other fields like FirstName, LastName if needed
            .ForMember(dest => dest.FirstName, opt => opt.Ignore()) // You can either map or ignore fields
            .ForMember(dest => dest.LastName, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // if you want to set CreatedAt automatically
            CreateMap<RegisterCommand, RegisterResponse>().ReverseMap();
            CreateMap<ApplicationUser, AllUserResponse>();
            CreateMap<PagedResult<ApplicationUser>,PagedResult<AllUserResponse>>().ReverseMap();
            CreateMap<LoginCommand, LoginResponse>().ReverseMap();
            #endregion

            #region IdentityRole
            CreateMap<AssignRolesResponse, AssignRolesCommand>().ReverseMap();
            CreateMap<AssignRolesCommand, IdentityRole>().ReverseMap();
            CreateMap<IdentityRole, AllRolesResponse>().ReverseMap();
            CreateMap<IdentityRole,GetRolesByIdResponse>().ReverseMap();
            CreateMap<UpdateRoleCommand, IdentityRole>().ReverseMap();
            CreateMap<DeleteRoleCommand, IdentityRole>().ReverseMap();
            CreateMap<PagedResult<IdentityRole>, PagedResult<AllRolesResponse>>().ReverseMap();
            CreateMap<IdentityRole, GetRolesByUserIdQueryResponse>().ReverseMap();
            #endregion

            #region Application User
            CreateMap<GetUserByIdResponse, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserCommand>().ReverseMap();
            CreateMap<ApplicationUser, AddUserCommand>().ReverseMap();
            CreateMap<DeleteUserCommand, ApplicationUser>().ReverseMap();
            CreateMap<AddUserResponse, AddUserCommand>().ReverseMap();
            CreateMap<ApplicationUser, GetUserByRoleIdQueryResponse>().ReverseMap();
            #endregion

            #region PermissionToRoles
            CreateMap<AddPermissionToRolesResponse, AddPermissionToRolesCommand>().ReverseMap();
            CreateMap<AddPermissionToRolesResponse, RolePermission>().ReverseMap();
            #endregion

            #region Permission
            CreateMap<AddPermissionCommand, AddPermissionResponse>().ReverseMap();
            CreateMap<AddPermissionResponse, Permission>().ReverseMap();
            CreateMap<Permission, DeletePermissionCommand>().ReverseMap();
            CreateMap<Permission, AllPermissionResponse>().ReverseMap();
            CreateMap<PagedResult<Permission>, PagedResult<AllPermissionResponse>>().ReverseMap();
            CreateMap<GetPermissionByIdQueryResponse, Permission>().ReverseMap();


            CreateMap<RolePermission, AssignableRolesResponse>().ReverseMap();

            CreateMap<RolePermission, AssignableRolesByPermissionIdResponse>().ReverseMap();


            CreateMap<Permission, UpdatePermissionCommand>().ReverseMap();

            #endregion

            CreateMap<UpdateDateCommand,ApplicationUser>().ReverseMap();
        }
    }
}
