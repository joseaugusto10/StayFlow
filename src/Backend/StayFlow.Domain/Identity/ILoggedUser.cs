using StayFlow.Domain.Entities;

namespace StayFlow.Domain.Identity;

public interface ILoggedUser
{
	Task<User> Get();
	Guid GetUserId();
	Guid GetHotelId();
}
