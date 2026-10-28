using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFlow.Domain.Entities;

namespace StayFlow.Infrastructure.DataAccess.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
	public void Configure(EntityTypeBuilder<Reservation> builder)
	{
		builder.Property(reservation => reservation.Code).IsRequired().HasMaxLength(12);
		builder.HasIndex(reservation => reservation.Code).IsUnique();
		builder.Property(reservation => reservation.Status).HasConversion<string>().HasMaxLength(20);
		builder.Property(reservation => reservation.Notes).HasMaxLength(1000);

		builder.Ignore(reservation => reservation.Nights);
		builder.Ignore(reservation => reservation.TotalGuests);

		builder.HasIndex(reservation => new { reservation.RoomId, reservation.CheckInDate, reservation.CheckOutDate });
		builder.HasIndex(reservation => new { reservation.HotelId, reservation.CheckInDate });

		builder.HasOne(reservation => reservation.Room)
			.WithMany(room => room.Reservations)
			.HasForeignKey(reservation => reservation.RoomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(reservation => reservation.Guest)
			.WithMany(guest => guest.Reservations)
			.HasForeignKey(reservation => reservation.GuestId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
