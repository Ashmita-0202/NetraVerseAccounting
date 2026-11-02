using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesItems;

namespace TN.Sales.Application.Sales.Command.AddSalesDetails
{
    public record AddSalesDetailsRequest
   (
          string? date,
              string? billNumber,
              string ledgerId,
              string amountInWords,
              decimal? discountPercent,
              decimal? discountAmount,
              decimal? vatPercent,
              decimal? vatAmount,
              decimal grandTotalAmount,
              string? paymentId,
              bool isSales,
              string? StockCenterId,
                string? chequeNumber,
            string? bankName,
            string? accountName,
            string? salesQuotationNumber,
                 decimal? SubTotalAmount,
            decimal? TaxableAmount,
            decimal? AmountAfterVat,
             List<BillSundryRequestDTOs> BillSundryIds = null!,
              List<AddSalesItemsRequest> salesItems=null!
        );
}
