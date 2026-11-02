using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Events.StockUpdated
{
    public record StockUpdatedEvent
    (
        string ItemId, double Quantity
        ) : INotification;
}
