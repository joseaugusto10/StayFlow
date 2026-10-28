using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFlow.Application.UseCases.Dashboard;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(ResponseDashboardJson), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get(
		[FromServices] IGetDashboardUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(cancellationToken));
	}
}
