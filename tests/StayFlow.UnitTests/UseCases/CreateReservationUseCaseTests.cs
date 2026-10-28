using Moq;
using StayFlow.Application.UseCases.Reservation.Create;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Guest;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.UseCases;

public class CreateReservationUseCaseTests
{
	private readonly Mock<IRoomUpdateOnlyRepository> _rooms = new();
	private readonly Mock<IGuestReadOnlyRepository> _guests = new();
	private readonly Mock<IReservationReadOnlyRepository> _reservationsRead = new();
	private readonly Mock<IReservationWriteOnlyRepository> _reservationsWrite = new();
	private readonly Mock<ILoggedUser> _loggedUser = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	private readonly Room _room = Builders.Room(capacity: 2, rate: 100m);
	private readonly Guest _guest = Builders.Guest();

	public CreateReservationUseCaseTests()
	{
		_loggedUser.Setup(user => user.GetHotelId()).Returns(Builders.HotelId);
		_rooms.Setup(repo => repo.GetById(Builders.HotelId, _room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_room);
		_guests.Setup(repo => repo.GetById(Builders.HotelId, _guest.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_guest);
	}

	private CreateReservationUseCase CreateUseCase() => new(
		_rooms.Object, _guests.Object, _reservationsRead.Object, _reservationsWrite.Object,
		_loggedUser.Object, new FixedDateProvider(), _unitOfWork.Object);

	[Fact]
	public async Task Success_Persists_Reservation_With_Calculated_Price_And_Rotates_Concurrency_Stamp()
	{
		var stampBefore = _room.ConcurrencyStamp;
		Reservation? saved = null;
		_reservationsWrite.Setup(repo => repo.Add(It.IsAny<Reservation>())).Callback<Reservation>(r => saved = r);

		var response = await CreateUseCase().Execute(Builders.CreateReservation(_room.Id, _guest.Id, nights: 2));

		response.TotalAmount.ShouldBe(200m);
		response.Code.ShouldStartWith("SF-");
		saved.ShouldNotBeNull();
		saved.Status.ShouldBe(ReservationStatus.Confirmed);
		_room.ConcurrencyStamp.ShouldNotBe(stampBefore);
		_unitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task Overlapping_Dates_Are_Rejected()
	{
		_reservationsRead
			.Setup(repo => repo.HasOverlap(_room.Id, It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);

		var exception = await Should.ThrowAsync<BusinessRuleException>(
			() => CreateUseCase().Execute(Builders.CreateReservation(_room.Id, _guest.Id)));

		exception.GetErrorMessages().ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.ROOM_NOT_AVAILABLE);
		_unitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task Room_Under_Maintenance_Is_Rejected()
	{
		_room.Status = RoomStatus.Maintenance;

		var exception = await Should.ThrowAsync<BusinessRuleException>(
			() => CreateUseCase().Execute(Builders.CreateReservation(_room.Id, _guest.Id)));

		exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ROOM_UNDER_MAINTENANCE);
	}

	[Fact]
	public async Task Party_Larger_Than_Capacity_Is_Rejected()
	{
		var exception = await Should.ThrowAsync<BusinessRuleException>(
			() => CreateUseCase().Execute(Builders.CreateReservation(_room.Id, _guest.Id, adults: 3)));

		exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ROOM_CAPACITY_EXCEEDED);
	}

	[Fact]
	public async Task Room_From_Another_Hotel_Is_Not_Found()
	{
		var foreignRoomId = Guid.NewGuid();

		var exception = await Should.ThrowAsync<NotFoundException>(
			() => CreateUseCase().Execute(Builders.CreateReservation(foreignRoomId, _guest.Id)));

		exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ROOM_NOT_FOUND);
	}
}
