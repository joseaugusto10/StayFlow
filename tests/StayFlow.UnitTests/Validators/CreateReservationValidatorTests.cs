using StayFlow.Application.UseCases.Reservation.Create;
using StayFlow.Exception;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.Validators;

public class CreateReservationValidatorTests
{
	private readonly CreateReservationValidator _validator = new(new FixedDateProvider());

	[Fact]
	public void Valid_Request_Passes()
	{
		_validator.Validate(Builders.CreateReservation(Guid.NewGuid(), Guid.NewGuid())).IsValid.ShouldBeTrue();
	}

	[Fact]
	public void Check_Out_Before_Check_In_Fails()
	{
		var request = Builders.CreateReservation(Guid.NewGuid(), Guid.NewGuid());
		request.CheckOutDate = request.CheckInDate;

		_validator.Validate(request).Errors
			.ShouldContain(error => error.ErrorMessage == ResourceMessagesException.VALIDATION_DATES_INVALID);
	}

	[Fact]
	public void Check_In_In_The_Past_Fails()
	{
		var request = Builders.CreateReservation(Guid.NewGuid(), Guid.NewGuid());
		request.CheckInDate = FixedDateProvider.MONDAY.AddDays(-1);

		_validator.Validate(request).Errors
			.ShouldContain(error => error.ErrorMessage == ResourceMessagesException.VALIDATION_CHECKIN_IN_PAST);
	}

	[Fact]
	public void Stay_Longer_Than_30_Nights_Fails()
	{
		var request = Builders.CreateReservation(Guid.NewGuid(), Guid.NewGuid(), nights: 31);

		_validator.Validate(request).Errors
			.ShouldContain(error => error.ErrorMessage == ResourceMessagesException.VALIDATION_STAY_TOO_LONG);
	}
}
