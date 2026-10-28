using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Security.Tokens;

namespace StayFlow.Infrastructure.Tokens.Access;

internal sealed class JwtTokenHandler : IAccessTokenGenerator
{
	public const string HOTEL_CLAIM = "hotel_id";
	public const string ROLE_CLAIM = "role";

	private readonly uint _expirationTimeMinutes;
	private readonly string _signingKey;

	public JwtTokenHandler(uint expirationTimeMinutes, string signingKey)
	{
		_expirationTimeMinutes = expirationTimeMinutes;
		_signingKey = signingKey;
	}

	public string Generate(User user)
	{
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Name, user.Name),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ROLE_CLAIM, user.Role.ToString()),
			new(HOTEL_CLAIM, user.HotelId.ToString())
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
			SigningCredentials = new SigningCredentials(Credentials(), SecurityAlgorithms.HmacSha256),
			Subject = new ClaimsIdentity(claims)
		};

		return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
	}

	private SymmetricSecurityKey Credentials() => new(Encoding.UTF8.GetBytes(_signingKey));
}
