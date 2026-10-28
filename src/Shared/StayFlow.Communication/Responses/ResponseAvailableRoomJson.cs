namespace StayFlow.Communication.Responses;

public class ResponseAvailableRoomJson
{
	public ResponseRoomJson Room { get; set; } = default!;
	public ResponseQuoteJson Quote { get; set; } = default!;
}
