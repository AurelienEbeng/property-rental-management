namespace PropertyRentalManagement.Models
{
    public class BuildingOwnerMappings
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
