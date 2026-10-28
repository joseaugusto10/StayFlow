using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StayFlow.Api.Authorization;
using StayFlow.Application.UseCases.User.Profile;
using StayFlow.Application.UseCases.User.Register;
using StayFlow.Application.UseCases.User.Staff.GetAll;
using StayFlow.Application.UseCases.User.Staff.Register;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
	[HttpPost("register")]
	[AllowAnonymous]
	[EnableRateLimiting(RateLimitPolicies.AUTH)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register(
		[FromBody] RequestRegisterUserAccountJson request,
		[FromServices] IRegisterUserAccountUseCase useCase)
	{
		await useCase.Execute(request);
		return StatusCode(StatusCodes.Status201Created);
	}

	[HttpGet("profile")]
	[ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Profile([FromServices] IGetUserProfileUseCase useCase)
	{
		return Ok(await useCase.Execute());
	}

	[HttpGet("staff")]
	[Authorize(Policy = AuthorizationPolicies.MANAGER_ONLY)]
	[ProducesResponseType(typeof(List<ResponseStaffMemberJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetStaff(
		[FromServices] IGetStaffUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(cancellationToken));
	}

	[HttpPost("staff")]
	[Authorize(Policy = AuthorizationPolicies.MANAGER_ONLY)]
	[ProducesResponseType(typeof(ResponseCreatedJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> RegisterStaff(
		[FromBody] RequestRegisterStaffJson request,
		[FromServices] IRegisterStaffUseCase useCase)
	{
		var response = await useCase.Execute(request);
		return StatusCode(StatusCodes.Status201Created, response);
	}
}
