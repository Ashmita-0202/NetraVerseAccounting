using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.AddSalesDetails.RequestCommandMapper
{
    public static class AddSalesDetailsRequestMapper
    {
        public static AddSalesDetailsCommand ToCommand(this AddSalesDetailsRequest request)
        {
            return new AddSalesDetailsCommand
                (
                request.date,
                request.billNumber,
                request.ledgerId,
                request.amountInWords,
                request.discountPercent,
                request.discountAmount,
                request.vatPercent,
                request.vatAmount,
                request.grandTotalAmount,
                 request.paymentId,
                 request.isSales,
                request.StockCenterId,
                request.chequeNumber,
                request.bankName,
                request.accountName,
                request.salesQuotationNumber,
                request.SubTotalAmount,
                request.TaxableAmount,
                request.AmountAfterVat,
                request.BillSundryIds,
                request.salesItems
               
                );
        }
    }
}
