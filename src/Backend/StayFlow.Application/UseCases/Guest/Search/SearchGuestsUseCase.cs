using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories.Guest;

namespace StayFlow.Application.UseCases.Guest.Search;

public class SearchGuestsUseCase : ISearchGuestsUseCase
{
	private const int MAX_RESULTS = 50;

	private readonly IGuestReadOnlyRepository _guestReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;

	public SearchGuestsUseCase(IGuestReadOnlyRepository guestReadOnlyRepository, ILoggedUser loggedUser)
	{
		_guestReadOnlyRepository = guestReadOnlyRepository;
		_loggedUser = loggedUser;
	}

	public async Task<List<ResponseGuestJson>> Execute(string? term, CancellationToken cancellationToken = default)
	{
		var guests = await _guestReadOnlyRepository.Search(_loggedUser.GetHotelId(), term?.Trim(), MAX_RESULTS, cancellationToken);
		return guests.Select(GuestMapper.ToResponse).ToList();
	}
}
