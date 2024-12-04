
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Context
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDBContext>();

                context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(new List<Role>()
                    {
                        new Role()
                        {
                            Name = "Admin"
                        },
                        new Role()
                        {
                            Name ="Owner"
                        },
                        new Role()
                        {
                            Name="Tenant"
                        },
                        new Role()
                        {
                            Name="PropertyManager"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<User>()
                    {
                        new User(){ FirstName="Demo", LastName="Admin", Password="12345678", Email="demoadmin@gmail.com"},
                        new User(){ FirstName="Demo", LastName="Property Manager", Password="12345678", Email="demopropertymanager@gmail.com"},
                        new User(){ FirstName="Demo", LastName="Owner", Password="12345678", Email="demoowner@gmail.com"},
                        new User(){ FirstName="Demo", LastName="Tenant", Password="12345678", Email="demotenant@gmail.com"},
                    });
                    context.SaveChanges();
                }

                if (!context.UserRoleMappings.Any())
                {
                    var user1 = context.Users.Where(u => u.FirstName == "Demo" && u.LastName == "Admin" && u.Email == "demoadmin@gmail.com").FirstOrDefault();
                    var role1 = context.Roles.Where(r => r.Name == "Admin").FirstOrDefault();

                    var user2 = context.Users.Where(u => u.FirstName == "Demo" && u.LastName == "Property Manager" && u.Email == "demopropertymanager@gmail.com").FirstOrDefault();
                    var role2 = context.Roles.Where(r => r.Name == "PropertyManager").FirstOrDefault();

                    var user3 = context.Users.Where(u => u.FirstName == "Demo" && u.LastName == "Owner" && u.Email == "demoowner@gmail.com").FirstOrDefault();
                    var role3 = context.Roles.Where(r => r.Name == "Owner").FirstOrDefault();

                    var user4 = context.Users.Where(u => u.FirstName == "Demo" && u.LastName == "Tenant" && u.Email == "demotenant@gmail.com").FirstOrDefault();
                    var role4 = context.Roles.Where(r => r.Name == "Tenant").FirstOrDefault();

                    context.UserRoleMappings.AddRange(new List<UserRoleMappings>()
                    {
                        new UserRoleMappings() { RoleId = role1.Id , UserId=user1.Id},
                        new UserRoleMappings() { RoleId = role2.Id, UserId = user2.Id },
                        new UserRoleMappings() { RoleId = role3.Id, UserId = user3.Id },
                        new UserRoleMappings() { RoleId = role4.Id, UserId = user4.Id }
                    });
                    context.SaveChanges();
                }

                if (!context.ServicesIncluded.Any())
                {
                    context.ServicesIncluded.AddRange(new List<ServiceIncluded>()
                    {
                        new ServiceIncluded(){ Name="Electricity"},
                        new ServiceIncluded(){ Name="Heating"},
                        new ServiceIncluded(){ Name="Water"},
                        new ServiceIncluded(){ Name="Wi-Fi"},
                        new ServiceIncluded(){ Name="Parking"}
                    });
                    context.SaveChanges();
                }

                if (!context.AmenitiesInBuilding.Any())
                {
                    context.AmenitiesInBuilding.AddRange(new List<AmenitiesInBuilding>()
                    {
                        new AmenitiesInBuilding(){ Name="Gym"},
                        new AmenitiesInBuilding(){ Name="Concierge"},
                        new AmenitiesInBuilding(){ Name="24 Hour Security"},
                        new AmenitiesInBuilding(){ Name="Storage Space"},
                        new AmenitiesInBuilding(){ Name="Elevator"},
                        new AmenitiesInBuilding(){ Name="Laundry Room"},
                        new AmenitiesInBuilding(){ Name="Bar"},
                    });
                    context.SaveChanges();
                }

                if (!context.AmenitiesNearby.Any())
                {
                    context.AmenitiesNearby.AddRange(new List<AmenitiesNearby>()
                    {
                        new AmenitiesNearby(){ Name="Supermarket"},
                        new AmenitiesNearby(){ Name="Pharmacy"},
                        new AmenitiesNearby(){ Name="Restaurant"},
                        new AmenitiesNearby(){ Name="Bar"},
                        new AmenitiesNearby(){ Name="Highway Access"},
                        new AmenitiesNearby(){ Name="Public Transportation"},
                        new AmenitiesNearby(){ Name="Hospital"},
                    });
                    context.SaveChanges();
                }

                if (!context.EquipmentsIncluded.Any())
                {
                    context.EquipmentsIncluded.AddRange(new List<EquipmentIncluded>()
                    {
                        new EquipmentIncluded(){ Name="Stove"},
                        new EquipmentIncluded(){ Name="Fireplace"},
                        new EquipmentIncluded(){ Name="Dishwasher"},
                        new EquipmentIncluded(){ Name="Washing Machine"},
                        new EquipmentIncluded(){ Name="Fridge"},
                        new EquipmentIncluded(){ Name="Window Coverings"},
                        new EquipmentIncluded(){ Name="Dryer"},
                        new EquipmentIncluded(){ Name="Ventilator"},
                    });
                    context.SaveChanges();
                }

                if (!context.OutdoorSpaces.Any())
                {
                    context.OutdoorSpaces.AddRange(new List<OutdoorSpace>()
                    {
                        new OutdoorSpace(){ Name="Garden"},
                        new OutdoorSpace(){ Name="Balcony"},
                    });
                    context.SaveChanges();
                }
            }

        }

    }
}
