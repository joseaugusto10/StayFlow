namespace StayFlow.Communication.Responses;

public class ResponseDashboardJson
{
	public int TotalRooms { get; set; }
	public int OccupiedToday { get; set; }
	public decimal OccupancyRateToday { get; set; }
	public int GuestsInHouse { get; set; }
	public decimal RevenueThisMonth { get; set; }

	/// <summary>ADR - Average Daily Rate: room revenue / room nights sold (current month).</summary>
	public decimal AverageDailyRate { get; set; }

	/// <summary>RevPAR - Revenue per Available Room: room revenue / room nights available (current month).</summary>
	public decimal RevPar { get; set; }

	public List<ResponseReservationListItemJson> ArrivalsToday { get; set; } = [];
	public List<ResponseReservationListItemJson> DeparturesToday { get; set; } = [];
	public List<ResponseOccupancyPointJson> OccupancyForecast { get; set; } = [];
}

public class ResponseOccupancyPointJson
{
	public DateOnly Date { get; set; }
	public int OccupiedRooms { get; set; }
	public decimal Rate { get; set; }
}
