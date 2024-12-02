namespace PropertyRentalManagement.Models
{
    public class ApartmentServiceIncludedMappings
    {
        public int ApartmentId { get; set; }
        public Apartment Apartment {  get; set; }
        public int ServiceIncludedId {  get; set; }
        public ServiceIncluded ServiceIncluded {  get; set; }
    }
}
