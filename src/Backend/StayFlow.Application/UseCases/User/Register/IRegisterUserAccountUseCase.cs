using StayFlow.Communication.Requests;

namespace StayFlow.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
	Task Execute(RequestRegisterUserAccountJson request);
}
