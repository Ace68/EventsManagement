using EventsManagement.Shared.CustomTypes;
using EventsManagement.Shared.Handlers;

namespace EventsManagement.Events.SharedKernel.Messages.Queries;

public sealed class GetCommunityEvents(PageNumber pageNumber, 
    PageSize pageSize) : Query
{
    public PageNumber PageNumber { get; init; } = pageNumber;
    public PageSize PageSize { get; init; } = pageSize;
}