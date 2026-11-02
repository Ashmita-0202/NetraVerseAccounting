using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.AccountBook.Queries.SalesRegister
{
    public record SalesRegisterQueryResponse
    (
        string date,
        string billNumber,
        string accountId,
        string type,
        decimal totalAmount,
        string companyId,
         decimal? vatAmount,
          decimal? vatPercent,
        List<SalesRegisterItemDTOs> salesRegisterItems
        );

    public record SalesRegisterItemDTOs
        (
        string itemId,
        decimal amount,
        decimal quantity,
        string unitId
       
        );
}
