using EventsManagement.Shared.ExternalContracts;
using Lena.Core;

namespace EventsManagement.Events.Facade;

public interface IEventsFacade
{
    Task<Result<string>> CreateCommunityEventAsync(CreateCommunityEventJson body, CancellationToken cancellationToken);
}