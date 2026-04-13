using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.Entities.Entities;
using EventsManagement.Notifications.Entities.Helpers;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Notifications.Infrastructure.Repository;

public class EventsSchedulerRepository(NotificationsContext notificationsContext,
    ILoggerFactory loggerFactory) : IEventsManagementRepository<EventsScheduler>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<EventsSchedulerRepository>();
    
    public Task<EventsScheduler> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(EventsScheduler entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            EventsSchedulerDto eventsSchedulerDto = entity.ToDto();
            if (notificationsContext.Database.ProviderName?.Contains("InMemory") != true)
            {
                await using var transaction = await notificationsContext.Database.BeginTransactionAsync(cancellationToken);
                await AddEntityAsync(eventsSchedulerDto, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                // Skip transaction for InMemory provider
                await AddEntityAsync(eventsSchedulerDto, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public Task UpdateAsync(EventsScheduler entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(EventsScheduler entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    private async Task AddEntityAsync(EventsSchedulerDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var dbSet = notificationsContext.Set<EventsSchedulerDto>();
        await dbSet.AddAsync(dto, cancellationToken);
        await notificationsContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task UpdateEntityAsync(EventsSchedulerDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var dbSet = notificationsContext.Set<EventsSchedulerDto>();
        dbSet.Update(dto);
        await notificationsContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task DeleteEntityAsync(EventsSchedulerDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        notificationsContext.Set<EventsSchedulerDto>().Remove(dto);
        await notificationsContext.SaveChangesAsync(cancellationToken);
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

    ~EventsSchedulerRepository()
    {
        Dispose(false);
    }

    #endregion
}