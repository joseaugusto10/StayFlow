using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Responses;

public class ResponseReservationListItemJson
{
	public Guid Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public string GuestName { get; set; } = string.Empty;
	public string RoomNumber { get; set; } = string.Empty;
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public int Nights { get; set; }
	public int Guests { get; set; }
	public decimal TotalAmount { get; set; }
	public ReservationStatus Status { get; set; }
}
