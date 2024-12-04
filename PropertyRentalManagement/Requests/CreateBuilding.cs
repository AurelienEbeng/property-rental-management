
namespace PropertyRentalManagement.Requests
{
    public class CreateBuilding
    {
        public int NumberOfFloors { get; set; }
        public int NumberOfApartments { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public List<CheckBoxOption> AmenitiesInBuildingCheckboxes {  get; set; }
        public List<string> AmenitiesInBuilding { get; set; }
        public List<CheckBoxOption> AmenitiesNearbyCheckboxes { get; set; }
        public List<string> AmenitiesNearby { get; set; }
        public List<CheckBoxOption> OwnersCheckboxes { get; set; }
        public List<string> Owners { get; set; }
        public List<CheckBoxOption> PropertyManagersCheckboxes { get; set; }
        public List<string> PropertyManagers { get; set; }
    }
}
