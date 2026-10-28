using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Repositories.Room;

namespace StayFlow.Infrastructure.DataAccess.Repositories;

internal sealed class RoomRepository : IRoomWriteOnlyRepository, IRoomReadOnlyRepository, IRoomUpdateOnlyRepository
{
	private readonly StayFlowDbContext _dbContext;

	public RoomRepository(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(Room room) => await _dbContext.Rooms.AddAsync(room);

	public async Task<IList<Room>> GetAll(Guid hotelId, CancellationToken cancellationToken = default) =>
		await _dbContext.Rooms
			.AsNoTracking()
			.Where(room => room.HotelId == hotelId && room.IsActive)
			.OrderBy(room => room.Floor).ThenBy(room => room.Number)
			.ToListAsync(cancellationToken);

	async Task<Room?> IRoomReadOnlyRepository.GetById(Guid hotelId, Guid roomId, CancellationToken cancellationToken) =>
		await _dbContext.Rooms
			.AsNoTracking()
			.FirstOrDefaultAsync(room => room.HotelId == hotelId && room.Id == roomId && room.IsActive, cancellationToken);

	async Task<Room?> IRoomUpdateOnlyRepository.GetById(Guid hotelId, Guid roomId, CancellationToken cancellationToken) =>
		await _dbContext.Rooms
			.FirstOrDefaultAsync(room => room.HotelId == hotelId && room.Id == roomId && room.IsActive, cancellationToken);

	public async Task<bool> ExistRoomWithNumber(Guid hotelId, string number, Guid? ignoreRoomId = null, CancellationToken cancellationToken = default) =>
		await _dbContext.Rooms.AnyAsync(room =>
			room.HotelId == hotelId &&
			room.Number == number &&
			(ignoreRoomId == null || room.Id != ignoreRoomId), cancellationToken);

	public async Task<IList<Room>> GetAvailable(Guid hotelId, DateOnly checkIn, DateOnly checkOut, int guests, CancellationToken cancellationToken = default) =>
		await _dbContext.Rooms
			.AsNoTracking()
			.Where(room =>
				room.HotelId == hotelId &&
				room.IsActive &&
				room.Status == RoomStatus.Available &&
				room.Capacity >= guests &&
				room.Reservations.Any(reservation =>
					reservation.Status != ReservationStatus.Cancelled &&
					reservation.CheckInDate < checkOut &&
					checkIn < reservation.CheckOutDate) == false)
			.OrderBy(room => room.Number)
			.ToListAsync(cancellationToken);

	public async Task<int> CountActive(Guid hotelId, CancellationToken cancellationToken = default) =>
		await _dbContext.Rooms.CountAsync(room => room.HotelId == hotelId && room.IsActive, cancellationToken);
}
