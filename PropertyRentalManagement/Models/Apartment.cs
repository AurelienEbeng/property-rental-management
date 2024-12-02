namespace PropertyRentalManagement.Models
{
    public class Apartment
    {
        public int Id { get; set; }
        public int BuildingId {  get; set; }
        public Building Building {  get; set; }
        public string Rooms { get; set; }
        public int Size{ get; set; }
        public int FloorNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public Boolean IsVacant {  get; set; }

        public ICollection<EquipmentIncluded> EquipmentsIncluded { get; set; }
        public ICollection<OutdoorSpace> OutdoorSpaces { get; set; }
        public ICollection<ServiceIncluded> ServicesIncluded { get; set; }
        public ICollection<User> Tenants { get; set; }
        public ICollection<Listing> Listings { get; set; }
    }
}
