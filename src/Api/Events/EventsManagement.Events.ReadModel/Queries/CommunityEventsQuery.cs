using System.Linq.Expressions;
using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.Infrastructure;
using EventsManagement.Shared.Exceptions;
using EventsManagement.Shared.Persister;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Events.ReadModel.Queries;

internal class CommunityEventsQuery(EventsManagementContext eventsManagementContext,
    ILoggerFactory loggerFactory) : IEventsManagementQuery<CommunityEventDto>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<CommunityEventsQuery>();
    
    public async Task<PagedResult<CommunityEventDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var queryable = eventsManagementContext.Set<CommunityEventDto>()
                .Include(c => c.EventOrganizers)
                .Where(a => a.Id.Equals(id));
            var result = await queryable.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return new PagedResult<CommunityEventDto>(new List<CommunityEventDto>
                   {
                       result!
                   }, 0, 0, 1) ??
                   throw new EntityNotFoundException(
                       $"Entity of type {nameof(CommunityEventDto)} with id {id} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<PagedResult<CommunityEventDto>> GetByFilterAsync(Expression<Func<CommunityEventDto, bool>>? query, int page, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (--page < 0)
            page = 0;

        try
        {
            var queryable = query != null
                ? eventsManagementContext.Set<CommunityEventDto>()
                    .Include(c => c.EventOrganizers)
                    .Where(query)
                : eventsManagementContext.Set<CommunityEventDto>()
                    .Include(c => c.EventOrganizers);
                    
            var count = await queryable.CountAsync(cancellationToken: cancellationToken);
            var results = await queryable.Skip(page * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            return new PagedResult<CommunityEventDto>(results, page, pageSize, count);
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

    ~CommunityEventsQuery()
    {
        Dispose(false);
    }

    #endregion
}