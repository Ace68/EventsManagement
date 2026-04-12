using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Persister;
using Lena.Core;

namespace EventsManagement.Events.Facade;

public interface IEventsFacade
{
    Task<Result<string>> CreateCommunityEventAsync(CreateCommunityEventJson body, CancellationToken cancellationToken);
    Task<Result<PagedResult<CommunityEventJson>>> GetCommunityEventsAsync(int page, int pageSize, CancellationToken cancellationToken);
}