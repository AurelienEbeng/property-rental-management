using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmenitiesInBuilding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesInBuilding", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmenitiesNearby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenitiesNearby", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentsIncluded",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentsIncluded", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutdoorSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutdoorSpaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicesIncluded",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesIncluded", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfFloors = table.Column<int>(type: "int", nullable: false),
                    NumberOfApartments = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMappings",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMappings", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    Rooms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    ApartmentNumber = table.Column<int>(type: "int", nullable: false),
                    IsVacant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartments_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BuildingAmenitiesInBuildingMappings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    AmenitiesInBuildingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingAmenitiesInBuildingMappings", x => new { x.AmenitiesInBuildingId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BuildingAmenitiesInBuildingMappings_AmenitiesInBuilding_AmenitiesInBuildingId",
                        column: x => x.AmenitiesInBuildingId,
                        principalTable: "AmenitiesInBuilding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BuildingAmenitiesInBuildingMappings_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BuildingAmenitiesNearbyMappings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    AmenitiesNearbyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingAmenitiesNearbyMappings", x => new { x.AmenitiesNearbyId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BuildingAmenitiesNearbyMappings_AmenitiesNearby_AmenitiesNearbyId",
                        column: x => x.AmenitiesNearbyId,
                        principalTable: "AmenitiesNearby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BuildingAmenitiesNearbyMappings_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BuildingOwnerMappings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingOwnerMappings", x => new { x.OwnerId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BuildingOwnerMappings_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BuildingOwnerMappings_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BuildingPropertyManagerMappings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    PropertyManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingPropertyManagerMappings", x => new { x.PropertyManagerId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BuildingPropertyManagerMappings_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BuildingPropertyManagerMappings_Users_PropertyManagerId",
                        column: x => x.PropertyManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentEquipmentsIncludedMappings",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    EquipmentIncludedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentEquipmentsIncludedMappings", x => new { x.EquipmentIncludedId, x.ApartmentId });
                    table.ForeignKey(
                        name: "FK_ApartmentEquipmentsIncludedMappings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApartmentEquipmentsIncludedMappings_EquipmentsIncluded_EquipmentIncludedId",
                        column: x => x.EquipmentIncludedId,
                        principalTable: "EquipmentsIncluded",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentOutdoorSpaceMappings",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    OutdoorSpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentOutdoorSpaceMappings", x => new { x.OutdoorSpaceId, x.ApartmentId });
                    table.ForeignKey(
                        name: "FK_ApartmentOutdoorSpaceMappings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApartmentOutdoorSpaceMappings_OutdoorSpaces_OutdoorSpaceId",
                        column: x => x.OutdoorSpaceId,
                        principalTable: "OutdoorSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentServiceIncludedMappings",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    ServiceIncludedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentServiceIncludedMappings", x => new { x.ServiceIncludedId, x.ApartmentId });
                    table.ForeignKey(
                        name: "FK_ApartmentServiceIncludedMappings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApartmentServiceIncludedMappings_ServicesIncluded_ServiceIncludedId",
                        column: x => x.ServiceIncludedId,
                        principalTable: "ServicesIncluded",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentTenantMappings",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    DateMovedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateMovedOut = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentTenantMappings", x => new { x.TenantId, x.ApartmentId });
                    table.ForeignKey(
                        name: "FK_ApartmentTenantMappings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApartmentTenantMappings_Users_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    AvailableFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaseDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArePetsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    IsSmokingAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Listings_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentEquipmentsIncludedMappings_ApartmentId",
                table: "ApartmentEquipmentsIncludedMappings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentOutdoorSpaceMappings_ApartmentId",
                table: "ApartmentOutdoorSpaceMappings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentServiceIncludedMappings_ApartmentId",
                table: "ApartmentServiceIncludedMappings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentTenantMappings_ApartmentId",
                table: "ApartmentTenantMappings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingAmenitiesInBuildingMappings_BuildingId",
                table: "BuildingAmenitiesInBuildingMappings",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingAmenitiesNearbyMappings_BuildingId",
                table: "BuildingAmenitiesNearbyMappings",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingOwnerMappings_BuildingId",
                table: "BuildingOwnerMappings",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingPropertyManagerMappings_BuildingId",
                table: "BuildingPropertyManagerMappings",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AddressId",
                table: "Buildings",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ApartmentId",
                table: "Listings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatorId",
                table: "Listings",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_RoleId",
                table: "UserRoleMappings",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentEquipmentsIncludedMappings");

            migrationBuilder.DropTable(
                name: "ApartmentOutdoorSpaceMappings");

            migrationBuilder.DropTable(
                name: "ApartmentServiceIncludedMappings");

            migrationBuilder.DropTable(
                name: "ApartmentTenantMappings");

            migrationBuilder.DropTable(
                name: "BuildingAmenitiesInBuildingMappings");

            migrationBuilder.DropTable(
                name: "BuildingAmenitiesNearbyMappings");

            migrationBuilder.DropTable(
                name: "BuildingOwnerMappings");

            migrationBuilder.DropTable(
                name: "BuildingPropertyManagerMappings");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "EquipmentsIncluded");

            migrationBuilder.DropTable(
                name: "OutdoorSpaces");

            migrationBuilder.DropTable(
                name: "ServicesIncluded");

            migrationBuilder.DropTable(
                name: "AmenitiesInBuilding");

            migrationBuilder.DropTable(
                name: "AmenitiesNearby");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
