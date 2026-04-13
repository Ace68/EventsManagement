using EventsManagement.Notifications.Domain;
using EventsManagement.Notifications.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Notifications.Facade;

public static class NotificationsHelper
{
    public static IServiceCollection AddNotifications(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<INotificationsFacade, NotificationFacade>();

        services.AddDomain();
        services.AddInfrastructure(configuration);
        
        return services;
    }
}