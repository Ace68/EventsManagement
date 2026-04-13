using System.Linq.Expressions;
using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.Infrastructure;
using EventsManagement.Shared.Exceptions;
using EventsManagement.Shared.Persister;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Notifications.ReadModel.Queries;

public class EventsSchedulerQuery(NotificationsContext notificationsContext,
    ILoggerFactory loggerFactory) : IEventsManagementQuery<EventsSchedulerDto>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<EventsSchedulerQuery>();
    
    public async Task<PagedResult<EventsSchedulerDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var queryable = notificationsContext.Set<EventsSchedulerDto>()
                .Where(a => a.Id.Equals(id));
            var result = await queryable.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return new PagedResult<EventsSchedulerDto>(new List<EventsSchedulerDto>
                   {
                       result!
                   }, 0, 0, 1) ??
                   throw new EntityNotFoundException(
                       $"Entity of type {nameof(EventsSchedulerDto)} with id {id} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<PagedResult<EventsSchedulerDto>> GetByFilterAsync(Expression<Func<EventsSchedulerDto, bool>>? query, int page, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (--page < 0)
            page = 0;

        try
        {
            var queryable = query != null
                ? notificationsContext.Set<EventsSchedulerDto>()
                    .Where(query)
                : notificationsContext.Set<EventsSchedulerDto>();
                    
            var count = await queryable.CountAsync(cancellationToken: cancellationToken);
            var results = await queryable.Skip(page * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            return new PagedResult<EventsSchedulerDto>(results, page, pageSize, count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
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

    ~EventsSchedulerQuery()
    {
        Dispose(false);
    }

    #endregion
}