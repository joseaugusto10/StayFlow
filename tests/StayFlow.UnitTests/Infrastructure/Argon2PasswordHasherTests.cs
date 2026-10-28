using StayFlow.Infrastructure.Security.PasswordHashing;

namespace StayFlow.UnitTests.Infrastructure;

public class Argon2PasswordHasherTests
{
	private readonly Argon2PasswordHasher _hasher = new();

	[Fact]
	public void Hash_Verifies_With_Correct_Password()
	{
		var hash = _hasher.HashPassword("Str0ng@Pass");

		_hasher.VerifyPassword("Str0ng@Pass", hash).ShouldBeTrue();
		_hasher.VerifyPassword("wrong", hash).ShouldBeFalse();
	}

	[Fact]
	public void Same_Password_Produces_Different_Hashes_Because_Of_Random_Salt()
	{
		_hasher.HashPassword("Str0ng@Pass").ShouldNotBe(_hasher.HashPassword("Str0ng@Pass"));
	}

	[Fact]
	public void Malformed_Hash_Returns_False_Instead_Of_Throwing()
	{
		_hasher.VerifyPassword("x", "not-base64!").ShouldBeFalse();
	}
}
