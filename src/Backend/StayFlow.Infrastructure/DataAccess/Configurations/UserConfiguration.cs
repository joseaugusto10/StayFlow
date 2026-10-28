using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(user => user.Name).IsRequired().HasMaxLength(150);
		builder.Property(user => user.Email).IsRequired().HasMaxLength(254);
		builder.HasIndex(user => user.Email).IsUnique();
		builder.Property(user => user.Role).HasConversion<string>().HasMaxLength(20);

		builder.HasOne(user => user.Hotel)
			.WithMany(hotel => hotel.Users)
			.HasForeignKey(user => user.HotelId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
