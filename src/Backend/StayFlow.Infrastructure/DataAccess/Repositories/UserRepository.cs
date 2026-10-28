using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Repositories.User;

namespace StayFlow.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
	private readonly StayFlowDbContext _dbContext;

	public UserRepository(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

	public async Task<bool> ExistUserWithEmail(string email) =>
		await _dbContext.Users.AnyAsync(user => user.Email == email);

	public async Task<bool> ExistActiveUserWithId(Guid userId) =>
		await _dbContext.Users.AnyAsync(user => user.IsActive && user.Id == userId);

	public async Task<User?> GetByEmail(string email) =>
		await _dbContext.Users
			.AsNoTracking()
			.SingleOrDefaultAsync(user => user.IsActive && user.Email == email);

	public async Task<IList<User>> GetByHotel(Guid hotelId, CancellationToken cancellationToken = default) =>
		await _dbContext.Users
			.AsNoTracking()
			.Where(user => user.HotelId == hotelId && user.IsActive)
			.OrderBy(user => user.Role).ThenBy(user => user.Name)
			.ToListAsync(cancellationToken);
}
