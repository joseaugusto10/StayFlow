namespace StayFlow.Domain.Entities;

public class Hotel : EntityBase
{
	public string Name { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public int Stars { get; set; } = 3;
	public ICollection<User> Users { get; set; } = [];
	public ICollection<Room> Rooms { get; set; } = [];
}
