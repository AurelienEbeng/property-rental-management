namespace PropertyRentalManagement.Models
{
    public class ApartmentOutdoorSpaceMappings
    {
        public int ApartmentId {  get; set; }
        public Apartment Apartment {  get; set; }
        public int OutdoorSpaceId { get; set; }
        public OutdoorSpace OutdoorSpace {  get; set; }
    }
}
