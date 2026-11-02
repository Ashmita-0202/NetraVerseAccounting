

using TN.Shared.Application.Shared.NotificationDTOs;

namespace TN.Shared.Application.ServiceInterface
{
    public interface IActivityChannel
    {
        ValueTask AddActivityAsync(StockExpiryDTOs stockExpiryDTOs, CancellationToken cancellationToken = default);
        ValueTask<StockExpiryDTOs?> ReadSingleAsync(CancellationToken cancellationToken = default);
        IAsyncEnumerable<StockExpiryDTOs> ReadAllAsync(CancellationToken cancellationToken);

        bool TryRead(out StockExpiryDTOs? activity);
        void Complete();
    }
}
