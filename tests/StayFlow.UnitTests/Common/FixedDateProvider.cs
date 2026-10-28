using StayFlow.Domain.Providers;

namespace StayFlow.UnitTests.Common;

public sealed class FixedDateProvider : IDateProvider
{
	public static readonly DateOnly MONDAY = new(2030, 1, 7);

	public DateOnly Today { get; init; } = MONDAY;
}
