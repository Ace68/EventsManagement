using EventsManagement.Notifications.Entities.Entities;
using EventsManagement.Notifications.SharedKernel.Messages.Commands;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Notifications.Domain.CommandHandlers;

internal sealed class AddCommunityEventCommandHandlerAsync(IEventsManagementRepository<EventsScheduler> repository, ILoggerFactory loggerFactory)
     : EventsManagementCommandHandler<AddCommunityEventToScheduler, EventsScheduler>(repository, loggerFactory)
{
    public override Task HandleAsync(AddCommunityEventToScheduler command, CancellationToken cancellationToken = default)
    {
        
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = EventsScheduler.Create(new CommunityEventId(command.AggregateId.Value), command.EventName,
            command.EventVenue, command.EventDate);
        
        return Repository.AddAsync(aggregate, cancellationToken);   
    }
}