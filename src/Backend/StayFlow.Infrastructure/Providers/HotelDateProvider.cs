using StayFlow.Domain.Providers;

namespace StayFlow.Infrastructure.Providers;

internal sealed class HotelDateProvider : IDateProvider
{
	private readonly TimeZoneInfo _timeZone;

	public HotelDateProvider(string timeZoneId)
	{
		_timeZone = TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var timeZone)
			? timeZone
			: TimeZoneInfo.Utc;
	}

	public DateOnly Today => DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone));
}
