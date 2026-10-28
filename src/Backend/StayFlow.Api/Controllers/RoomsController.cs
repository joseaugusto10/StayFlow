using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFlow.Api.Authorization;
using StayFlow.Application.UseCases.Room.ChangeStatus;
using StayFlow.Application.UseCases.Room.GetAll;
using StayFlow.Application.UseCases.Room.GetAvailability;
using StayFlow.Application.UseCases.Room.Register;
using StayFlow.Application.UseCases.Room.Update;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(List<ResponseRoomJson>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(
		[FromServices] IGetRoomsUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(cancellationToken));
	}

	[HttpGet("availability")]
	[ProducesResponseType(typeof(List<ResponseAvailableRoomJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetAvailability(
		[FromQuery] RequestAvailabilityJson request,
		[FromServices] IGetRoomAvailabilityUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(request, cancellationToken));
	}

	[HttpPost]
	[Authorize(Policy = AuthorizationPolicies.MANAGER_ONLY)]
	[ProducesResponseType(typeof(ResponseRoomJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Register(
		[FromBody] RequestRoomJson request,
		[FromServices] IRegisterRoomUseCase useCase,
		CancellationToken cancellationToken)
	{
		var response = await useCase.Execute(request, cancellationToken);
		return StatusCode(StatusCodes.Status201Created, response);
	}

	[HttpPut("{id:guid}")]
	[Authorize(Policy = AuthorizationPolicies.MANAGER_ONLY)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(
		[FromRoute] Guid id,
		[FromBody] RequestRoomJson request,
		[FromServices] IUpdateRoomUseCase useCase,
		CancellationToken cancellationToken)
	{
		await useCase.Execute(id, request, cancellationToken);
		return NoContent();
	}

	[HttpPatch("{id:guid}/status")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ChangeStatus(
		[FromRoute] Guid id,
		[FromBody] RequestChangeRoomStatusJson request,
		[FromServices] IChangeRoomStatusUseCase useCase,
		CancellationToken cancellationToken)
	{
		await useCase.Execute(id, request, cancellationToken);
		return NoContent();
	}
}
