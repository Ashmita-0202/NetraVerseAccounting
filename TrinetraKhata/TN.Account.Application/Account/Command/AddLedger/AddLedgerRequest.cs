

namespace TN.Account.Application.Account.Command.AddLedger
{
   public record AddLedgerRequest
    
       (
            string name,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string subledgerGroupId,
            decimal? openingBalance
       );
    
}
