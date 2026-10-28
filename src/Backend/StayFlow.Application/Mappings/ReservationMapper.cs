using StayFlow.Communication.Responses;
using CommunicationEnums = StayFlow.Communication.Enums;

namespace StayFlow.Application.Mappings;

public static class ReservationMapper
{
	public static ResponseReservationListItemJson ToListItem(Domain.Entities.Reservation reservation)
	{
		return new ResponseReservationListItemJson
		{
			Id = reservation.Id,
			Code = reservation.Code,
			GuestName = reservation.Guest?.Name ?? string.Empty,
			RoomNumber = reservation.Room?.Number ?? string.Empty,
			CheckInDate = reservation.CheckInDate,
			CheckOutDate = reservation.CheckOutDate,
			Nights = reservation.Nights,
			Guests = reservation.TotalGuests,
			TotalAmount = reservation.TotalAmount,
			Status = (CommunicationEnums.ReservationStatus)reservation.Status
		};
	}

	public static ResponseReservationDetailsJson ToDetails(Domain.Entities.Reservation reservation, DateOnly today)
	{
		return new ResponseReservationDetailsJson
		{
			Id = reservation.Id,
			Code = reservation.Code,
			CheckInDate = reservation.CheckInDate,
			CheckOutDate = reservation.CheckOutDate,
			Nights = reservation.Nights,
			Adults = reservation.Adults,
			Children = reservation.Children,
			TotalAmount = reservation.TotalAmount,
			Status = (CommunicationEnums.ReservationStatus)reservation.Status,
			Notes = reservation.Notes,
			CreatedAt = reservation.CreatedAt,
			CheckedInAt = reservation.CheckedInAt,
			CheckedOutAt = reservation.CheckedOutAt,
			CancelledAt = reservation.CancelledAt,
			Guest = GuestMapper.ToResponse(reservation.Guest),
			Room = RoomMapper.ToResponse(reservation.Room),
			CanCheckIn = reservation.CanCheckIn(today),
			CanCheckOut = reservation.CanCheckOut(),
			CanCancel = reservation.CanCancel()
		};
	}
}
