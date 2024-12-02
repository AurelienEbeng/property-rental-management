namespace PropertyRentalManagement.Models
{
    public class BuildingAmemitiesInBuildingMappings
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int AmenitiesInBuildingId {  get; set; }
        public AmenitiesInBuilding AmenitiesInBuilding { get; set; }
    }
}
