using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IValidator<LoginCommand> _validator;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        

        public LoginCommandHandler(IValidator<LoginCommand> validator,IMapper mapper, IUserServices userServices)
        {
            _userServices = userServices;
            _validator = validator;
            _mapper = mapper;
      
            
        }
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e=>e.ErrorMessage));
                    return Result<LoginResponse>.Failure(errors);
                }

                var logInToken = await _userServices.Login(request);

                if(logInToken.Errors.Any())
                {
                    var errors = string.Join(", ", logInToken.Errors);
                    return Result<LoginResponse>.Failure(errors);
                }

                if(logInToken is null || !logInToken.IsSuccess)
                {
                    return Result<LoginResponse>.Failure("User Login Failed");
                }

                var (token, refreshToken) = logInToken.Data;             


                return Result<LoginResponse>.Success(new LoginResponse(token,refreshToken));

            }
            catch(Exception ex)
            {
                throw new Exception("An error occoured while login ");
            }

            
        }
    }
}
