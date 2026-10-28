using StayFlow.Communication.Enums;

namespace StayFlow.Communication.Requests;

public class RequestFiltersReservationsJson
{
	public string? Search { get; set; }
	public ReservationStatus? Status { get; set; }
	public DateOnly? From { get; set; }
	public DateOnly? To { get; set; }
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}
