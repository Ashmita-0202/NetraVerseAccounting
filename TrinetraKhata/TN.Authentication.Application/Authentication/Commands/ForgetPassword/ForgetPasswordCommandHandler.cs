using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<ForgetPasswordResponse>>
    {
        private readonly IValidator<ForgetPasswordCommand> _validator;
        private readonly IUserServices _userServices;


        public ForgetPasswordCommandHandler(IValidator<ForgetPasswordCommand> validator, IUserServices userServices)
        {
            _validator = validator;
            _userServices = userServices;
            
        }

       
        public async Task<Result<ForgetPasswordResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors =  string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                    return Result<ForgetPasswordResponse>.Failure(errors);
                }

                var forgetPassword = await _userServices.ForgetPassword(request);
                if (forgetPassword.Errors.Any())
                {
                    var errors = string.Join(", ", forgetPassword.Errors);
                    return Result<ForgetPasswordResponse>.Failure(errors);
                }

                if (forgetPassword is null || !forgetPassword.IsSuccess )
                {
                    return Result<ForgetPasswordResponse>.Failure("Forget password failed");
                }

                var message = forgetPassword.Message;

                return Result<ForgetPasswordResponse>.Success(message);



            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while forgetting password");
            }
        }
    }
}
