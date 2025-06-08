using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelZ.Core.Models;

namespace TravelZ.Api.Data;


public class ApplicationDbContext : IdentityDbContext<User>
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options) => 
	options.UseSqlite("DataSource = TravelZ.db; Cache=Shared");
}