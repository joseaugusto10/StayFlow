namespace StayFlow.Domain.Repositories;

public interface IUnitOfWork
{
	Task<CommitResult> Commit(CancellationToken cancellationToken = default);
}
