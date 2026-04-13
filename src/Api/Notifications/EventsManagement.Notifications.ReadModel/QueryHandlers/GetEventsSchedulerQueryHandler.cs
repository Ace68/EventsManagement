using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.SharedKernel.Messages.Queries;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;

namespace EventsManagement.Notifications.ReadModel.QueryHandlers;

public sealed class GetEventsSchedulerQueryHandler(IEventsManagementQuery<EventsSchedulerDto> eventsSchedulerQuery)
    : EventsManagementQueryHandlerAsync<GetEventsScheduler, EventsSchedulerDto>
{
    public override async Task<PagedResult<EventsSchedulerDto>> HandleAsync(GetEventsScheduler query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await eventsSchedulerQuery.GetByFilterAsync(null, query.PageNumber.Value, query.PageSize.Value,
            cancellationToken);
    }
}