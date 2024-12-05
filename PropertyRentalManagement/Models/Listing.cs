namespace PropertyRentalManagement.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public int ApartmentId {  get; set; }
        public Apartment Apartment { get; set; }
        public int Price {  get; set; }
        public DateTime AvailableFrom {  get; set; }
        public string Email {  get; set; }
        public int CreatorId {  get; set; }
        public User Creator {  get; set; }
        public string Description {  get; set; }
        public string Title {  get; set; }
        public DateTime DateCreated {  get; set; }
        public string LeaseDuration {  get; set; }
        public Boolean ArePetsAllowed { get; set; } = false;
        public Boolean IsSmokingAllowed { get; set; } = false;

    }
}
