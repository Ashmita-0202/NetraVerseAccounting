

using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.ReceiptVouchers
{
    public record ReceiptVouchersByQueryResponse(
        string Id,
        string? TransactionDate,
        decimal? TotalAmount,
        TransactionType TransactionMode,
        string paymentId,
        List<ReceiptVouchersRequest> ReceiptVouchersRequests
    );
}
