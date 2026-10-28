namespace StayFlow.Domain.Repositories.Room;

public interface IRoomUpdateOnlyRepository
{
	Task<Entities.Room?> GetById(Guid hotelId, Guid roomId, CancellationToken cancellationToken = default);
}
