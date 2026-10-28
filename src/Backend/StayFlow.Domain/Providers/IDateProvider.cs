namespace StayFlow.Domain.Providers;

public interface IDateProvider
{
	DateOnly Today { get; }
}
