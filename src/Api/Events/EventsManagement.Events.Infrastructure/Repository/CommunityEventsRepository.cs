using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.Entities.Entities;
using EventsManagement.Events.Entities.Helpers;
using EventsManagement.Notifications.Facade;
using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Events.Infrastructure.Repository;

public class CommunityEventsRepository(EventsManagementContext eventsManagementContext,
    INotificationsFacade notificationsFacade,
    ILoggerFactory loggerFactory) : IEventsManagementRepository<CommunityEvent>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<CommunityEventsRepository>();
    
    public Task<CommunityEvent> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(CommunityEvent entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            CommunityEventDto communityEventDto = entity.ToDto();
            if (eventsManagementContext.Database.ProviderName?.Contains("InMemory") != true)
            {
                await using var transaction = await eventsManagementContext.Database.BeginTransactionAsync(cancellationToken);
                await AddEntityAsync(communityEventDto, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                // Skip transaction for InMemory provider
                await AddEntityAsync(communityEventDto, cancellationToken);
            }

            AddCommunityEventToSchedulerJson jsonBody = new()
            {
                EventId = communityEventDto.Id.ToString(),
                EventName = communityEventDto.EventName,
                EventDate = communityEventDto.EventDate,
                EventVenue = communityEventDto.EventVenue
            };
            await notificationsFacade.AddCommunityEventToSchedulerAsync(jsonBody, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public Task UpdateAsync(CommunityEvent entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CommunityEvent entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async Task AddEntityAsync(CommunityEventDto dto, CancellationToken cancellationToken)
    {
        var dbSet = eventsManagementContext.Set<CommunityEventDto>();
        await dbSet.AddAsync(dto, cancellationToken);
        await eventsManagementContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task UpdateEntityAsync(CommunityEventDto dto, CancellationToken cancellationToken)
    {
        var dbSet = eventsManagementContext.Set<CommunityEventDto>();
        dbSet.Update(dto);
        await eventsManagementContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task DeleteEntityAsync(CommunityEventDto dto, CancellationToken cancellationToken)
    {
        eventsManagementContext.Set<CommunityEventDto>().Remove(dto);
        await eventsManagementContext.SaveChangesAsync(cancellationToken);
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