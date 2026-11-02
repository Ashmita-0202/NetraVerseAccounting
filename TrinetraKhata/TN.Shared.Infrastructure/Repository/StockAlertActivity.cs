using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.BackGroundServices;
using TN.Shared.Application.Shared.NotificationDTOs;

namespace TN.Shared.Infrastructure.Repository
{
    public class StockAlertActivity : IStockAlertActivity
    {

        private readonly Channel<StockExpiryDTOs> _channel;

        public StockAlertActivity()
        {
            _channel = Channel.CreateUnbounded<StockExpiryDTOs>(new UnboundedChannelOptions
            {
                SingleReader = false,
                SingleWriter = false
            });

        }
        public async ValueTask AddActivityAsync(StockExpiryDTOs stockExpiryDTOs, CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(stockExpiryDTOs, cancellationToken);
        }

        public async ValueTask<StockExpiryDTOs?> ReadSingleAsync(CancellationToken cancellationToken = default)
        {
            if (await _channel.Reader.WaitToReadAsync(cancellationToken))
            {
                if (_channel.Reader.TryRead(out var item))
                {
                    return item;
                }
            }

            return null; // no data available right now
        }

        public IAsyncEnumerable<StockExpiryDTOs> ReadAllAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
