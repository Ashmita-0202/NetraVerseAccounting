using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserCommandHandler(IUserServices userServices, IMapper mapper, IValidator<UpdateUserCommand> validator)
        {
            _userServices = userServices;
            _mapper = mapper;
            _validator = validator;


        }

        public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateUserResponse>.Failure(errors);

                }

                var updateUser = await _userServices.Update(request.Id, request);

                if (updateUser.Errors.Any())
                {
                    var errors = string.Join(", ", updateUser.Errors);
                    return Result<UpdateUserResponse>.Failure(errors);
                }

                if (updateUser is null || !updateUser.IsSuccess)
                {
                    return Result<UpdateUserResponse>.Failure("Updates User failed");
                }

                var userDisplay = _mapper.Map<UpdateUserResponse>(updateUser.Data);
                return Result<UpdateUserResponse>.Success(userDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating user by {request.id}", ex);
            }
        }
    }
}
