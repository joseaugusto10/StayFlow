using StayFlow.Communication.Responses;
using CommunicationEnums = StayFlow.Communication.Enums;

namespace StayFlow.Application.Mappings;

public static class UserMapper
{
	public static ResponseUserProfileJson ToProfileResponse(Domain.Entities.User user)
	{
		return new ResponseUserProfileJson
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = (CommunicationEnums.UserRole)user.Role,
			HotelName = user.Hotel?.Name ?? string.Empty,
			HotelCity = user.Hotel?.City ?? string.Empty
		};
	}

	public static ResponseStaffMemberJson ToStaffResponse(Domain.Entities.User user)
	{
		return new ResponseStaffMemberJson
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = (CommunicationEnums.UserRole)user.Role,
			CreatedAt = user.CreatedAt
		};
	}
}
