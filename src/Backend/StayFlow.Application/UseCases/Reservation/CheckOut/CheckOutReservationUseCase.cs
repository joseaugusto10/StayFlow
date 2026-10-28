using StayFlow.Application.UseCases.Shared;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Reservation.CheckOut;

public class CheckOutReservationUseCase : ICheckOutReservationUseCase
{
	private readonly IReservationUpdateOnlyRepository _reservationUpdateOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;
	private readonly IUnitOfWork _unitOfWork;

	public CheckOutReservationUseCase(
		IReservationUpdateOnlyRepository reservationUpdateOnlyRepository,
		ILoggedUser loggedUser,
		IDateProvider dateProvider,
		IUnitOfWork unitOfWork)
	{
		_reservationUpdateOnlyRepository = reservationUpdateOnlyRepository;
		_loggedUser = loggedUser;
		_dateProvider = dateProvider;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(Guid reservationId, CancellationToken cancellationToken = default)
	{
		var reservation = await _reservationUpdateOnlyRepository.GetById(_loggedUser.GetHotelId(), reservationId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.RESERVATION_NOT_FOUND);

		if (reservation.CanCheckOut() == false)
			throw new BusinessRuleException(ResourceMessagesException.RESERVATION_CANNOT_CHECK_OUT);

		reservation.CheckOut();

		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);
	}
}
