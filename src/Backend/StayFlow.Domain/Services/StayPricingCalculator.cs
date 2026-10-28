namespace StayFlow.Domain.Services;

public static class StayPricingCalculator
{
	public const decimal WEEKEND_SURCHARGE = 0.25m;
	public const decimal LONG_STAY_DISCOUNT = 0.10m;
	public const int LONG_STAY_MIN_NIGHTS = 7;

	public static StayQuote Calculate(decimal nightlyRate, DateOnly checkIn, DateOnly checkOut)
	{
		var nights = checkOut.DayNumber - checkIn.DayNumber;
		if (nights <= 0)
			return StayQuote.Empty;

		var subtotal = 0m;
		var weekendNights = 0;

		for (var date = checkIn; date < checkOut; date = date.AddDays(1))
		{
			var isWeekendNight = date.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday;
			if (isWeekendNight)
			{
				weekendNights++;
				subtotal += nightlyRate * (1 + WEEKEND_SURCHARGE);
			}
			else
			{
				subtotal += nightlyRate;
			}
		}

		var discount = nights >= LONG_STAY_MIN_NIGHTS
			? Math.Round(subtotal * LONG_STAY_DISCOUNT, 2)
			: 0m;

		return new StayQuote(
			Nights: nights,
			WeekendNights: weekendNights,
			Subtotal: Math.Round(subtotal, 2),
			Discount: discount,
			Total: Math.Round(subtotal - discount, 2));
	}
}

public sealed record StayQuote(int Nights, int WeekendNights, decimal Subtotal, decimal Discount, decimal Total)
{
	public static readonly StayQuote Empty = new(0, 0, 0m, 0m, 0m);
}
