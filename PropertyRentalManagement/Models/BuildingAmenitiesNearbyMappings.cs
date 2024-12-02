namespace PropertyRentalManagement.Models
{
    public class BuildingAmenitiesNearbyMappings
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int AmenitiesNearbyId { get; set; }
        public AmenitiesNearby AmenitiesNearby { get; set; }
    }
}
