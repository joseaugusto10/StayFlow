using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.User.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
	Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
