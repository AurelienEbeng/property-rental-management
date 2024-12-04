using Microsoft.AspNetCore.Mvc.Rendering;

namespace PropertyRentalManagement.Requests
{
    public class CreateApartment
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Rooms { get; set; }
        public int Size { get; set; }
        public int FloorNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public Boolean IsVacant { get; set; }

        public List<CheckBoxOption> EquipmentsIncludedCheckboxes { get; set; }
        public List<string> EquipmentsIncluded { get; set; }
        public List<CheckBoxOption> OutdoorSpacesCheckboxes { get; set; }
        public List<string> OutdoorSpaces { get; set; }
        public List<CheckBoxOption> ServicesIncludedCheckboxes { get; set; }
        public List<string> ServicesIncluded { get; set; }
        public List<SelectListItem> BuildingSelectItems { get; set; }

    }
}
