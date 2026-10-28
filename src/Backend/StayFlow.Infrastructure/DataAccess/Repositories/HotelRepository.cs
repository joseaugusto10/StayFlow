using StayFlow.Domain.Entities;
using StayFlow.Domain.Repositories.Hotel;

namespace StayFlow.Infrastructure.DataAccess.Repositories;

internal sealed class HotelRepository : IHotelWriteOnlyRepository
{
	private readonly StayFlowDbContext _dbContext;

	public HotelRepository(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(Hotel hotel) => await _dbContext.Hotels.AddAsync(hotel);
}
