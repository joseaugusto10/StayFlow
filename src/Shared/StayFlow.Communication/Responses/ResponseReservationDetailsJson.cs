using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Responses;

public class ResponseReservationDetailsJson
{
	public Guid Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public int Nights { get; set; }
	public int Adults { get; set; }
	public int Children { get; set; }
	public decimal TotalAmount { get; set; }
	public ReservationStatus Status { get; set; }
	public string? Notes { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? CheckedInAt { get; set; }
	public DateTime? CheckedOutAt { get; set; }
	public DateTime? CancelledAt { get; set; }
	public ResponseGuestJson Guest { get; set; } = default!;
	public ResponseRoomJson Room { get; set; } = default!;
	public bool CanCheckIn { get; set; }
	public bool CanCheckOut { get; set; }
	public bool CanCancel { get; set; }
}
