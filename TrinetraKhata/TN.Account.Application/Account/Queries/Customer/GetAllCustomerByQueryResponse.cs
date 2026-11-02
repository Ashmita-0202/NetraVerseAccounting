using TN.Account.Domain.Enum;

namespace TN.Account.Application.Account.Queries.Customer
{
    public record GetAllCustomerByQueryResponse
    (
            string id,
            string fullName,
            string address,
            string contact,
            string? email,
            string? description,
            string? panNo,
            int? maxDueDates,
            decimal? maxCreditLimit,
            bool? isEnabled,
            decimal? openingBalance,
            BalanceType? balanceType,
            bool? isSmsEnabled,
            bool? isEmailEnabled,
            string ledgerId

    );
}
