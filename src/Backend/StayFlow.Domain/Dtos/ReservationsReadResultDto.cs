using StayFlow.Domain.Entities;

namespace StayFlow.Domain.Dtos;

public sealed class ReservationsReadResultDto
{
	public IList<Reservation> Items { get; init; } = [];
	public int TotalCount { get; init; }
}
