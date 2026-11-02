
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllPayments;
using TN.Transactions.Application.Transactions.Queries.GetPaymentById;

namespace TN.Transactions.Application.ServiceInterface
{
    public  interface IPaymentsServices
    {
        Task<Result<UpdatePaymentResponse>> Update(string id,UpdatePaymentCommand updatePaymentCommand);
        Task<Result<PagedResult<GetFilterPaymentQueryResponse>>> GetPaymentFilter(PaginationRequest paginationRequest,FilterPaymentDto filterPaymentDto);
        Task<Result<AddPaymentsResponse>> Add(AddPaymentsCommand addPaymentsCommand);
        Task<Result<PagedResult<GetAllPaymentsQueryResposne>>> GetAll(PaginationRequest paginationRequest, string? ledgerId, CancellationToken cancellationToken=default);
    
        Task<Result<GetPaymentByIdQueryResponse>> GetPaymentById(string id,CancellationToken cancellationToken=default);
    }
}
