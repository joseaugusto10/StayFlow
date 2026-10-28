using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;

namespace StayFlow.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
	private readonly ILoggedUser _loggedUser;

	public GetUserProfileUseCase(ILoggedUser loggedUser)
	{
		_loggedUser = loggedUser;
	}

	public async Task<ResponseUserProfileJson> Execute()
	{
		var user = await _loggedUser.Get();
		return UserMapper.ToProfileResponse(user);
	}
}
