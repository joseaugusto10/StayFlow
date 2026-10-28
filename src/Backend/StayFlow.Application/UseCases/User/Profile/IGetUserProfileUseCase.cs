using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
	Task<ResponseUserProfileJson> Execute();
}
