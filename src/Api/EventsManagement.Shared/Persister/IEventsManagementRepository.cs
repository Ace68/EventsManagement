namespace EventsManagement.Shared.Persister;

public interface IEventsManagementRepository : IDisposable
{
    Task<TAggregate> GetByIdAsync<TAggregate>(string id, CancellationToken cancellationToken) where TAggregate : class;
    Task AddAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class;
    Task UpdateAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class;
    Task DeleteAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class;
}