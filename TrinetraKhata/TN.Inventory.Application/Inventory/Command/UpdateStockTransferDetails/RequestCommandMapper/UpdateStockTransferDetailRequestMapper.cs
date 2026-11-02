using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails.RequestCommandMapper
{
    public static class UpdateStockTransferDetailRequestMapper
    {
        public static UpdateStockTransferDetailsCommand ToCommand(this UpdateStockTransferDetailsRequest request, string id)
        {
            return new UpdateStockTransferDetailsCommand
                (
                    id,
                    request.transferDate,
                    request.stockCenterNumber,
                    request.fromStockCenterId,
                    request.toStockCenterId,
                    request.narration,
                    request.addStockTransferItemsRequests



                );
        }
    }
}
