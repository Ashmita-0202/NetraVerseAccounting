using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesItems;

namespace TN.Sales.Application.Sales.Command.AddSalesDetails
{
    public record AddSalesDetailsResponse
   (
         string Id = "",
          string Date = "",
        string BillNumber = "",
        string LedgerId = "",
        string AmountInWords = "",
        decimal DiscountPercent = 0,
        decimal DiscountAmount = 0,
        decimal VatPercent =0,
        decimal VatAmount = 0,
        string CreatedBy = "",
        string CompanyId = "",
        DateTime CreatedAt = default,
        decimal GrandTotalAmount=0,
        string PaymentId = "",
        string StockCenterId = "",

             decimal? SubTotalAmount = 0,
            decimal? TaxableAmount = 0,
            decimal? AmountAfterVat = 0,
        List<AddSalesItemsRequest>? SalesItems = null
        );
}
