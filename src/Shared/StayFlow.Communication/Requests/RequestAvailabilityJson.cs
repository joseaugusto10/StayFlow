namespace StayFlow.Communication.Requests;

public class RequestAvailabilityJson
{
	public DateOnly CheckIn { get; set; }
	public DateOnly CheckOut { get; set; }
	public int Guests { get; set; } = 1;
}
