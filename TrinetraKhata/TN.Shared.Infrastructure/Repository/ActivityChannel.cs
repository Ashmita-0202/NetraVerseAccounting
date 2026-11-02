using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.NotificationDTOs;

namespace TN.Shared.Infrastructure.Repository
{
    public class ActivityChannel : IActivityChannel
    {
        private readonly Channel<StockExpiryDTOs> _channel;

        public ActivityChannel()
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

        public void Complete()
        {
            _channel.Writer.Complete();
        }

        public bool TryRead(out StockExpiryDTOs? activity)
        {
            return _channel.Reader.TryRead(out activity);
        }
    }
}
