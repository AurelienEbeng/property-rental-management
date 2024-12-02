
namespace PropertyRentalManagement.Models
{
    public class Building
    {
        public int Id { get; set; }
        public int NumberOfFloors { get; set; }
        public int NumberOfApartments { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<AmenitiesInBuilding> AmenitiesInBuilding { get; set; }
        public ICollection<AmenitiesNearby> AmenitiesNearby { get; set; }
        public ICollection<User> Owners { get; set; }
        public ICollection<User> PropertyManagers { get; set; }
    }
}
