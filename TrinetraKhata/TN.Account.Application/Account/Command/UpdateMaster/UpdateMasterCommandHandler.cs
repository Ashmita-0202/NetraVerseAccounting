using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateMaster
{
   public class UpdateMasterCommandHandler:IRequestHandler<UpdateMasterCommand,Result<UpdateMasterResponse>>
    {
        private readonly IMasterService _masterService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateMasterCommand> _validator;

        public UpdateMasterCommandHandler(IMasterService masterService,IMapper mapper,IValidator<UpdateMasterCommand> validator) 
        {
            _masterService=masterService;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateMasterResponse>> Handle(UpdateMasterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateMasterResponse>.Failure(errors);

                }

                var updateMaster = await _masterService.Update(request.id, request);

                if (updateMaster.Errors.Any())
                {
                    var errors = string.Join(", ", updateMaster.Errors);
                    return Result<UpdateMasterResponse>.Failure(errors);
                }

                if (updateMaster is null || !updateMaster.IsSuccess)
                {
                    return Result<UpdateMasterResponse>.Failure("Updates Ledger Group failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateMasterResponse>(updateMaster.Data);
                return Result<UpdateMasterResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating master by {request.id}", ex);
            }
        }
    }
}
