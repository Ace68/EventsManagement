using EventsManagement.Attendees.Facade.EventHandlers;
using EventsManagement.Notifications.Facade.EventHandlers;
using EventsManagements.InMemoryBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Persistence;

namespace EventsManagement.Attendees.Facade;

public static class AttendeesHelper
{
    public static IServiceCollection AddAttendees(this IServiceCollection services,
        IConfiguration configuration)
    {
        var eventhubParameters = configuration.GetSection("EventsManagement:EventHub").Get<EventHubParameters>();
        services.AddSingleton<CommunityEventHubHandler>(sp => 
            new CommunityEventHubHandler(
                eventhubParameters!,
                sp.GetRequiredService<IServiceBus>()));
        services.AddHostedService<EventHubListenerHostedService>();
        
        return services;
    }
}