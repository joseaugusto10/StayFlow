using StayFlow.Application.UseCases.User.Register;
using StayFlow.Exception;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.Validators;

public class RegisterUserAccountValidatorTests
{
	[Fact]
	public void Valid_Request_Passes()
	{
		new RegisterUserAccountValidator().Validate(Builders.RegisterUser()).IsValid.ShouldBeTrue();
	}

	[Theory]
	[InlineData("short1!", nameof(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH))]
	[InlineData("nouppercase1!", nameof(ResourceMessagesException.VALIDATION_PASSWORD_UPPERCASE_REQUIRED))]
	[InlineData("NoNumber!!", nameof(ResourceMessagesException.VALIDATION_PASSWORD_NUMBER_REQUIRED))]
	[InlineData("NoSymbol123", nameof(ResourceMessagesException.VALIDATION_PASSWORD_SPECIAL_CHARACTER_REQUIRED))]
	public void Weak_Password_Fails(string password, string expectedKey)
	{
		var request = Builders.RegisterUser();
		request.Password = password;

		var result = new RegisterUserAccountValidator().Validate(request);

		result.IsValid.ShouldBeFalse();
		result.Errors.ShouldHaveSingleItem().ErrorMessage
			.ShouldBe(ResourceMessagesException.ResourceManager.GetString(expectedKey));
	}

	[Fact]
	public void Missing_Hotel_Name_Fails()
	{
		var request = Builders.RegisterUser();
		request.HotelName = string.Empty;

		var result = new RegisterUserAccountValidator().Validate(request);

		result.Errors.ShouldContain(error => error.ErrorMessage == ResourceMessagesException.VALIDATION_HOTEL_NAME_REQUIRED);
	}
}
