using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Responses;

public class ResponseUserProfileJson
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public UserRole Role { get; set; }
	public string HotelName { get; set; } = string.Empty;
	public string HotelCity { get; set; } = string.Empty;
}
