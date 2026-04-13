using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.Entities.Helpers;
using EventsManagement.Notifications.SharedKernel.CustomTypes;
using EventsManagement.Notifications.SharedKernel.Messages.Commands;
using EventsManagement.Notifications.SharedKernel.Messages.Queries;
using EventsManagement.Shared.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Lena.Core;
using Muflone.Persistence;

namespace EventsManagement.Notifications.Facade;

internal class NotificationFacade(IServiceBus serviceBus,
    IQueryHandlerAsync<GetEventsScheduler, EventsSchedulerDto> eventsSchedulerQuery) : INotificationsFacade
{
    public async Task<Result<string>> AddCommunityEventToSchedulerAsync(AddCommunityEventToSchedulerJson body, CancellationToken cancellationToken = default)
    {
        AddCommunityEventToScheduler command = new(new CommunityEventId(body.EventId),
            new EventName(body.EventName),
            new EventVenue(body.EventVenue),
            new EventDate(body.EventDate));
        
        await serviceBus.SendAsync(command, cancellationToken);
        
        return Result.Success(command.AggregateId.Value);
    }

    public async Task<Result<PagedResult<EventsSchedulerJson>>> GetSchedulerAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        GetEventsScheduler query = new(new PageNumber(page), new PageSize(pageSize));
        PagedResult<EventsSchedulerDto> result = await eventsSchedulerQuery.HandleAsync(query, cancellationToken);
        
        return Result<PagedResult<EventsSchedulerJson>>.Success(new PagedResult<EventsSchedulerJson>(
            result.Results.Select(r => r.ToJson()).ToList(), result.Page, result.PageSize, result.TotalRecords));
    }
}