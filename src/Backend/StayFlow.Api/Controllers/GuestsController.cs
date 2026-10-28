using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFlow.Application.UseCases.Guest.Register;
using StayFlow.Application.UseCases.Guest.Search;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GuestsController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(List<ResponseGuestJson>), StatusCodes.Status200OK)]
	public async Task<IActionResult> Search(
		[FromQuery] string? term,
		[FromServices] ISearchGuestsUseCase useCase,
		CancellationToken cancellationToken)
	{
		return Ok(await useCase.Execute(term, cancellationToken));
	}

	[HttpPost]
	[ProducesResponseType(typeof(ResponseGuestJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register(
		[FromBody] RequestRegisterGuestJson request,
		[FromServices] IRegisterGuestUseCase useCase,
		CancellationToken cancellationToken)
	{
		var response = await useCase.Execute(request, cancellationToken);
		return StatusCode(StatusCodes.Status201Created, response);
	}
}
