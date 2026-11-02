using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.BillSundry.RequestCommandMapper
{
    public static class BillSundryRequestMapper
    {
        public static AddBillSundryCommand ToCommand(this AddBillSundryRequest request)
        {
            return new AddBillSundryCommand(request.name, request.billType, request.defaultValue, request.BillSundryNature, request.isCOGSAffected, request.isCOGPAffected, request.isCOGSTAffected, request.isSalesAccountingAffected, request.isPurchaseAccountingAffected, request.isSalesAmountAdjusted, request.isPurchaseAmountAdjusted, request.CustomerAmountAdjusted, request.VendorAmountAdjusted,request.salesAdjustedLedgerId,request.customerAdjustedLedgerId, request.purchaseAdjustedLedgerId, request.vendorAdjustedLedgerId, request.calculationType, request.calculationTypeOf);
        }
    }
}
