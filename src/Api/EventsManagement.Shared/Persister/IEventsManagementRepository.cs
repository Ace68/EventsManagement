namespace EventsManagement.Shared.Persister;

public interface IEventsManagementRepository<TAggregate> : IDisposable where TAggregate : class
{
    Task<TAggregate> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken);
    Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken);
    Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken);
}