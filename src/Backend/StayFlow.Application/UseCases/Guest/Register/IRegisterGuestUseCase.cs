using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Guest.Register;

public interface IRegisterGuestUseCase
{
	Task<ResponseGuestJson> Execute(RequestRegisterGuestJson request, CancellationToken cancellationToken = default);
}
