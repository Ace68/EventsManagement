using EventsManagement.Events.Domain;
using EventsManagement.Events.Infrastructure;
using EventsManagement.Events.ReadModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Events.Facade;

public static class EventsHelper
{
    public static IServiceCollection AddEvents(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IEventsFacade, EventsFacade>();

        services.AddInfrastructure(configuration);
        services.AddDomain();
        services.AddReadModel();
        
        return services;
    }
}