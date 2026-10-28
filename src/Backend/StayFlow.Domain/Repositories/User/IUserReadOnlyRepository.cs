namespace StayFlow.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
	Task<bool> ExistUserWithEmail(string email);
	Task<bool> ExistActiveUserWithId(Guid userId);
	Task<Entities.User?> GetByEmail(string email);
	Task<IList<Entities.User>> GetByHotel(Guid hotelId, CancellationToken cancellationToken = default);
}
