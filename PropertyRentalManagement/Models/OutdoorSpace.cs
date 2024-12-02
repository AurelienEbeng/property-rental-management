namespace PropertyRentalManagement.Models
{
    public class OutdoorSpace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
    }
}
