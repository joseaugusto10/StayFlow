using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Repositories.User;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Domain.Security.Tokens;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.User.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly IAccessTokenGenerator _accessTokenGenerator;

	public LoginWithEmailAndPasswordUseCase(
		IPasswordHasher passwordHasher,
		IUserReadOnlyRepository userReadOnlyRepository,
		IAccessTokenGenerator accessTokenGenerator)
	{
		_passwordHasher = passwordHasher;
		_userReadOnlyRepository = userReadOnlyRepository;
		_accessTokenGenerator = accessTokenGenerator;
	}

	public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
	{
		var email = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;

		var user = await _userReadOnlyRepository.GetByEmail(email);
		if (user is null)
			throw new InvalidLoginException();

		var isPasswordValid = _passwordHasher.VerifyPassword(request.Password ?? string.Empty, user.PasswordHash);
		if (isPasswordValid == false)
			throw new InvalidLoginException();

		return new ResponseRegisteredUserJson
		{
			Name = user.Name,
			Tokens = new ResponseTokensJson
			{
				AccessToken = _accessTokenGenerator.Generate(user)
			}
		};
	}
}
