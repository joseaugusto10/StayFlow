using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Security.Tokens;
using StayFlow.Infrastructure.DataAccess;
using StayFlow.Infrastructure.Tokens.Access;

namespace StayFlow.Infrastructure.Identity;

internal class LoggedUser : ILoggedUser
{
	private readonly IAccessTokenProvider _accessTokenProvider;
	private readonly StayFlowDbContext _dbContext;

	public LoggedUser(IAccessTokenProvider accessTokenProvider, StayFlowDbContext dbContext)
	{
		_accessTokenProvider = accessTokenProvider;
		_dbContext = dbContext;
	}

	public async Task<User> Get()
	{
		var userId = GetUserId();

		return await _dbContext.Users
			.AsNoTracking()
			.Include(user => user.Hotel)
			.FirstAsync(user => user.IsActive && user.Id == userId);
	}

	public Guid GetUserId() => ReadGuidClaim(JwtRegisteredClaimNames.Sub);

	public Guid GetHotelId() => ReadGuidClaim(JwtTokenHandler.HOTEL_CLAIM);

	private Guid ReadGuidClaim(string claimType)
	{
		var accessToken = _accessTokenProvider.GetToken();

		var jsonWebToken = new JsonWebTokenHandler().ReadJsonWebToken(accessToken);

		var claim = jsonWebToken.Claims.FirstOrDefault(claim => claim.Type == claimType);

		if (claim is null || Guid.TryParse(claim.Value, out var value) == false)
			throw new InvalidOperationException($"Invalid access token claim '{claimType}'.");

		return value;
	}
}
