using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddTransactions;
using TN.Transactions.Application.Transactions.Command.UpdateTransactions;
using TN.Transactions.Application.Transactions.Queries.GetAllTransactions;
using TN.Transactions.Application.Transactions.Queries.GetTransactionsById;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;

namespace TN.Transactions.Application.ServiceInterface
{
    public interface ITransactionsService
    {
        Task<Result<AddTransactionsResponse>> Add(AddTransactionsCommand addTransactionsCommand);
         Task<Result<PagedResult<GetAllTransactionsByQueryResponse>>> GetAllTransactions(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<GetTransactionsByIdQueryResponse>> GetTransactionsById(string id,CancellationToken cancellationToken=default);

        Task<Result<ReceiptVouchersByQueryResponse>> GetReceiptVouchers(string transactionsDetailsId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateTransactionsResponse>> Update(UpdateTransactionsCommand command,string id);
    }
}
