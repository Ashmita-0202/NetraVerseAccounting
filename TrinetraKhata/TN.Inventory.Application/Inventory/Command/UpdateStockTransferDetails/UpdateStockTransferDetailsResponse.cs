using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddStockTransferItems;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails
{
    public record  UpdateStockTransferDetailsResponse
   (
        
            string transferDate,
            string stockCenterNumber,
            string fromStockCenterId,
            string toStockCenterId,
            string narration,
            List<AddStockTransferItemsRequest> addStockTransferItemsRequests

    );
}
