using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StarWars.EventHub.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Services
{
    public class OrderHostedService : IHostedService, IObserver<OrderRequest>
    {
        private IDisposable _unsubscriber;
        private readonly ILogger<OrderHostedService> _logger;
        private readonly IEventBusService<OrderRequest> _eventBus;
        private readonly ICourierService _courierService;
        private readonly IParcelPickupService _parcelService;
        private readonly INotificationService _notificationService;

        public OrderHostedService(
            ILogger<OrderHostedService> logger,
            IEventBusService<OrderRequest> eventBus,
            ICourierService courierService,
            IParcelPickupService parcelService,
            INotificationService notificationService)
        {
            _logger = logger;
            _eventBus = eventBus;
            _courierService = courierService;
            _parcelService = parcelService;
            _notificationService = notificationService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _unsubscriber = _eventBus.Subscribe(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _unsubscriber.Dispose();
            return Task.CompletedTask;
        }

        public virtual void OnNext(OrderRequest request)
        {
            _ = Process(request);
        }

        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public async Task Process(OrderRequest request)
        { 
            try
            {
                // Get Courier Details 
                var courierDetails = await _courierService.GetDetails(request.BranchId, request.DeliveryType);

                // Create a parcel pickup request
                var parcelPickupRequest = new ParcelPickupRequest {
                    ServiceCode = courierDetails.ServiceCode,
                    PickupDateTime = request.PickupDateTime,
                    Quantity = request.Quantity,
                    Pickup_Address = new PickupAddress { Site_Code = courierDetails.SiteCode },
                    Delivery_Address = request.DeliveryAddress
                };

                // Create a jobNumber
                var jobNumber = await _parcelService.Pickup(parcelPickupRequest, courierDetails.Username);

                // Create a consignment
                var consignment = new Consignment { ConsignmentId = new Random().Next(100000, 999999).ToString() };

                // Send a notification
                await _notificationService.Send(jobNumber, consignment.ConsignmentId);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
            }
        }
    }
}
