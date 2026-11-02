using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.NotificationDTOs
{
    public record ActivityDTOs
(
        string UserId,              // ID of the user performing the action
    string Action,              // Action performed: "Login", "CreateInvoice", "DeleteItem"
    DateTime TimeStamp,         // When the action happened
    string Module,              // Main module: "Sales", "Inventory", "Auth", etc.
    string? SubModule = null,   // Optional: "Invoice", "StockItem", "Reports", etc.
    string? Menu = null,        // Optional: UI menu/button name if applicable
    string? Description = null, // Optional: extra details about the action
    string? IpAddress = null,   // Optional: capture user IP for auditing
    string? UserAgent = null    // Optional: browser/device info

    );
}
