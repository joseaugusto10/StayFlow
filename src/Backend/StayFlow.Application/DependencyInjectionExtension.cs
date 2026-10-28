using Microsoft.Extensions.DependencyInjection;
using StayFlow.Application.UseCases.Dashboard;
using StayFlow.Application.UseCases.Guest.Register;
using StayFlow.Application.UseCases.Guest.Search;
using StayFlow.Application.UseCases.Reservation.Cancel;
using StayFlow.Application.UseCases.Reservation.CheckIn;
using StayFlow.Application.UseCases.Reservation.CheckOut;
using StayFlow.Application.UseCases.Reservation.Create;
using StayFlow.Application.UseCases.Reservation.GetById;
using StayFlow.Application.UseCases.Reservation.GetWithFilters;
using StayFlow.Application.UseCases.Room.ChangeStatus;
using StayFlow.Application.UseCases.Room.GetAll;
using StayFlow.Application.UseCases.Room.GetAvailability;
using StayFlow.Application.UseCases.Room.Register;
using StayFlow.Application.UseCases.Room.Update;
using StayFlow.Application.UseCases.User.Login.WithEmailAndPassword;
using StayFlow.Application.UseCases.User.Profile;
using StayFlow.Application.UseCases.User.Register;
using StayFlow.Application.UseCases.User.Staff.GetAll;
using StayFlow.Application.UseCases.User.Staff.Register;

namespace StayFlow.Application;

public static class DependencyInjectionExtension
{
	public static void AddApplication(this IServiceCollection services)
	{
		AddUserUseCases(services);
		AddRoomUseCases(services);
		AddGuestUseCases(services);
		AddReservationUseCases(services);

		services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
	}

	private static void AddUserUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
		services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
		services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
		services.AddScoped<IRegisterStaffUseCase, RegisterStaffUseCase>();
		services.AddScoped<IGetStaffUseCase, GetStaffUseCase>();
	}

	private static void AddRoomUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterRoomUseCase, RegisterRoomUseCase>();
		services.AddScoped<IUpdateRoomUseCase, UpdateRoomUseCase>();
		services.AddScoped<IChangeRoomStatusUseCase, ChangeRoomStatusUseCase>();
		services.AddScoped<IGetRoomsUseCase, GetRoomsUseCase>();
		services.AddScoped<IGetRoomAvailabilityUseCase, GetRoomAvailabilityUseCase>();
	}

	private static void AddGuestUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterGuestUseCase, RegisterGuestUseCase>();
		services.AddScoped<ISearchGuestsUseCase, SearchGuestsUseCase>();
	}

	private static void AddReservationUseCases(IServiceCollection services)
	{
		services.AddScoped<ICreateReservationUseCase, CreateReservationUseCase>();
		services.AddScoped<IGetReservationsWithFiltersUseCase, GetReservationsWithFiltersUseCase>();
		services.AddScoped<IGetReservationByIdUseCase, GetReservationByIdUseCase>();
		services.AddScoped<ICheckInReservationUseCase, CheckInReservationUseCase>();
		services.AddScoped<ICheckOutReservationUseCase, CheckOutReservationUseCase>();
		services.AddScoped<ICancelReservationUseCase, CancelReservationUseCase>();
	}
}
