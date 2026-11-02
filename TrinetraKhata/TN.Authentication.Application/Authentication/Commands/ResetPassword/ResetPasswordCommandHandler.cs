using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
    {
        private readonly IValidator<ResetPasswordCommand> _validator;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;


        public ResetPasswordCommandHandler(IValidator<ResetPasswordCommand> validator,IMapper mapper, IUserServices userServices)
        {
            _userServices = userServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<ResetPasswordResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<ResetPasswordResponse>.Failure("Invalid request");
                }

                var resetData = await _userServices.ResetPassword(request);
                if (resetData.Errors.Any())
                {
                    var errors = string.Join(", ", resetData.Errors);
                    return Result<ResetPasswordResponse>.Failure(errors);
                }

                if (resetData is null || !resetData.IsSuccess)
                {
                    return Result<ResetPasswordResponse>.Failure("User Registration Failed");
                }

                var message = resetData.Message;

                return Result<ResetPasswordResponse>.Success(message);

            }
            catch(Exception ex)
            {
                throw new Exception("Something went wrong during password reset");
            }
        }
    }
}
