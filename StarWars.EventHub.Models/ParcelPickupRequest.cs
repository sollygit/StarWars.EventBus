using System;

namespace StarWars.EventHub.Models
{
    public class ParcelPickupRequest
    {
        public string ServiceCode { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int Quantity { get; set; }
        public PickupAddress Pickup_Address { get; set; }
        public DeliveryAddress Delivery_Address { get; set; }
    }
}
