using StayFlow.Application.Mappings;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Domain.Services;

namespace StayFlow.Application.UseCases.Room.GetAvailability;

public class GetRoomAvailabilityUseCase : IGetRoomAvailabilityUseCase
{
	private readonly IRoomReadOnlyRepository _roomReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;

	public GetRoomAvailabilityUseCase(
		IRoomReadOnlyRepository roomReadOnlyRepository,
		ILoggedUser loggedUser,
		IDateProvider dateProvider)
	{
		_roomReadOnlyRepository = roomReadOnlyRepository;
		_loggedUser = loggedUser;
		_dateProvider = dateProvider;
	}

	public async Task<List<ResponseAvailableRoomJson>> Execute(RequestAvailabilityJson request, CancellationToken cancellationToken = default)
	{
		new GetRoomAvailabilityValidator(_dateProvider).Validate(request).ThrowIfInvalid();

		var rooms = await _roomReadOnlyRepository.GetAvailable(
			_loggedUser.GetHotelId(), request.CheckIn, request.CheckOut, request.Guests, cancellationToken);

		return rooms
			.Select(room => new ResponseAvailableRoomJson
			{
				Room = RoomMapper.ToResponse(room),
				Quote = RoomMapper.ToResponse(StayPricingCalculator.Calculate(room.NightlyRate, request.CheckIn, request.CheckOut))
			})
			.OrderBy(item => item.Quote.Total)
			.ToList();
	}
}
