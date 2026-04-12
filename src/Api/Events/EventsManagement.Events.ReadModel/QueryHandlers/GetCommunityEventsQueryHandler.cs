using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.SharedKernel.Messages.Queries;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;

namespace EventsManagement.Events.ReadModel.QueryHandlers;

public class GetCommunityEventsQueryHandler(IEventsManagementQuery<CommunityEventDto> communityEventsQuery) 
    : EventsManagementQueryHandlerAsync<GetCommunityEvents, CommunityEventDto>
{
    public override async Task<PagedResult<CommunityEventDto>> HandleAsync(GetCommunityEvents query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await communityEventsQuery.GetByFilterAsync(null, query.PageNumber.Value, query.PageSize.Value,
            cancellationToken);
    }
}