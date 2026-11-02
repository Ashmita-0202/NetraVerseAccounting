using MediatR;

using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public record AddPaymentsCommand
    (
         string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            string? paymentsNumber,
          string paymentMethodId,
            string? chequeNumber,

string? bankName,
string? accountName,
            List<AddTransactionItemsForPayments> addTransactionItemsForPayments
        ) : IRequest<Result<AddPaymentsResponse>>;
    
}
