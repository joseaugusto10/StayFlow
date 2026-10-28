using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess.Configurations;

internal class RoomConfiguration : IEntityTypeConfiguration<Room>
{
	public void Configure(EntityTypeBuilder<Room> builder)
	{
		builder.Property(room => room.Number).IsRequired().HasMaxLength(10);
		builder.HasIndex(room => new { room.HotelId, room.Number }).IsUnique();
		builder.Property(room => room.Type).HasConversion<string>().HasMaxLength(20);
		builder.Property(room => room.Status).HasConversion<string>().HasMaxLength(20);
		builder.Property(room => room.Description).HasMaxLength(500);
		builder.Property(room => room.ConcurrencyStamp).IsConcurrencyToken();

		builder.HasOne(room => room.Hotel)
			.WithMany(hotel => hotel.Rooms)
			.HasForeignKey(room => room.HotelId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
