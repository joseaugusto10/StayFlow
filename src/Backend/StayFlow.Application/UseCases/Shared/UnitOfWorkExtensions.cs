using StayFlow.Domain.Repositories;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Shared;

internal static class UnitOfWorkExtensions
{
	public static async Task CommitOrThrowOnConflict(this IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
	{
		var result = await unitOfWork.Commit(cancellationToken);

		if (result == CommitResult.ConcurrencyConflict)
			throw new ConflictException(ResourceMessagesException.CONCURRENCY_CONFLICT);
	}
}
