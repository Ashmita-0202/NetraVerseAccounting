using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockTransferDetails.RequestCommandMapper
{
    public static class AddStockTransferRequestMapper
    {
        public static AddStockTransferCommand ToAddStockTransferCommand(this AddStockTransferDetailsRequest request)
        {
            return new AddStockTransferCommand(
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
