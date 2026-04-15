using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.ReadModel.Queries;
using EventsManagement.Notifications.ReadModel.QueryHandlers;
using EventsManagement.Notifications.SharedKernel.Messages.Queries;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Notifications.ReadModel;

public static class ReadModelHelper
{
    public static IServiceCollection AddReadModel(this IServiceCollection services)
    {
        services.AddScoped<IEventsManagementQuery<EventsSchedulerDto>, EventsSchedulerQuery>();
        services.AddScoped<IQueryHandlerAsync<GetEventsScheduler, EventsSchedulerDto>, GetEventsSchedulerQueryHandler>();
        
        return services;
    }
}