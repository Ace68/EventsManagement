using EventsManagement.Shared.Persister;

namespace EventsManagement.Shared.Handlers;

public abstract class EventsManagementQueryHandlerAsync<TQuery, TDto> : IQueryHandlerAsync<TQuery, TDto>
    where TQuery : IQuery
    where TDto : DtoBase
{
    public abstract Task<PagedResult<TDto>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}