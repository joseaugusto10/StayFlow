using StayFlow.Domain.Enums;

namespace StayFlow.Domain.Entities;

public class Room : EntityBase
{
	public string Number { get; set; } = string.Empty;
	public int Floor { get; set; }
	public RoomType Type { get; set; }
	public int Capacity { get; set; }
	public decimal NightlyRate { get; set; }
	public RoomStatus Status { get; set; } = RoomStatus.Available;
	public string? Description { get; set; }

	/// <summary>
	/// Optimistic concurrency token. Every new booking "touches" the room, so two
	/// simultaneous bookings for the same room cannot both be persisted.
	/// </summary>
	public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

	public Guid HotelId { get; set; }
	public Hotel Hotel { get; set; } = default!;
	public ICollection<Reservation> Reservations { get; set; } = [];

	public bool IsBookable() => IsActive && Status == RoomStatus.Available;

	public void Touch()
	{
		ConcurrencyStamp = Guid.NewGuid();
		UpdatedAt = DateTime.UtcNow;
	}
}
