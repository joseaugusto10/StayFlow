using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Guest;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Domain.Services;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Reservation.Create;

public class CreateReservationUseCase : ICreateReservationUseCase
{
	private readonly IRoomUpdateOnlyRepository _roomUpdateOnlyRepository;
	private readonly IGuestReadOnlyRepository _guestReadOnlyRepository;
	private readonly IReservationReadOnlyRepository _reservationReadOnlyRepository;
	private readonly IReservationWriteOnlyRepository _reservationWriteOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;
	private readonly IUnitOfWork _unitOfWork;

	public CreateReservationUseCase(
		IRoomUpdateOnlyRepository roomUpdateOnlyRepository,
		IGuestReadOnlyRepository guestReadOnlyRepository,
		IReservationReadOnlyRepository reservationReadOnlyRepository,
		IReservationWriteOnlyRepository reservationWriteOnlyRepository,
		ILoggedUser loggedUser,
		IDateProvider dateProvider,
		IUnitOfWork unitOfWork)
	{
		_roomUpdateOnlyRepository = roomUpdateOnlyRepository;
		_guestReadOnlyRepository = guestReadOnlyRepository;
		_reservationReadOnlyRepository = reservationReadOnlyRepository;
		_reservationWriteOnlyRepository = reservationWriteOnlyRepository;
		_loggedUser = loggedUser;
		_dateProvider = dateProvider;
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseCreatedReservationJson> Execute(RequestCreateReservationJson request, CancellationToken cancellationToken = default)
	{
		new CreateReservationValidator(_dateProvider).Validate(request).ThrowIfInvalid();

		var hotelId = _loggedUser.GetHotelId();

		var room = await _roomUpdateOnlyRepository.GetById(hotelId, request.RoomId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.ROOM_NOT_FOUND);

		var guest = await _guestReadOnlyRepository.GetById(hotelId, request.GuestId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.GUEST_NOT_FOUND);

		if (room.IsBookable() == false)
			throw new BusinessRuleException(ResourceMessagesException.ROOM_UNDER_MAINTENANCE);

		if (request.Adults + request.Children > room.Capacity)
			throw new BusinessRuleException(ResourceMessagesException.ROOM_CAPACITY_EXCEEDED);

		var hasOverlap = await _reservationReadOnlyRepository.HasOverlap(
			room.Id, request.CheckInDate, request.CheckOutDate, cancellationToken);
		if (hasOverlap)
			throw new BusinessRuleException(ResourceMessagesException.ROOM_NOT_AVAILABLE);

		var quote = StayPricingCalculator.Calculate(room.NightlyRate, request.CheckInDate, request.CheckOutDate);

		var reservation = new Domain.Entities.Reservation
		{
			Code = Domain.Entities.Reservation.GenerateCode(),
			HotelId = hotelId,
			RoomId = room.Id,
			GuestId = guest.Id,
			CheckInDate = request.CheckInDate,
			CheckOutDate = request.CheckOutDate,
			Adults = request.Adults,
			Children = request.Children,
			Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
			TotalAmount = quote.Total,
			Status = ReservationStatus.Confirmed
		};

		room.Touch();

		await _reservationWriteOnlyRepository.Add(reservation);
		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);

		return new ResponseCreatedReservationJson
		{
			Id = reservation.Id,
			Code = reservation.Code,
			TotalAmount = reservation.TotalAmount
		};
	}
}
