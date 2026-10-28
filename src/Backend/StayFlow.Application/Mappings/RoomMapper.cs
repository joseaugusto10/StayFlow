using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Services;
using CommunicationEnums = StayFlow.Communication.Enums;
using DomainEnums = StayFlow.Domain.Enums;

namespace StayFlow.Application.Mappings;

public static class RoomMapper
{
	public static Domain.Entities.Room RequestToEntity(RequestRoomJson request, Guid hotelId)
	{
		var room = new Domain.Entities.Room { HotelId = hotelId };
		ApplyRequest(room, request);
		return room;
	}

	public static void ApplyRequest(Domain.Entities.Room room, RequestRoomJson request)
	{
		room.Number = request.Number.Trim();
		room.Floor = request.Floor;
		room.Type = (DomainEnums.RoomType)request.Type;
		room.Capacity = request.Capacity;
		room.NightlyRate = request.NightlyRate;
		room.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
		room.UpdatedAt = DateTime.UtcNow;
	}

	public static ResponseRoomJson ToResponse(Domain.Entities.Room room)
	{
		return new ResponseRoomJson
		{
			Id = room.Id,
			Number = room.Number,
			Floor = room.Floor,
			Type = (CommunicationEnums.RoomType)room.Type,
			Capacity = room.Capacity,
			NightlyRate = room.NightlyRate,
			Status = (CommunicationEnums.RoomStatus)room.Status,
			Description = room.Description
		};
	}

	public static ResponseQuoteJson ToResponse(StayQuote quote)
	{
		return new ResponseQuoteJson
		{
			Nights = quote.Nights,
			WeekendNights = quote.WeekendNights,
			Subtotal = quote.Subtotal,
			Discount = quote.Discount,
			Total = quote.Total
		};
	}
}
