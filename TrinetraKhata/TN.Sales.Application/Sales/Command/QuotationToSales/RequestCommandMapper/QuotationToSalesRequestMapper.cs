using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.QuotationToSales.RequestCommandMapper
{
    public static class QuotationToSalesRequestMapper
    {
        public static QuotationToSalesCommand ToCommand(this QuotationToSalesRequest request)
        {
            return new QuotationToSalesCommand
                (
                request.salesQuotationId,
                request.paymentId,
                request.billNumbers,
                request.chequeNumber,
                request.bankName,
                request.accountName
                );
        }
    }
        
}
