using StayFlow.Domain.Entities;

namespace StayFlow.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
	string Generate(User user);
}
