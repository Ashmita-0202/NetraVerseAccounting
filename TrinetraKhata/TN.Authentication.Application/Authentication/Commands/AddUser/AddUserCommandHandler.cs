using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddUser
{
    public class AddUserCommandHandler:IRequestHandler<AddUserCommand,Result<AddUserResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddUserCommand> _validator;

        public AddUserCommandHandler(IUserServices userServices,IMapper mapper,IValidator<AddUserCommand> validator) 
        { 
            _userServices=userServices;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<AddUserResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<AddUserResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<AddUserResponse>.Failure("Invalid request");
                }

                var userData = await _userServices.AddUser(request);

                if (userData.Errors.Any())
                {
                    var errors = string.Join(", ", userData.Errors);
                    return Result<AddUserResponse>.Failure(errors);
                }

                if (userData is null || !userData.IsSuccess)
                {
                    return Result<AddUserResponse>.Failure("Add User Failed");
                }
                var userDisplay = _mapper.Map<AddUserResponse>(request);

                if (userDisplay is null)
                {
                    return Result<AddUserResponse>.Failure("Mapping to AddUserResponse Failed");
                }
                return Result<AddUserResponse>.Success(userDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during user creation");
            }


        }
    }
}
