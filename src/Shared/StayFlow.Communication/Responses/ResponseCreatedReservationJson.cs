namespace StayFlow.Communication.Responses;

public class ResponseCreatedReservationJson
{
	public Guid Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public decimal TotalAmount { get; set; }
}
