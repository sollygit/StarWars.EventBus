using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Services
{
    public interface INotificationService
    {
        Task<int> Send(string jobNumber, string consignmentId);
    }

    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            this._logger = logger;
        }

        public async Task<int> Send(string jobNumber, string consignmentId)
        {
            var id = Math.Abs((jobNumber + consignmentId).GetHashCode());

            _logger.LogDebug($"Notification ID: {id}");

            // Simulate an asynchronous operation
            return await Task.Run(async () =>
            {
                await Task.Delay(3000);
                return id;
            });
        }
    }
}
