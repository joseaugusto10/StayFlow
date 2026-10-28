using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Dashboard;

public interface IGetDashboardUseCase
{
	Task<ResponseDashboardJson> Execute(CancellationToken cancellationToken = default);
}
