using EventsManagement.Shared.ExternalContracts;
using Lena.Core;

namespace EventsManagement.Notifications.Facade;

public interface INotificationsFacade
{
    Task<Result<string>> AddCommunityEventToSchedulerAsync(AddCommunityEventToSchedulerJson body,
        CancellationToken cancellationToken = default);

    Task<Result<EventsSchedulerJson>> GetSchedulerAsync(int page, int pageSize, CancellationToken cancellationToken);
}