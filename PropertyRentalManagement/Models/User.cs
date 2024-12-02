namespace PropertyRentalManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Building> ManagerBuildings { get; set; }
        public ICollection<Building> OwnerBuildings{ get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
