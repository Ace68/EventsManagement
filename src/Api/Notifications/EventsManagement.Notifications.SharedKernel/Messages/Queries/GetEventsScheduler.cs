using EventsManagement.Shared.CustomTypes;
using EventsManagement.Shared.Handlers;

namespace EventsManagement.Notifications.SharedKernel.Messages.Queries;

public class GetEventsScheduler(PageNumber pageNumber, 
    PageSize pageSize) : Query
{
    public PageNumber PageNumber { get; init; } = pageNumber;
    public PageSize PageSize { get; init; } = pageSize;
}