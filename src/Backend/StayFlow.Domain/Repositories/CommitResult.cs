namespace StayFlow.Domain.Repositories;

/// <summary>
/// Outcome of a unit of work. Lets Infrastructure report an optimistic-concurrency
/// conflict without throwing exceptions from (or depending on) outer layers.
/// </summary>
public enum CommitResult
{
	Success = 0,
	ConcurrencyConflict = 1
}
