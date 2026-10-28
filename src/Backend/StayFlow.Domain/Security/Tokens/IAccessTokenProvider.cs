namespace StayFlow.Domain.Security.Tokens;

public interface IAccessTokenProvider
{
	string GetToken();
}
