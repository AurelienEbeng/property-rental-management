namespace PropertyRentalManagement.Models
{
    public class ApartmentEquipmentsIncludedMappings
    {
        public int ApartmentId {  get; set; }
        public Apartment Apartment {  get; set; }
        public int EquipmentIncludedId {  get; set; }
        public EquipmentIncluded EquipmentIncluded {  get; set; }
    }
}
