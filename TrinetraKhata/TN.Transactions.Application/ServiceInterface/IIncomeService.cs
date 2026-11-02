using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllIncome;
using TN.Transactions.Application.Transactions.Queries.GetIncomeById;

namespace TN.Transactions.Application.ServiceInterface
{
    public interface IIncomeService
    {
        Task<Result<AddIncomeResponse>> Add(AddIncomeCommand addPaymentsCommand);
        Task<Result<PagedResult<GetAllIncomeQueryResponse>>> GetAll(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetIncomeByIdQueryResponse>> GetIncomeById(string id, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetFilterIncomeByDateQueryResponse>>> GetIncomeFilter(PaginationRequest paginationRequest,FilterIncomeDto filterIncomeDto);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateIncomeResponse>> Update(string id, UpdateIncomeCommand updateIncomeCommand);
    }
}
