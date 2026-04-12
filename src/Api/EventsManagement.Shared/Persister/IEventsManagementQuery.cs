using System.Linq.Expressions;

namespace EventsManagement.Shared.Persister;

public interface IEventsManagementQuery<TDto>  : IDisposable
    where TDto : DtoBase
{
    Task<PagedResult<TDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<PagedResult<TDto>> GetByFilterAsync(Expression<Func<TDto, bool>>? query, int page, int pageSize, CancellationToken cancellationToken);
}