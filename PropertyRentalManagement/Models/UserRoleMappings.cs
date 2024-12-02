namespace PropertyRentalManagement.Models
{
    public class UserRoleMappings
    {
        public int UserId {  get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
