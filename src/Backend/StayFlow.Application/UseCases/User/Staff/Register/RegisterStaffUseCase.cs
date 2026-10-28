using FluentValidation.Results;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.User;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.User.Staff.Register;

public class RegisterStaffUseCase : IRegisterStaffUseCase
{
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterStaffUseCase(
		IPasswordHasher passwordHasher,
		IUserWriteOnlyRepository userWriteOnlyRepository,
		IUserReadOnlyRepository userReadOnlyRepository,
		ILoggedUser loggedUser,
		IUnitOfWork unitOfWork)
	{
		_passwordHasher = passwordHasher;
		_userWriteOnlyRepository = userWriteOnlyRepository;
		_userReadOnlyRepository = userReadOnlyRepository;
		_loggedUser = loggedUser;
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseCreatedJson> Execute(RequestRegisterStaffJson request)
	{
		var email = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;

		var result = new RegisterStaffValidator().Validate(request);
		if (result.IsValid && await _userReadOnlyRepository.ExistUserWithEmail(email))
			result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
		result.ThrowIfInvalid();

		var user = new Domain.Entities.User
		{
			Name = request.Name.Trim(),
			Email = email,
			PasswordHash = _passwordHasher.HashPassword(request.Password),
			Role = UserRole.Receptionist,
			HotelId = _loggedUser.GetHotelId()
		};

		await _userWriteOnlyRepository.Add(user);
		await _unitOfWork.CommitOrThrowOnConflict();

		return new ResponseCreatedJson { Id = user.Id };
	}
}
