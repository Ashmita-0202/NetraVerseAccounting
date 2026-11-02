

using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Inventory.Application.Inventory.Queries.InventoryByItem
{
    public record InventoryByItemQueryResponse
   (
        string itemId,
        string entryDate,
        decimal quantityOut,
        decimal rate,
        decimal quantityIn,
        decimal amountIn,
        decimal amountOut,
        string ledgerId,
        InventoriesType type,
        List<string?> serialNumbers

        );
}
