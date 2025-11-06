using System;

namespace StarWars.EventHub.Models
{
    public class OrderRequest
    {
        public int BranchId { get; set; }
        public string DeliveryType { get; set; }
        public string OrderNumber { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int Quantity { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
    }
}
