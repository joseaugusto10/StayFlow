using FluentValidation.Results;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Hotel;
using StayFlow.Domain.Repositories.User;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly IHotelWriteOnlyRepository _hotelWriteOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterUserAccountUseCase(
		IPasswordHasher passwordHasher,
		IUserWriteOnlyRepository userWriteOnlyRepository,
		IUserReadOnlyRepository userReadOnlyRepository,
		IHotelWriteOnlyRepository hotelWriteOnlyRepository,
		IUnitOfWork unitOfWork)
	{
		_passwordHasher = passwordHasher;
		_userWriteOnlyRepository = userWriteOnlyRepository;
		_userReadOnlyRepository = userReadOnlyRepository;
		_hotelWriteOnlyRepository = hotelWriteOnlyRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(RequestRegisterUserAccountJson request)
	{
		await ValidateAndThrowOnFailures(request);

		var hotel = new Domain.Entities.Hotel
		{
			Name = request.HotelName.Trim(),
			City = request.City.Trim()
		};

		var user = new Domain.Entities.User
		{
			Name = request.Name.Trim(),
			Email = request.Email.Trim().ToLowerInvariant(),
			PasswordHash = _passwordHasher.HashPassword(request.Password),
			Role = UserRole.Manager,
			HotelId = hotel.Id
		};

		await _hotelWriteOnlyRepository.Add(hotel);
		await _userWriteOnlyRepository.Add(user);
		await _unitOfWork.CommitOrThrowOnConflict();
	}

	private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
	{
		var result = new RegisterUserAccountValidator().Validate(request);

		if (result.IsValid)
		{
			var emailExist = await _userReadOnlyRepository.ExistUserWithEmail(request.Email.Trim().ToLowerInvariant());
			if (emailExist)
				result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
		}

		result.ThrowIfInvalid();
	}
}
