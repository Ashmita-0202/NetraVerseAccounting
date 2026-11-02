using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddSubledgerGroup
{
    public  class AddSubledgerGroupCommandHandler: IRequestHandler<AddSubledgerGroupCommand, Result<AddSubledgerGroupResponse>>
    {
        private readonly ISubledgerGroupService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSubledgerGroupCommand> _validator;

        public AddSubledgerGroupCommandHandler(ISubledgerGroupService service,IMapper mapper,IValidator<AddSubledgerGroupCommand> validator)
        {
            _service=service;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<AddSubledgerGroupResponse>> Handle(AddSubledgerGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSubledgerGroupResponse>.Failure(errors);
                }

                var addSubLedgerGroup = await _service.Add(request);

                if (addSubLedgerGroup.Errors.Any())
                {
                    var errors = string.Join(", ", addSubLedgerGroup.Errors);
                    return Result<AddSubledgerGroupResponse>.Failure(errors);
                }

                if (addSubLedgerGroup is null || !addSubLedgerGroup.IsSuccess)
                {
                    return Result<AddSubledgerGroupResponse>.Failure(" ");
                }

                var subledgerGroupDisplay = _mapper.Map<AddSubledgerGroupResponse>(addSubLedgerGroup.Data);
                return Result<AddSubledgerGroupResponse>.Success(subledgerGroupDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding SubledgerGroup", ex);


            }
        }
    }
}
