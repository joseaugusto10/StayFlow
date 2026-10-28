using StayFlow.Domain.Enums;

namespace StayFlow.Domain.Entities;

public class Reservation : EntityBase
{
	public string Code { get; set; } = string.Empty;
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public int Adults { get; set; } = 1;
	public int Children { get; set; }
	public decimal TotalAmount { get; set; }
	public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
	public string? Notes { get; set; }
	public DateTime? CheckedInAt { get; set; }
	public DateTime? CheckedOutAt { get; set; }
	public DateTime? CancelledAt { get; set; }

	public Guid HotelId { get; set; }
	public Guid RoomId { get; set; }
	public Room Room { get; set; } = default!;
	public Guid GuestId { get; set; }
	public Guest Guest { get; set; } = default!;

	public int Nights => CheckOutDate.DayNumber - CheckInDate.DayNumber;
	public int TotalGuests => Adults + Children;

	public bool CanCheckIn(DateOnly today) =>
		Status == ReservationStatus.Confirmed && today >= CheckInDate && today < CheckOutDate;

	public bool CanCheckOut() => Status == ReservationStatus.CheckedIn;

	public bool CanCancel() => Status == ReservationStatus.Confirmed;

	public void CheckIn()
	{
		Status = ReservationStatus.CheckedIn;
		CheckedInAt = DateTime.UtcNow;
		UpdatedAt = DateTime.UtcNow;
	}

	public void CheckOut()
	{
		Status = ReservationStatus.CheckedOut;
		CheckedOutAt = DateTime.UtcNow;
		UpdatedAt = DateTime.UtcNow;
	}

	public void Cancel()
	{
		Status = ReservationStatus.Cancelled;
		CancelledAt = DateTime.UtcNow;
		UpdatedAt = DateTime.UtcNow;
	}

	public static string GenerateCode() =>
		$"SF-{Convert.ToHexString(Guid.NewGuid().ToByteArray())[..6]}";
}
