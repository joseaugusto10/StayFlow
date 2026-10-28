using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFlow.Application.UseCases.Reservation.Cancel;
using StayFlow.Application.UseCases.Reservation.CheckIn;
using StayFlow.Application.UseCases.Reservation.CheckOut;
using StayFlow.Application.UseCases.Reservation.Create;
using StayFlow.Application.UseCases.Reservation.GetById;
using StayFlow.Application.UseCases.Reservation.GetWithFilters;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReservationsController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseCreatedReservationJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> Create(
		[FromBody] RequestCreateReservationJson request,
		[FromServices] ICreateReservationUseCase useCase,
		CancellationToken cancellationToken)
	{
		var response = await useCase.Execute(request, cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
	}

	[HttpPost("search")]
	[ProducesResponseType(typeof(ResponseReservationsJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Search(
		[FromBody] RequestFiltersReservationsJson request,
		[FromServices] IGetReservationsWithFiltersUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(request, cancellationToken));
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(ResponseReservationDetailsJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(
		[FromRoute] Guid id,
		[FromServices] IGetReservationByIdUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(id, cancellationToken));
	}

	[HttpPatch("{id:guid}/check-in")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> CheckIn(
		[FromRoute] Guid id,
		[FromServices] ICheckInReservationUseCase useCase,
		CancellationToken cancellationToken)
	{
		await useCase.Execute(id, cancellationToken);
		return NoContent();
	}

	[HttpPatch("{id:guid}/check-out")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> CheckOut(
		[FromRoute] Guid id,
		[FromServices] ICheckOutReservationUseCase useCase,
		CancellationToken cancellationToken)
	{
		await useCase.Execute(id, cancellationToken);
		return NoContent();
	}

	[HttpPatch("{id:guid}/cancel")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> Cancel(
		[FromRoute] Guid id,
		[FromServices] ICancelReservationUseCase useCase,
		CancellationToken cancellationToken)
	{
		await useCase.Execute(id, cancellationToken);
		return NoContent();
	}
}
