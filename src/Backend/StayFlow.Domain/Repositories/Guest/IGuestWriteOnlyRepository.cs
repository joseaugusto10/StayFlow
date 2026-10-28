namespace StayFlow.Domain.Repositories.Guest;

public interface IGuestWriteOnlyRepository
{
	Task Add(Entities.Guest guest);
}
