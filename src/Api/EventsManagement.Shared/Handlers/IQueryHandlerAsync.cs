using EventsManagement.Shared.Persister;

namespace EventsManagement.Shared.Handlers;

public interface IQueryHandlerAsync<in TQuery, TDto>
    where TQuery : IQuery
    where TDto : DtoBase
{
    Task<PagedResult<TDto>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}