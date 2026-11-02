using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Events.StockUpdated
{
    public class StockUpdatedEventHandler : INotificationHandler<StockUpdatedEvent>
    {
        private readonly ILogger<StockUpdatedEventHandler> _logger;

        public StockUpdatedEventHandler(ILogger<StockUpdatedEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(StockUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stock updated for ItemId: {notification.ItemId}, Quantity: {notification.Quantity}");
            // You can trigger analytics, cache invalidation, etc.
            return Task.CompletedTask;
        }
    }
}
