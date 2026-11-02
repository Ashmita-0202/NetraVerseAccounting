

namespace TN.Inventory.Application.Inventory.Queries.GetAllInventory
{
    public record GetAllInventoryByQueryResponse
   (        
             string id,
            string itemId,
            string itemNames,
            decimal remainingQuantity,
            decimal averageAmountIn,
            decimal averageAmountOut,
            DateTime entryDate,
            decimal salesQuantity
    );
}
