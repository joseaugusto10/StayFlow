using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Reservation.GetById;

public class GetReservationByIdUseCase : IGetReservationByIdUseCase
{
	private readonly IReservationReadOnlyRepository _reservationReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;

	public GetReservationByIdUseCase(
		IReservationReadOnlyRepository reservationReadOnlyRepository,
		ILoggedUser loggedUser,
		IDateProvider dateProvider)
	{
		_reservationReadOnlyRepository = reservationReadOnlyRepository;
		_loggedUser = loggedUser;
		_dateProvider = dateProvider;
	}

	public async Task<ResponseReservationDetailsJson> Execute(Guid reservationId, CancellationToken cancellationToken = default)
	{
		var reservation = await _reservationReadOnlyRepository.GetById(_loggedUser.GetHotelId(), reservationId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.RESERVATION_NOT_FOUND);

		return ReservationMapper.ToDetails(reservation, _dateProvider.Today);
	}
}
