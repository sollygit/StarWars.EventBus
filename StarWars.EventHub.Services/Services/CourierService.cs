using Microsoft.Extensions.Logging;
using StarWars.EventHub.Models;
using StarWars.EventHub.Services.Settings;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Services
{
    public interface ICourierService
    {
        Task<CourierDetails> GetDetails(int branchId, string deliveryType);
    }

    public class CourierService : ICourierService
    {
        private readonly ILogger<CourierService> _logger;
        private readonly CourierSettings _courierSettings;

        public CourierService(ILogger<CourierService> logger, CourierSettings courierSettings)
        {
            _logger = logger;
            _courierSettings = courierSettings;
        }

        public async Task<CourierDetails> GetDetails(int branchId, string deliveryType)
        {
            _logger.LogDebug("Fetching courier details for BranchId: {BranchId}, DeliveryType: {DeliveryType}", branchId, deliveryType);

            var courierDetails = new CourierDetails { 
                ServiceCode = deliveryType, 
                SiteCode = $"Site-{branchId}-{_courierSettings.CountryCode}", 
                Username = _courierSettings.Username
            };

            // Simulate an asynchronous operation
            return await Task.Run(async () =>
            {
                await Task.Delay(3000);
                return courierDetails;
            });
        }
    }
}
