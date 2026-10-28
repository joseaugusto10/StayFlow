using StayFlow.Domain.Services;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.Domain;

public class StayPricingCalculatorTests
{
	private static readonly DateOnly Monday = FixedDateProvider.MONDAY;

	[Fact]
	public void Weekday_Nights_Use_Base_Rate()
	{
		var quote = StayPricingCalculator.Calculate(100m, Monday, Monday.AddDays(3)); // Mon, Tue, Wed

		quote.Nights.ShouldBe(3);
		quote.WeekendNights.ShouldBe(0);
		quote.Total.ShouldBe(300m);
	}

	[Fact]
	public void Friday_And_Saturday_Nights_Have_Surcharge()
	{
		var friday = Monday.AddDays(4);
		var quote = StayPricingCalculator.Calculate(100m, friday, friday.AddDays(3)); // Fri, Sat, Sun

		quote.WeekendNights.ShouldBe(2);
		quote.Total.ShouldBe(125m + 125m + 100m);
	}

	[Fact]
	public void Long_Stay_Receives_Discount()
	{
		var quote = StayPricingCalculator.Calculate(100m, Monday, Monday.AddDays(7)); // 5 weekdays + Fri + Sat

		quote.Subtotal.ShouldBe(750m);
		quote.Discount.ShouldBe(75m);
		quote.Total.ShouldBe(675m);
	}

	[Fact]
	public void Invalid_Interval_Returns_Empty_Quote()
	{
		StayPricingCalculator.Calculate(100m, Monday, Monday).ShouldBe(StayQuote.Empty);
		StayPricingCalculator.Calculate(100m, Monday, Monday.AddDays(-2)).ShouldBe(StayQuote.Empty);
	}
}
