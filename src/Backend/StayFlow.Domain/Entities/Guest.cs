namespace StayFlow.Domain.Entities;

public class Guest : EntityBase
{
	public string Name { get; set; } = string.Empty;
	public string Document { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Nationality { get; set; }
	public Guid HotelId { get; set; }
	public ICollection<Reservation> Reservations { get; set; } = [];
}
