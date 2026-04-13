using EventsManagement.Notifications.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Notifications.Facade;

public static class NotificationsHelper
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddScoped<INotificationsFacade, NotificationFacade>();

        services.AddDomain();
        
        return services;
    }
}