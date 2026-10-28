using System.Globalization;
using StayFlow.Communication.Enums;

namespace StayFlow.Web.Services;

public static class Format
{
	private static readonly CultureInfo Brazil = new("pt-BR");

	// Prices are always in BRL (the hotel's currency), whatever the UI language.
	public static string Money(decimal value) => value.ToString("C", Brazil);
	public static string Date(DateOnly date) => date.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
	public static string ShortDate(DateOnly date) => date.ToString("dd/MM", CultureInfo.CurrentCulture);
	public static string WeekDay(DateOnly date) => date.ToString("ddd", CultureInfo.CurrentCulture);
	public static string DateTimeLocal(DateTime value) => value.ToLocalTime().ToString("g", CultureInfo.CurrentCulture);
	public static string LongToday() => DateTime.Now.ToString("D", CultureInfo.CurrentCulture);
	public static string Percent(decimal value) => $"{value.ToString("0.#", CultureInfo.CurrentCulture)}%";

	public static string Css(ReservationStatus status) => status switch
	{
		ReservationStatus.Confirmed => "badge-blue",
		ReservationStatus.CheckedIn => "badge-green",
		ReservationStatus.CheckedOut => "badge-gray",
		ReservationStatus.Cancelled => "badge-red",
		_ => "badge-gray"
	};
}
