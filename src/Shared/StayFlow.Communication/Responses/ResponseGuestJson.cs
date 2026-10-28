namespace StayFlow.Communication.Responses;

public class ResponseGuestJson
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Document { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Nationality { get; set; }
}
