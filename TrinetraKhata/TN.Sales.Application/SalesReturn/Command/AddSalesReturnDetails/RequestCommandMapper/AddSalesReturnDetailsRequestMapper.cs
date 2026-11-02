

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails.RequestCommandMapper
{
    public static class AddSalesReturnDetailsRequestMapper
    {
        public static AddSalesReturnDetailsCommand ToCommand(this AddSalesReturnDetailsRequest request)
        {
            return new AddSalesReturnDetailsCommand
                (
                    request.salesDetailsId,
                    request.returnDate,
                    request.totalReturnAmount,
                    request.taxAdjustment,
                    request.discount,
                    request.netReturnAmount,
                    request.reason,
                    request.paymentId,
                    request.salesReturnNumber,
                    request.chequeNumber,
                    request.bankName,
                    request.accountName,
                    request.subTotalAmount,
                    request.taxableAmount,
                    request.amountAfterVat,
                    request.BillSundryIds,
                    request.SalesReturnItems
                    

                );
        }
    }
}
