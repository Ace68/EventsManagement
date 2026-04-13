using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.ReadModel.EventHandlers;
using EventsManagement.Notifications.ReadModel.Queries;
using EventsManagement.Notifications.ReadModel.QueryHandlers;
using EventsManagement.Notifications.SharedKernel.Messages.Commands;
using EventsManagement.Notifications.SharedKernel.Messages.Queries;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using EventsManagements.InMemoryBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace EventsManagement.Notifications.ReadModel;

public static class ReadModelHelper
{
    public static IServiceCollection AddReadModel(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IEventsManagementQuery<EventsSchedulerDto>, EventsSchedulerQuery>();
        services.AddScoped<IQueryHandlerAsync<GetEventsScheduler, EventsSchedulerDto>, GetEventsSchedulerQueryHandler>();
        
        var eventhubParameters = configuration.GetSection("EventsManagement:EventHub").Get<EventHubParameters>();
        services.AddSingleton<CommunityEventHubHandler>(sp => 
            new CommunityEventHubHandler(
                eventhubParameters!,
                sp.GetRequiredService<IServiceBus>()));
        services.AddHostedService<EventHubListenerHostedService>();
        
        return services;
    }
}