

namespace TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate
{
    public record  FilterReceiptDto
    (
         string? ledgerId,
         string? startDate,
         string? endDate
    );
}
