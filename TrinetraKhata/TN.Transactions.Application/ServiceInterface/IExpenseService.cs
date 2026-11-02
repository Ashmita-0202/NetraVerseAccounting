using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddExpense;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllExpense;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Transactions.Application.Transactions.Queries.GetExpenseById;

namespace TN.Transactions.Application.ServiceInterface
{
    public interface IExpenseService
    {
        Task<Result<AddExpenseResponse>> AddExpense(AddExpenseCommand addExpenseCommand);
        Task<Result<PagedResult<GetAllExpenseQueryResponse>>> GetAll(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetExpenseByIdQueryResponse>> GetExpenseById(string id, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetFilterExpenseByDateQueryResponse>>> GetFilterExpense(PaginationRequest paginationRequest,FilterExpenseDto filterExpenseDto);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateExpenseResponse>> UpdateExpense(string id, UpdateExpenseCommand updateExpenseCommand);
    }
}
