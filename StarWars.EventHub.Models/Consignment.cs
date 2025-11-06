namespace StarWars.EventHub.Models
{
    public class Consignment
    {
        public string ConsignmentId { get; set; }
        

        public Consignment()
        {
            ConsignmentId = "NONE";
        }

        public Consignment(string consignmentId)
        {
            ConsignmentId = consignmentId;
        }
    }
}
