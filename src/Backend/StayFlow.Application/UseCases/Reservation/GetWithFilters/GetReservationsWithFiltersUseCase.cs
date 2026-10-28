using StayFlow.Application.Mappings;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories.Reservation;
using DomainEnums = StayFlow.Domain.Enums;

namespace StayFlow.Application.UseCases.Reservation.GetWithFilters;

public class GetReservationsWithFiltersUseCase : IGetReservationsWithFiltersUseCase
{
	private readonly IReservationReadOnlyRepository _reservationReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;

	public GetReservationsWithFiltersUseCase(IReservationReadOnlyRepository reservationReadOnlyRepository, ILoggedUser loggedUser)
	{
		_reservationReadOnlyRepository = reservationReadOnlyRepository;
		_loggedUser = loggedUser;
	}

	public async Task<ResponseReservationsJson> Execute(RequestFiltersReservationsJson request, CancellationToken cancellationToken = default)
	{
		new GetReservationsWithFiltersValidator().Validate(request).ThrowIfInvalid();

		var filters = new ReservationFilters
		{
			Search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim(),
			Status = request.Status.HasValue ? (DomainEnums.ReservationStatus)request.Status.Value : null,
			From = request.From,
			To = request.To,
			Page = request.Page,
			PageSize = request.PageSize
		};

		var result = await _reservationReadOnlyRepository.GetWithFilters(_loggedUser.GetHotelId(), filters, cancellationToken);

		return new ResponseReservationsJson
		{
			Items = result.Items.Select(ReservationMapper.ToListItem).ToList(),
			TotalCount = result.TotalCount,
			Page = request.Page,
			PageSize = request.PageSize
		};
	}
}
