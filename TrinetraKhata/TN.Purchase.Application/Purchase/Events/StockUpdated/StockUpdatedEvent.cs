using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Events.StockUpdated
{
    public record StockUpdatedEvent
    (
        string ItemId, double Quantity
        ) : INotification;
}
