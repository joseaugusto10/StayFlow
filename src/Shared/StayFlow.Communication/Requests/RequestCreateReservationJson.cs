namespace StayFlow.Communication.Requests;

public class RequestCreateReservationJson
{
	public Guid RoomId { get; set; }
	public Guid GuestId { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public int Adults { get; set; } = 1;
	public int Children { get; set; }
	public string? Notes { get; set; }
}
