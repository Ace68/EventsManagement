using EventsManagement.Shared.Persister;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;

namespace EventsManagement.Shared.Handlers;

public abstract class EventsManagementCommandHandler<TCommand> : ICommandHandlerAsync<TCommand> where TCommand : class, ICommand
{
    protected readonly IEventsManagementRepository Repository;
    protected readonly ILogger Logger;

    protected EventsManagementCommandHandler(IEventsManagementRepository repository, ILoggerFactory loggerFactory)
    {
        Repository = repository;
        Logger = loggerFactory.CreateLogger(GetType());
    }

    public abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);

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

    ~EventsManagementCommandHandler()
    {
        Dispose(false);
    }

    #endregion
}