using StayFlow.Domain.Enums;

namespace StayFlow.Domain.Entities;

public class User : EntityBase
{
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public UserRole Role { get; set; } = UserRole.Receptionist;
	public Guid HotelId { get; set; }
	public Hotel Hotel { get; set; } = default!;
}
