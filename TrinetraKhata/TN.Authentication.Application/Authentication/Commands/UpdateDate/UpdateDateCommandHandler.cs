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

namespace TN.Authentication.Application.Authentication.Commands.UpdateDate
{
    public class UpdateDateCommandHandler : IRequestHandler<UpdateDateCommand, Result<UpdateDateResponse>>
    {
       
        private readonly IAuthenticationServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateDateCommand> _validator;

        public UpdateDateCommandHandler(IAuthenticationServices services, IMapper mapper, IValidator<UpdateDateCommand> validator)

        {
            _services = services;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateDateResponse>> Handle(UpdateDateCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateDateResponse>.Failure(errors);
                }


                var updateResult = await _services.ExtendTrialPeriodAsync(request.userId, request.Date);

                if (updateResult.Errors.Any())
                {
                    var errors = string.Join(", ", updateResult.Errors);
                    return Result<UpdateDateResponse>.Failure(errors);
                }

                if (updateResult is null || !updateResult.IsSuccess)
                {
                    return Result<UpdateDateResponse>.Failure("Date update failed.");
                }


                var response = _mapper.Map<UpdateDateResponse>(updateResult.Data);
                return Result<UpdateDateResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the date for UserId {request.userId}", ex);
            }
        }
    }
}
