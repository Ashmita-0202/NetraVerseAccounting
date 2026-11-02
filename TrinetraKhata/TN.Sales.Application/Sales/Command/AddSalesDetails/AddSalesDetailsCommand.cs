using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Command.AddSalesDetails
{
    public record AddSalesDetailsCommand
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
            string paymentId,
            bool isSales,
            string? StockCenterId,
              string? chequeNumber,
            string? bankName,
            string? accountName,
            string? salesQuotationNumber,
                decimal? subTotalAmount ,
                decimal? taxableAmount,
                decimal? amountAfterVat,
            List<BillSundryRequestDTOs> BillSundryIds = null!,
          List<AddSalesItemsRequest> SalesItems = null!

        ) : IRequest<Result<AddSalesDetailsResponse>>;



    public record BillSundryRequestDTOs
(
    string? billSundryIds,
    decimal? rate
    );
}
