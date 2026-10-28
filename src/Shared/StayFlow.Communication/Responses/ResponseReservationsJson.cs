namespace StayFlow.Communication.Responses;

public class ResponseReservationsJson
{
	public List<ResponseReservationListItemJson> Items { get; set; } = [];
	public int TotalCount { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
