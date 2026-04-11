using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Events.SharedKernel.Messages.Commands;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;
using Lena.Core;
using Muflone.Messages.Commands;

namespace EventsManagement.Events.Facade;

internal class EventsFacade(ICommandHandlerAsync<CreateCommunityEvent> createCommunityEventCommandHandler) : IEventsFacade
{
    public async Task<Result<string>> CreateCommunityEventAsync(CreateCommunityEventJson body, CancellationToken cancellationToken)
    {
        CommunityEventId communityEventId = new (Guid.CreateVersion7().ToString());
        CreateCommunityEvent command = new(communityEventId,
            new EventName(body.EventName),
            new EventDescription(body.Description),
            new EventVenue(body.Venue),
            body.Organizers.Select(o => new EventOrganizer(o)).ToList(),
            new EventDate(body.Date),
            Guid.CreateVersion7());
        
        await createCommunityEventCommandHandler.HandleAsync(command, cancellationToken);
        
        return Result<string>.Success(communityEventId.Value);
    }
}