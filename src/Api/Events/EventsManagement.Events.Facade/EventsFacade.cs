using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.Entities.Helpers;
using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Events.SharedKernel.Messages.Commands;
using EventsManagement.Events.SharedKernel.Messages.Queries;
using EventsManagement.Shared.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Lena.Core;
using Muflone.Messages.Commands;

namespace EventsManagement.Events.Facade;

internal class EventsFacade(ICommandHandlerAsync<CreateCommunityEvent> createCommunityEventCommandHandler,
    IQueryHandlerAsync<GetCommunityEvents, CommunityEventDto> getCommunityEventQueryHandler) : IEventsFacade
{
    public async Task<Result<string>> CreateCommunityEventAsync(CreateCommunityEventJson body, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
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

    public async Task<Result<PagedResult<CommunityEventJson>>> GetCommunityEventsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        GetCommunityEvents query = new(new PageNumber(page), new PageSize(pageSize));
        PagedResult<CommunityEventDto> result = await getCommunityEventQueryHandler.HandleAsync(query, cancellationToken);

        return Result<PagedResult<CommunityEventJson>>.Success(new PagedResult<CommunityEventJson>(
            result.Results.Select(r => r.ToJson()).ToList(), result.Page, result.PageSize, result.TotalRecords));
    }
}