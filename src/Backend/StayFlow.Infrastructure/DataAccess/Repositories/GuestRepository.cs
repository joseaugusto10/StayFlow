using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Repositories.Guest;

namespace StayFlow.Infrastructure.DataAccess.Repositories;

internal sealed class GuestRepository : IGuestWriteOnlyRepository, IGuestReadOnlyRepository
{
	private readonly StayFlowDbContext _dbContext;

	public GuestRepository(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(Guest guest) => await _dbContext.Guests.AddAsync(guest);

	public async Task<IList<Guest>> Search(Guid hotelId, string? term, int take, CancellationToken cancellationToken = default)
	{
		var query = _dbContext.Guests
			.AsNoTracking()
			.Where(guest => guest.HotelId == hotelId && guest.IsActive);

		if (string.IsNullOrWhiteSpace(term) == false)
		{
			var pattern = $"%{term}%";
			query = query.Where(guest =>
				EF.Functions.Like(guest.Name, pattern) ||
				EF.Functions.Like(guest.Email, pattern) ||
				EF.Functions.Like(guest.Document, pattern));
		}

		return await query.OrderBy(guest => guest.Name).Take(take).ToListAsync(cancellationToken);
	}

	public async Task<Guest?> GetById(Guid hotelId, Guid guestId, CancellationToken cancellationToken = default) =>
		await _dbContext.Guests
			.AsNoTracking()
			.FirstOrDefaultAsync(guest => guest.HotelId == hotelId && guest.Id == guestId && guest.IsActive, cancellationToken);

	public async Task<bool> ExistGuestWithDocument(Guid hotelId, string document, CancellationToken cancellationToken = default) =>
		await _dbContext.Guests.AnyAsync(guest => guest.HotelId == hotelId && guest.Document == document, cancellationToken);
}
