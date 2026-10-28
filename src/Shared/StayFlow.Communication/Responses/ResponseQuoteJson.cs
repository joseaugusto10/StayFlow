namespace StayFlow.Communication.Responses;

public class ResponseQuoteJson
{
	public int Nights { get; set; }
	public int WeekendNights { get; set; }
	public decimal Subtotal { get; set; }
	public decimal Discount { get; set; }
	public decimal Total { get; set; }
}
