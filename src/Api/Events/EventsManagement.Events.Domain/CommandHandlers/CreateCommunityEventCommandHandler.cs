using EventsManagement.Events.Entities.Entities;
using EventsManagement.Events.SharedKernel.Messages.Commands;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Events.Domain.CommandHandlers;

public class CreateCommunityEventCommandHandler(IEventsManagementRepository<CommunityEvent> repository, ILoggerFactory loggerFactory)
    : EventsManagementCommandHandler<CreateCommunityEvent, CommunityEvent>(repository, loggerFactory)
{
    public override Task HandleAsync(CreateCommunityEvent command, CancellationToken cancellationToken = default)
    {
        var aggregate = CommunityEvent.Create(new CommunityEventId(command.AggregateId.Value), command.EventName,
            command.EventDescription, command.EventVenue, command.EventOrganizers, command.EventDate);
        
        return Repository.AddAsync(aggregate, cancellationToken);
    }
}