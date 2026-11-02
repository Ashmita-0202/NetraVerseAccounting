
namespace TN.Account.Application.Account.Queries.AccountReceivable
{
    public record AccountReceivableQueryResponse
    (
         string ledgerId,
        decimal balance,
        string subledgerGroupId
        );
}
