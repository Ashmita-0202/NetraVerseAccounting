using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateBillSundry.RequestCommandMapper
{
    public static class UpdateBillSundryRequestMapper
    {
        public static UpdateBillSundryCommand ToCommand(this UpdateBillSundryRequest request, string Id)
        {
            return new UpdateBillSundryCommand
            (
                Id,
              request.name,
              request.billType,
              request.defaultValue,
              request.BillSundryNature,
              request.isCOGSAffected,
              request.isCOGPAffected,
              request.isCOGSTAffected,
              request.isSalesAccountingAffected,
              request.isPurchaseAccountingAffected,
              request.isSalesAmountAdjusted,
              request.isPurchaseAmountAdjusted,
              request.CustomerAmountAdjusted,
              request.VendorAmountAdjusted,
              request.salesAdjustedLedgerId,
              request.customerAdjustedLedgerId,
              request.purchaseAdjustedLedgerId,
              request.vendorAdjustedLedgerId,
              request.calculationType,
              request.calculationTypeOf

            );
        }

    }
}
