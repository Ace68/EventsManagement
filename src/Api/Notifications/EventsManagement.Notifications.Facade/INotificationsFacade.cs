using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Persister;
using Lena.Core;

namespace EventsManagement.Notifications.Facade;

public interface INotificationsFacade
{
    Task<Result<string>> AddCommunityEventToSchedulerAsync(AddCommunityEventToSchedulerJson body,
        CancellationToken cancellationToken = default);

    Task<Result<PagedResult<EventsSchedulerJson>>> GetSchedulerAsync(int page, int pageSize, CancellationToken cancellationToken);
}