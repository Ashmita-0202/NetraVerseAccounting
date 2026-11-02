using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions
{
    public  class UpdateTransactionsCommandHandler:IRequestHandler<UpdateTransactionsCommand, Result<UpdateTransactionsResponse>>
    {
        private readonly ITransactionsService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateTransactionsCommand> _validator;

        public UpdateTransactionsCommandHandler(ITransactionsService service,IMapper mapper,IValidator<UpdateTransactionsCommand> validator) 
        {
            _service=service;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<UpdateTransactionsResponse>> Handle(UpdateTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateTransactionsResponse>.Failure(errors);

                }

                var updateTransaction = await _service.Update(request, request.id);

                if (updateTransaction.Errors.Any())
                {
                    var errors = string.Join(", ", updateTransaction.Errors);
                    return Result<UpdateTransactionsResponse>.Failure(errors);
                }

                if (updateTransaction is null || !updateTransaction.IsSuccess)
                {
                    return Result<UpdateTransactionsResponse>.Failure("Updates Transactions failed");
                }

                var transactionDisplay = _mapper.Map<UpdateTransactionsResponse>(updateTransaction.Data);
                return Result<UpdateTransactionsResponse>.Success(transactionDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Transactions by {request.id}", ex);
            }
        }
    }
    
}
