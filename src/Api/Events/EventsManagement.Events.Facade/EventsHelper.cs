using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Events.Facade;

public static class EventsHelper
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IEventsFacade, EventsFacade>();
        return services;
    }
}