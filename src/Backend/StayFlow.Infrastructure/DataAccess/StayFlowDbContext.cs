using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess;

internal class StayFlowDbContext : DbContext
{
	public StayFlowDbContext(DbContextOptions<StayFlowDbContext> dbContextOptions) : base(dbContextOptions)
	{ }

	public DbSet<Hotel> Hotels { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Room> Rooms { get; set; }
	public DbSet<Guest> Guests { get; set; }
	public DbSet<Reservation> Reservations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(StayFlowDbContext).Assembly);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{

		configurationBuilder.Properties<decimal>().HaveConversion<string>();
	}
}
