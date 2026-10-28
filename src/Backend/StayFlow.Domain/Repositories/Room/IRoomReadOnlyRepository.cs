namespace StayFlow.Domain.Repositories.Room;

public interface IRoomReadOnlyRepository
{
	Task<IList<Entities.Room>> GetAll(Guid hotelId, CancellationToken cancellationToken = default);
	Task<Entities.Room?> GetById(Guid hotelId, Guid roomId, CancellationToken cancellationToken = default);
	Task<bool> ExistRoomWithNumber(Guid hotelId, string number, Guid? ignoreRoomId = null, CancellationToken cancellationToken = default);
	Task<IList<Entities.Room>> GetAvailable(Guid hotelId, DateOnly checkIn, DateOnly checkOut, int guests, CancellationToken cancellationToken = default);
	Task<int> CountActive(Guid hotelId, CancellationToken cancellationToken = default);
}
