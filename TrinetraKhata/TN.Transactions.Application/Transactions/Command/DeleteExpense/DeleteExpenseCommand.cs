using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Transactions.Application.Transactions.Command.DeleteExpense
{
    public record  DeleteExpenseCommand
   (string id):IRequest<Result<bool>>;
}
