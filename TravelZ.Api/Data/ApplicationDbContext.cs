using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelZ.Core.Models;

namespace TravelZ.Api.Data;


public class ApplicationDbContext : IdentityDbContext<User>
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Property> Properties { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options) =>
		options.UseSqlite("DataSource = TravelZ.db; Cache=Shared");

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<Property>()
			.HasOne(p => p.Owner)
			.WithMany(u => u.Properties)
			.HasForeignKey(p => p.OwnerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Entity<Property>().HasData(
			new Property
			{
				Id = 1000,
				Name = "Lake House",
				Description = "A beautiful house by the lake.",
				Address = "123 Lake St",
				Country = "USA",
				Beds = 3,
				Bathrooms = 2,
				TVs = 2,
				Pools = 1,
				PetFriendly = true,
				Wifi = true,
				Parking = true,
				AirConditioning = true,
				OwnerId = null
			},
			new Property
			{
				Id = 1001,
				Name = "Mountain Cabin",
				Description = "Cozy cabin in the mountains.",
				Address = "456 Mountain Rd",
				Country = "USA",
				Beds = 2,
				Bathrooms = 1,
				TVs = 1,
				Pools = 0,
				PetFriendly = false,
				Wifi = false,
				Parking = true,
				AirConditioning = false,
				OwnerId = null
			}
		);
	}
}