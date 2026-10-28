namespace StayFlow.Domain.Repositories.Hotel;

public interface IHotelWriteOnlyRepository
{
	Task Add(Entities.Hotel hotel);
}
