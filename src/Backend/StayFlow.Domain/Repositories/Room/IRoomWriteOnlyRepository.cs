namespace StayFlow.Domain.Repositories.Room;

public interface IRoomWriteOnlyRepository
{
	Task Add(Entities.Room room);
}
