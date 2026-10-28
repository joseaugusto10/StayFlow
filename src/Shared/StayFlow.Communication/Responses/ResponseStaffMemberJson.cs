using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Responses;

public class ResponseStaffMemberJson
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public UserRole Role { get; set; }
	public DateTime CreatedAt { get; set; }
}
