using EventsManagement.Shared.Persister;

namespace EventsManagement.Events.Infrastructure.Repository;

public class CommunityEventsRepository : IEventsManagementRepository
{
    public Task<TAggregate> GetByIdAsync<TAggregate>(string id, CancellationToken cancellationToken) where TAggregate : class
    {
        throw new NotImplementedException();
    }

    public Task AddAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync<TAggregate>(TAggregate entity, CancellationToken cancellationToken) where TAggregate : class
    {
        throw new NotImplementedException();
    }
    
    #region Dispose

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CommunityEventsRepository()
    {
        Dispose(false);
    }

    #endregion
}