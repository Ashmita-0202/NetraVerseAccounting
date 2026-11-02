using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.DeleteExpense
{
    public  class DeleteExpenseCommandHandler:IRequestHandler<DeleteExpenseCommand,Result<bool>>
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public DeleteExpenseCommandHandler(IExpenseService expenseService,IMapper mapper)
        { 
            _expenseService=expenseService;
            _mapper=mapper;

        }

        public async Task<Result<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteExpense = await _expenseService.Delete(request.id, cancellationToken);
                if (deleteExpense is null)
                {
                    return Result<bool>.Failure("NotFound", "Expense not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
