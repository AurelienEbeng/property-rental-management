namespace PropertyRentalManagement.Models
{
    public class UserRoleMappings
    {
        public int UserId {  get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
