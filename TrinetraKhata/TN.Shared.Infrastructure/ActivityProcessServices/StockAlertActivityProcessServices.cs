using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.NotificationDTOs;
using TN.Shared.Domain.Entities.Notification;
using TN.Shared.Domain.IActivityProcessServices;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;
using TN.Shared.Infrastructure.SignalRHub;

namespace TN.Shared.Infrastructure.ActivityProcessServices
{
    public class StockAlertActivityProcessServices : IStockAlertActivityProcessServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<StockAlertActivityProcessServices> _logger;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityChannel _channel;


        public StockAlertActivityProcessServices(IActivityChannel channel, IHubContext<NotificationHub> hubContext, ILogger<StockAlertActivityProcessServices> logger, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _logger = logger;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _channel = channel;
        }


        public async Task ProcessStockExpiryAsync(CancellationToken stoppingToken)
        {

            var companyId = _tokenService.CompanyId().FirstOrDefault();

            while (_channel.TryRead(out var activity))
            {
                try
                {
                    if (activity.companyId != companyId)
                        continue;

                    // Send SignalR notification
                    await _hubContext.Clients.All.SendAsync(
                        "StockExpiryAlert",
                        $"⚠️ Stock '{activity.itemName}' is expiring soon on {activity.expiredDate}."
                    );

                    // Save notification to DB
                    DateTime currentDate = DateTime.UtcNow;
                    var stockAlertNotification = new StockExpiryNotification()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CompanyId = activity.companyId,
                        ItemName = activity.itemName,
                        ExpiredDate = activity.expiredDate,
                        DaysToExpire = (int)Math.Ceiling(((DateTime)activity.expiredDate - currentDate).TotalDays),
                        Description = $"Stock '{activity.itemName}' is expiring soon on {activity.expiredDate}.",
                        StockCenterId = activity.stockCenterId
                    };

                    await _unitOfWork.BaseRepository<StockExpiryNotification>().AddAsync(stockAlertNotification);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing stock expiry for item {ItemName}", activity.itemName);
                }



            }
        }
    }
}
