using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Responses;

public class ResponseRoomJson
{
	public Guid Id { get; set; }
	public string Number { get; set; } = string.Empty;
	public int Floor { get; set; }
	public RoomType Type { get; set; }
	public int Capacity { get; set; }
	public decimal NightlyRate { get; set; }
	public RoomStatus Status { get; set; }
	public string? Description { get; set; }
}
