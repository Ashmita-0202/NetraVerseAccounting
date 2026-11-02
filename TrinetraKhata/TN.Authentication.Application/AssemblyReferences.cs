using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;
using TN.Authentication.Application.Authentication.Commands.ForgetPassword;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.Authentication.Commands.ResetPassword;
using TN.Authentication.Application.Authentication.Commands.Roles;
using TN.Authentication.Application.Authentication.Commands.UpdateDate;
using TN.Authentication.Application.Authentication.Commands.UpdatePermission;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;

namespace TN.Authentication.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAuthenticationApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Fluent Validation
            services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddScoped<IValidator<LoginCommand>, LogInCommandValidator>();
            services.AddScoped<IValidator<RoleCommand>, RoleCommandValidators>();
            services.AddScoped<IValidator<AssignRolesCommand>, AssignRolesCommandValidator>();
            services.AddScoped<IValidator<ForgetPasswordCommand>, ForgetPasswordCommandValidator>();
            services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidators>();
            services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            services.AddScoped<IValidator<UpdateRoleCommand>, UpdateRoleCommandValidator>();
            services.AddScoped<IValidator<AddUserCommand>, AddUserCommandValidator>();
            services.AddScoped<IValidator<AddPermissionToRolesCommand>, AddPermissionToRolesCommandValidator>();
            services.AddScoped<IValidator<AddPermissionCommand>, AddPermissionCommandValidator>();
            services.AddScoped<IValidator<UpdatePermissionCommand>, UpdatePermissionCommandValidator>();
            services.AddScoped<IValidator<UpdateDateCommand>, UpdateDateCommandValidator>();
            return services;
        }


    }
}
