using StayFlow.Application.UseCases.Shared;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Reservation.Cancel;

public class CancelReservationUseCase : ICancelReservationUseCase
{
	private readonly IReservationUpdateOnlyRepository _reservationUpdateOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;
	private readonly IUnitOfWork _unitOfWork;

	public CancelReservationUseCase(
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

		if (reservation.CanCancel() == false)
			throw new BusinessRuleException(ResourceMessagesException.RESERVATION_CANNOT_CANCEL);

		reservation.Cancel();

		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);
	}
}
