using Moq;
using StayFlow.Application.UseCases.User.Login.WithEmailAndPassword;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Repositories.User;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Domain.Security.Tokens;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.UnitTests.UseCases;

public class LoginWithEmailAndPasswordUseCaseTests
{
	private readonly Mock<IPasswordHasher> _hasher = new();
	private readonly Mock<IUserReadOnlyRepository> _users = new();
	private readonly Mock<IAccessTokenGenerator> _tokens = new();

	private LoginWithEmailAndPasswordUseCase CreateUseCase() => new(_hasher.Object, _users.Object, _tokens.Object);

	[Fact]
	public async Task Valid_Credentials_Return_Token_And_Normalize_Email()
	{
		var user = new User { Name = "Marina", Email = "marina@hotel.com", PasswordHash = "hash" };
		_users.Setup(repo => repo.GetByEmail("marina@hotel.com")).ReturnsAsync(user);
		_hasher.Setup(h => h.VerifyPassword("Str0ng@Pass", "hash")).Returns(true);
		_tokens.Setup(t => t.Generate(user)).Returns("jwt");

		var response = await CreateUseCase().Execute(new RequestLoginJson { Email = "  Marina@Hotel.com ", Password = "Str0ng@Pass" });

		response.Name.ShouldBe("Marina");
		response.Tokens.AccessToken.ShouldBe("jwt");
	}

	[Fact]
	public async Task Unknown_Email_Throws_Invalid_Login()
	{
		await Should.ThrowAsync<InvalidLoginException>(
			() => CreateUseCase().Execute(new RequestLoginJson { Email = "nobody@hotel.com", Password = "x" }));
	}

	[Fact]
	public async Task Wrong_Password_Throws_Invalid_Login()
	{
		_users.Setup(repo => repo.GetByEmail(It.IsAny<string>())).ReturnsAsync(new User { PasswordHash = "hash" });
		_hasher.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

		await Should.ThrowAsync<InvalidLoginException>(
			() => CreateUseCase().Execute(new RequestLoginJson { Email = "a@b.com", Password = "wrong" }));
	}
}
