using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterCommandHandler(IValidator<RegisterCommand> validator,IAuthenticationServices authenticationServices, IMapper mapper, IUserServices userServices)
        {
            _validator = validator;
            _authenticationServices = authenticationServices;
            _mapper = mapper;
            _userServices = userServices;

        }
        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<RegisterResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<RegisterResponse>.Failure("Invalid request");
                }

                var userData = await _userServices.RegisterUser(request);

                if(userData.Errors.Any())
                {
                    var errors = string.Join(", ", userData.Errors);
                    return Result<RegisterResponse>.Failure(errors);    
                }

                if (userData is null || !userData.IsSuccess)
                {
                    return Result<RegisterResponse>.Failure("User Registration Failed");
                }
                var userDisplay = _mapper.Map<RegisterResponse>(request);

                if (userDisplay is null)
                {
                    return Result<RegisterResponse>.Failure("Mapping to RegisterResponse Failed");
                }
                return Result<RegisterResponse>.Success(userDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during user creation");
            }
            
           
        }
    }
}
