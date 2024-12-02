using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AmenitiesInBuilding> AmenitiesInBuilding { get; set; }
        public DbSet<AmenitiesNearby> AmenitiesNearby { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<ApartmentEquipmentsIncludedMappings> ApartmentEquipmentsIncludedMappings { get; set; }
        public DbSet<ApartmentOutdoorSpaceMappings> ApartmentOutdoorSpaceMappings { get; set; }
        public DbSet<ApartmentServiceIncludedMappings> ApartmentServiceIncludedMappings { get; set; }
        public DbSet<ApartmentTenantMappings> ApartmentTenantMappings { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingAmemitiesInBuildingMappings> BuildingAmenitiesInBuildingMappings { get; set; }
        public DbSet<BuildingAmenitiesNearbyMappings> BuildingAmenitiesNearbyMappings { get; set; }
        public DbSet<BuildingOwnerMappings> BuildingOwnerMappings { get; set; }
        public DbSet<BuildingPropertyManagerMappings> BuildingPropertyManagerMappings { get; set; }
        public DbSet<EquipmentIncluded> EquipmentsIncluded { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<OutdoorSpace> OutdoorSpaces { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceIncluded> ServicesIncluded { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoleMappings> UserRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Apartment>()
                .HasMany(apartment => apartment.EquipmentsIncluded)
                .WithMany(equipment => equipment.Apartments)
                .UsingEntity<ApartmentEquipmentsIncludedMappings>(
                    ae => ae.HasOne(prop => prop.EquipmentIncluded).WithMany().HasForeignKey(prop => prop.EquipmentIncludedId),
                    ae => ae.HasOne(prop => prop.Apartment).WithMany().HasForeignKey(prop => prop.ApartmentId),
                    ae =>
                    {
                        ae.HasKey(prop => new { prop.EquipmentIncludedId, prop.ApartmentId });
                    }
                );

            builder.Entity<Apartment>()
                .HasMany(apartment => apartment.OutdoorSpaces)
                .WithMany(outdoorSpace => outdoorSpace.Apartments)
                .UsingEntity<ApartmentOutdoorSpaceMappings>(
                    a => a.HasOne(prop => prop.OutdoorSpace).WithMany().HasForeignKey(prop => prop.OutdoorSpaceId),
                    a => a.HasOne(prop => prop.Apartment).WithMany().HasForeignKey(prop => prop.ApartmentId),
                    a =>
                    {
                        a.HasKey(prop => new { prop.OutdoorSpaceId, prop.ApartmentId });
                    }
                );

            builder.Entity<Apartment>()
                .HasMany(apartment => apartment.ServicesIncluded)
                .WithMany(service => service.Apartments)
                .UsingEntity<ApartmentServiceIncludedMappings>(
                    a => a.HasOne(prop => prop.ServiceIncluded).WithMany().HasForeignKey(prop => prop.ServiceIncludedId),
                    a => a.HasOne(prop => prop.Apartment).WithMany().HasForeignKey(prop => prop.ApartmentId),
                    a =>
                    {
                        a.HasKey(prop => new { prop.ServiceIncludedId, prop.ApartmentId });
                    }
                );

            builder.Entity<Apartment>()
                .HasMany(apartment => apartment.Tenants)
                .WithMany(tenant => tenant.Apartments)
                .UsingEntity<ApartmentTenantMappings>(
                    a => a.HasOne(prop => prop.Tenant).WithMany().HasForeignKey(prop => prop.TenantId),
                    a => a.HasOne(prop => prop.Apartment).WithMany().HasForeignKey(prop => prop.ApartmentId),
                    a =>
                    {
                        a.HasKey(prop => new { prop.TenantId, prop.ApartmentId });
                    }
                );

           
            // one to one between Address and Building
            builder.Entity<Address>()
                .HasOne(e => e.Building)
                .WithOne(e => e.Address)
                .HasForeignKey<Building>(e => e.AddressId)
                .IsRequired();


            builder.Entity<Building>()
               .HasMany(building => building.AmenitiesInBuilding)
               .WithMany(amenities => amenities.Buildings)
               .UsingEntity<BuildingAmemitiesInBuildingMappings>(
                   a => a.HasOne(prop => prop.AmenitiesInBuilding).WithMany().HasForeignKey(prop => prop.AmenitiesInBuildingId),
                   a => a.HasOne(prop => prop.Building).WithMany().HasForeignKey(prop => prop.BuildingId),
                   a =>
                   {
                       a.HasKey(prop => new { prop.AmenitiesInBuildingId, prop.BuildingId });
                   }
               );

            builder.Entity<Building>()
               .HasMany(building => building.AmenitiesNearby)
               .WithMany(amenities => amenities.Buildings)
               .UsingEntity<BuildingAmenitiesNearbyMappings>(
                   a => a.HasOne(prop => prop.AmenitiesNearby).WithMany().HasForeignKey(prop => prop.AmenitiesNearbyId),
                   a => a.HasOne(prop => prop.Building).WithMany().HasForeignKey(prop => prop.BuildingId),
                   a =>
                   {
                       a.HasKey(prop => new { prop.AmenitiesNearbyId, prop.BuildingId });
                   }
               );

            builder.Entity<Building>()
               .HasMany(building => building.Owners)
               .WithMany(owner => owner.OwnerBuildings)
               .UsingEntity<BuildingOwnerMappings>(
                   a => a.HasOne(prop => prop.Owner).WithMany().HasForeignKey(prop => prop.OwnerId),
                   a => a.HasOne(prop => prop.Building).WithMany().HasForeignKey(prop => prop.BuildingId),
                   a =>
                   {
                       a.HasKey(prop => new { prop.OwnerId, prop.BuildingId });
                   }
               );

            builder.Entity<Building>()
               .HasMany(building => building.PropertyManagers)
               .WithMany(propertyManager => propertyManager.ManagerBuildings)
               .UsingEntity<BuildingPropertyManagerMappings>(
                   a => a.HasOne(prop => prop.PropertyManager).WithMany().HasForeignKey(prop => prop.PropertyManagerId),
                   a => a.HasOne(prop => prop.Building).WithMany().HasForeignKey(prop => prop.BuildingId),
                   a =>
                   {
                       a.HasKey(prop => new { prop.PropertyManagerId, prop.BuildingId });
                   }
               );

            
            builder.Entity<Listing>()
                .HasOne(listing => listing.Apartment)
                .WithMany(apartment => apartment.Listings)
                .HasForeignKey(listing => listing.ApartmentId);

            
            builder.Entity<User>()
               .HasMany(user => user.Roles)
               .WithMany(roles => roles.Users)
               .UsingEntity<UserRoleMappings>(
                   a => a.HasOne(prop => prop.Role).WithMany().HasForeignKey(prop => prop.RoleId),
                   a => a.HasOne(prop => prop.User).WithMany().HasForeignKey(prop => prop.UserId),
                   a =>
                   {
                       a.HasKey(prop => new { prop.UserId, prop.RoleId });
                   }
               );

            builder.Entity<Apartment>()
                .HasOne(apt => apt.Building)
                .WithMany(building => building.Apartments)
                .HasForeignKey(apt => apt.BuildingId);
        }
    }
}
