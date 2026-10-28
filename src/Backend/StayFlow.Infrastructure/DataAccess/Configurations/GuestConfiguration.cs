using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess.Configurations;

internal class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
	public void Configure(EntityTypeBuilder<Guest> builder)
	{
		builder.Property(guest => guest.Name).IsRequired().HasMaxLength(150);
		builder.Property(guest => guest.Document).IsRequired().HasMaxLength(30);
		builder.Property(guest => guest.Email).IsRequired().HasMaxLength(254);
		builder.Property(guest => guest.Phone).IsRequired().HasMaxLength(30);
		builder.Property(guest => guest.Nationality).HasMaxLength(60);
		builder.HasIndex(guest => new { guest.HotelId, guest.Document }).IsUnique();
	}
}
