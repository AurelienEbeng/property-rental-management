namespace PropertyRentalManagement.Models
{
    public class BuildingPropertyManagerMappings
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int PropertyManagerId { get; set; }
        public User PropertyManager { get; set; }
    }
}
