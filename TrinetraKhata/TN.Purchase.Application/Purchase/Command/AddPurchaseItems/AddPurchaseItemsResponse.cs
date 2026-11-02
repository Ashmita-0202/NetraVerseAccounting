using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public record AddPurchaseItemsResponse
    (
        string id = "",
        string quantity = "",
        string unitId = "",
        string itemId = "",
        double price=0,
        string amount = "",
        string createdBy = "",
        string createdAt = "",
        string updatedBy="",
        string updatedAt="",
        string purchaseDetailsId="",
        string serialNumbers=""
    );
}