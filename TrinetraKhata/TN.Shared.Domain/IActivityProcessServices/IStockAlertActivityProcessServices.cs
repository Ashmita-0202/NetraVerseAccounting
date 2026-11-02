using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IActivityProcessServices
{
    public interface IStockAlertActivityProcessServices
    {
        Task ProcessStockExpiryAsync(CancellationToken stoppingToken);
    }
}
