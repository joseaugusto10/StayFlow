using StayFlow.Domain.Enums;

namespace StayFlow.Domain.Repositories.Reservation;

public sealed class ReservationFilters
{
	public string? Search { get; init; }
	public ReservationStatus? Status { get; init; }
	public DateOnly? From { get; init; }
	public DateOnly? To { get; init; }
	public int Page { get; init; } = 1;
	public int PageSize { get; init; } = 10;
}
