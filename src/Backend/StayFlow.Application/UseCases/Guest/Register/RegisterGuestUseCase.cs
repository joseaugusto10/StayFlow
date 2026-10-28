using FluentValidation.Results;
using StayFlow.Application.Mappings;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Guest;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Guest.Register;

public class RegisterGuestUseCase : IRegisterGuestUseCase
{
	private readonly IGuestWriteOnlyRepository _guestWriteOnlyRepository;
	private readonly IGuestReadOnlyRepository _guestReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterGuestUseCase(
		IGuestWriteOnlyRepository guestWriteOnlyRepository,
		IGuestReadOnlyRepository guestReadOnlyRepository,
		ILoggedUser loggedUser,
		IUnitOfWork unitOfWork)
	{
		_guestWriteOnlyRepository = guestWriteOnlyRepository;
		_guestReadOnlyRepository = guestReadOnlyRepository;
		_loggedUser = loggedUser;
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseGuestJson> Execute(RequestRegisterGuestJson request, CancellationToken cancellationToken = default)
	{
		var hotelId = _loggedUser.GetHotelId();

		var result = new RegisterGuestValidator().Validate(request);
		if (result.IsValid)
		{
			var document = GuestMapper.NormalizeDocument(request.Document);
			if (await _guestReadOnlyRepository.ExistGuestWithDocument(hotelId, document, cancellationToken))
				result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_GUEST_DOCUMENT_ALREADY_EXISTS));
		}
		result.ThrowIfInvalid();

		var guest = GuestMapper.RequestToEntity(request, hotelId);

		await _guestWriteOnlyRepository.Add(guest);
		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);

		return GuestMapper.ToResponse(guest);
	}
}
