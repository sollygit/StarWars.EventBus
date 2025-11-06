using Microsoft.Extensions.Logging;
using StarWars.EventHub.Models;
using System;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Services
{
    public interface IParcelPickupService
    {
        Task<string> Pickup(ParcelPickupRequest request, string username);
    }

    public class ParcelPickupService : IParcelPickupService
    {
        private static readonly Random _random = new();
        private readonly ILogger<ParcelPickupService> _logger;

        public ParcelPickupService(ILogger<ParcelPickupService> logger)
        {
            _logger = logger;
        }

        public async Task<string> Pickup(ParcelPickupRequest request, string username)
        {
            var timeComponent = (int)(request.PickupDateTime.Ticks % int.MaxValue);
            var randomNumber = _random.Next(0, int.MaxValue);
            var composite = timeComponent.ToString() + randomNumber.ToString() + username;
            var jobNumber = Math.Abs(composite.GetHashCode()).ToString();

            _logger.LogDebug($"Job Number: {jobNumber}");

            // Simulate an asynchronous operation
            return await Task.Run(async () =>
            {
                await Task.Delay(3000);
                return jobNumber;
            });
        }
    }
}
