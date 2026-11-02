using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Inventory.Application.Inventory.Command.AddStockTransferItems;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails
{
    public record  UpdateStockTransferDetailsCommand
    (

             string id,
            string transferDate,
            string stockCenterNumber,
            string fromStockCenterId,
            string toStockCenterId,
            string narration,
            List<AddStockTransferItemsRequest> addStockTransferItemsRequests


    ):IRequest<Result<UpdateStockTransferDetailsResponse>>;
}
