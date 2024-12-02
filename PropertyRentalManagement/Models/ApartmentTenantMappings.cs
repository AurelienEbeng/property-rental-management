namespace PropertyRentalManagement.Models
{
    public class ApartmentTenantMappings
    {
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public int UserId { get; set; }
        public User Tenant {  get; set; }
        public DateTime? DateMovedIn { get; set; }
        public DateTime? DateMovedOut { get; set; }
    }
}
