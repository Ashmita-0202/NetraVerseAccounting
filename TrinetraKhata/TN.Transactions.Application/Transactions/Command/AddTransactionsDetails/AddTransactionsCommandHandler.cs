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

namespace TN.Transactions.Application.Transactions.Command.AddTransactions
{
    public class AddTransactionsCommandHandler:IRequestHandler<AddTransactionsCommand, Result<AddTransactionsResponse>>
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IValidator<AddTransactionsCommand> _validator;
        private readonly IMapper _mapper;

        public AddTransactionsCommandHandler(ITransactionsService transactionsService,IValidator<AddTransactionsCommand> validator,IMapper mapper)
        {
            _transactionsService=transactionsService;
            _validator=validator;
            _mapper=mapper;
        }

        public async Task<Result<AddTransactionsResponse>> Handle(AddTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddTransactionsResponse>.Failure(errors);
                }

                var addTransactions = await _transactionsService.Add(request);

                if (addTransactions.Errors.Any())
                {
                    var errors = string.Join(", ", addTransactions.Errors);
                    return Result<AddTransactionsResponse>.Failure(errors);
                }

                if (addTransactions is null || !addTransactions.IsSuccess)
                {
                    return Result<AddTransactionsResponse>.Failure(" ");
                }

                var addTransactionsResponse = _mapper.Map<AddTransactionsResponse>(addTransactions.Data);
                return Result<AddTransactionsResponse>.Success(addTransactionsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Transactions", ex);


            }
        }
    }
}
