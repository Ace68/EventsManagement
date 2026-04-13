using System.Globalization;
using EventsManagement.Events.Domain;
using EventsManagement.Events.Infrastructure;
using EventsManagement.Events.ReadModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Events.Facade;

public static class EventsHelper
{
    public static IServiceCollection AddEvents(this IServiceCollection services,
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
        
        services.AddScoped<IEventsFacade, EventsFacade>();

        services.AddInfrastructure(configuration);
        services.AddDomain();
        services.AddReadModel();
        
        return services;
    }
}