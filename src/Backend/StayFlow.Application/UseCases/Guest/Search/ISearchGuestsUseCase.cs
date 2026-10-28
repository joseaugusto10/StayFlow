using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Guest.Search;

public interface ISearchGuestsUseCase
{
	Task<List<ResponseGuestJson>> Execute(string? term, CancellationToken cancellationToken = default);
}
