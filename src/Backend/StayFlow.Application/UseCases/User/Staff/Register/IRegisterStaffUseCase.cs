using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.User.Staff.Register;

public interface IRegisterStaffUseCase
{
	Task<ResponseCreatedJson> Execute(RequestRegisterStaffJson request);
}
