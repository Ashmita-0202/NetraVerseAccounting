using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.DeleteIncome
{
    public  class DeleteIncomeCommandHandler:IRequestHandler<DeleteIncomeCommand, Result<bool>>
    {
        private readonly IIncomeService _incomeService;

        public DeleteIncomeCommandHandler(IIncomeService incomeService)
        {
            _incomeService = incomeService;


        }

        public async Task<Result<bool>> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteIncome = await _incomeService.Delete(request.id, cancellationToken);
                if (deleteIncome is null)
                {
                    return Result<bool>.Failure("NotFound", "Income not Found");
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
