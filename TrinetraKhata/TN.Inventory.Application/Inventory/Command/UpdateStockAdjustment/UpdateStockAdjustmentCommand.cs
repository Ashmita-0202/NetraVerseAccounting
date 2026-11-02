using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Inventory.StockAdjustment;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment
{
    public record  UpdateStockAdjustmentCommand
    (
             string id,
            string itemId,
            double quantityChanged,
            ReasonType reason
        
    ):IRequest<Result<UpdateStockAdjustmentResponse>>;
}
