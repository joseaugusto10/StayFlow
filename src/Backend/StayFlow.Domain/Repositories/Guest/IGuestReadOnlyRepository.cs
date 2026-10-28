namespace StayFlow.Domain.Repositories.Guest;

public interface IGuestReadOnlyRepository
{
	Task<IList<Entities.Guest>> Search(Guid hotelId, string? term, int take, CancellationToken cancellationToken = default);
	Task<Entities.Guest?> GetById(Guid hotelId, Guid guestId, CancellationToken cancellationToken = default);
	Task<bool> ExistGuestWithDocument(Guid hotelId, string document, CancellationToken cancellationToken = default);
}
