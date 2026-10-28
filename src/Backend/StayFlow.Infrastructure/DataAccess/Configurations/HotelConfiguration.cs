using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess.Configurations;

internal class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
	public void Configure(EntityTypeBuilder<Hotel> builder)
	{
		builder.Property(hotel => hotel.Name).IsRequired().HasMaxLength(150);
		builder.Property(hotel => hotel.City).IsRequired().HasMaxLength(100);
	}
}
