using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Repositories;

namespace StayFlow.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
	private readonly StayFlowDbContext _dbContext;

	public UnitOfWork(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<CommitResult> Commit(CancellationToken cancellationToken = default)
	{
		try
		{
			await _dbContext.SaveChangesAsync(cancellationToken);
			return CommitResult.Success;
		}
		catch (DbUpdateConcurrencyException)
		{
			return CommitResult.ConcurrencyConflict;
		}
	}
}
