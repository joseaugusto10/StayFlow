using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories.User;

namespace StayFlow.Application.UseCases.User.Staff.GetAll;

public class GetStaffUseCase : IGetStaffUseCase
{
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;

	public GetStaffUseCase(IUserReadOnlyRepository userReadOnlyRepository, ILoggedUser loggedUser)
	{
		_userReadOnlyRepository = userReadOnlyRepository;
		_loggedUser = loggedUser;
	}

	public async Task<List<ResponseStaffMemberJson>> Execute(CancellationToken cancellationToken = default)
	{
		var users = await _userReadOnlyRepository.GetByHotel(_loggedUser.GetHotelId(), cancellationToken);
		return users.Select(UserMapper.ToStaffResponse).ToList();
	}
}
