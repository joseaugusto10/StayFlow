using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.User.Staff.GetAll;

public interface IGetStaffUseCase
{
	Task<List<ResponseStaffMemberJson>> Execute(CancellationToken cancellationToken = default);
}
