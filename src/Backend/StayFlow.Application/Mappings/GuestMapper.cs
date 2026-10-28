using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.Mappings;

public static class GuestMapper
{
	public static Domain.Entities.Guest RequestToEntity(RequestRegisterGuestJson request, Guid hotelId)
	{
		return new Domain.Entities.Guest
		{
			HotelId = hotelId,
			Name = request.Name.Trim(),
			Document = NormalizeDocument(request.Document),
			Email = request.Email.Trim().ToLowerInvariant(),
			Phone = request.Phone.Trim(),
			Nationality = string.IsNullOrWhiteSpace(request.Nationality) ? null : request.Nationality.Trim()
		};
	}

	public static string NormalizeDocument(string document) =>
		new(document.Where(char.IsLetterOrDigit).ToArray());

	public static ResponseGuestJson ToResponse(Domain.Entities.Guest guest)
	{
		return new ResponseGuestJson
		{
			Id = guest.Id,
			Name = guest.Name,
			Document = guest.Document,
			Email = guest.Email,
			Phone = guest.Phone,
			Nationality = guest.Nationality
		};
	}
}
