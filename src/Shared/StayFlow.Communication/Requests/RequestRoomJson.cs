using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Requests;

public class RequestRoomJson
{
	public string Number { get; set; } = string.Empty;
	public int Floor { get; set; }
	public RoomType Type { get; set; } = RoomType.Double;
	public int Capacity { get; set; } = 2;
	public decimal NightlyRate { get; set; }
	public string? Description { get; set; }
}
