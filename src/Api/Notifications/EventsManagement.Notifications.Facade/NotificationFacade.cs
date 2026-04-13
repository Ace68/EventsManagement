using EventsManagement.Notifications.SharedKernel.CustomTypes;
using EventsManagement.Notifications.SharedKernel.Messages.Commands;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;
using Lena.Core;
using Muflone.Messages.Commands;

namespace EventsManagement.Notifications.Facade;

internal class NotificationFacade(ICommandHandlerAsync<AddCommunityEventToScheduler> addCommmunityEventToSchedulerCommandHandler) : INotificationsFacade
{
    public async Task<Result<string>> AddCommunityEventToScheduler(AddCommunityEventToSchedulerJson body, CancellationToken cancellationToken = default)
    {
        AddCommunityEventToScheduler command = new(new CommunityEventId(body.EventId),
            new EventName(body.EventName),
            new EventVenue(body.EventVenue),
            new EventDate(body.EventDate));
        
        await addCommmunityEventToSchedulerCommandHandler.HandleAsync(command, cancellationToken);
        
        return Result.Success(command.AggregateId.Value);
    }

    public Task<Result<EventsSchedulerJson>> GetSchedulerAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}