using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Attendees.Facade;

public static class AttendeesHelper
{
    public static IServiceCollection AddAttendees(this IServiceCollection services)
    {
        // services.AddScoped<IAttendeesFacade, AttendeesFacade>();
        return services;
    }
}