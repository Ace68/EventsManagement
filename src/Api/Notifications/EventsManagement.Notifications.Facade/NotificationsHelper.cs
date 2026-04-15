using System.Globalization;
using EventsManagement.Notifications.Domain;
using EventsManagement.Notifications.Facade.EventHandlers;
using EventsManagement.Notifications.Infrastructure;
using EventsManagement.Notifications.ReadModel;
using EventsManagements.InMemoryBroker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Persistence;

namespace EventsManagement.Notifications.Facade;

public static class NotificationsHelper
{
    public static IServiceCollection AddNotifications(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidation();
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                if (context.ProblemDetails is HttpValidationProblemDetails validationProblemDetails)
                {
                    context.ProblemDetails.Detail =
                        $"Error(s) occurred: {validationProblemDetails.Errors.Values.Sum(x => x.Length)}";
                }

                context.ProblemDetails.Extensions.TryAdd("timestamp",
                    DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
            };
        });
        
        services.AddScoped<INotificationsFacade, NotificationFacade>();

        services.AddDomain();
        services.AddInfrastructure(configuration);
        services.AddReadModel();
        
        var eventhubParameters = configuration.GetSection("EventsManagement:EventHub").Get<EventHubParameters>();
        services.AddSingleton<CommunityEventHubHandler>(sp => 
            new CommunityEventHubHandler(
                eventhubParameters!,
                sp.GetRequiredService<IServiceBus>()));
        services.AddHostedService<EventHubListenerHostedService>();
        
        return services;
    }
}