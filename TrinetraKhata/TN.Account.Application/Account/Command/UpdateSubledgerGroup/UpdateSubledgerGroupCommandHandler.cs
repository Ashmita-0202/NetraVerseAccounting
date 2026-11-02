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

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup
{
    public class UpdateSubledgerGroupCommandHandler:IRequestHandler<UpdateSubledgerGroupCommand,Result<UpdateSubledgerGroupResponse>>
    {
        private readonly ISubledgerGroupService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSubledgerGroupCommand> _validator;

        public UpdateSubledgerGroupCommandHandler(ISubledgerGroupService service,IMapper mapper,IValidator<UpdateSubledgerGroupCommand> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateSubledgerGroupResponse>> Handle(UpdateSubledgerGroupCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSubledgerGroupResponse>.Failure(errors);

                }

                var updateSubLedgerGroup = await _service.Update(request.id, request);

                if (updateSubLedgerGroup.Errors.Any())
                {
                    var errors = string.Join(", ", updateSubLedgerGroup.Errors);
                    return Result<UpdateSubledgerGroupResponse>.Failure(errors);
                }

                if (updateSubLedgerGroup is null || !updateSubLedgerGroup.IsSuccess)
                {
                    return Result<UpdateSubledgerGroupResponse>.Failure("Updates SubLedger Group failed");
                }

                var subledgerGroupResponse = _mapper.Map<UpdateSubledgerGroupResponse>(updateSubLedgerGroup.Data);
                return Result<UpdateSubledgerGroupResponse>.Success(subledgerGroupResponse);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating SubLedgerGroup by {request.id}");
            }
        }
    }
}
