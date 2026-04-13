using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Notifications.Infrastructure;

public static class InfrastructureHelper
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<NotificationsContext>(options => 
            options.UseSqlServer(configuration["EventsManagement:SqlServer:ConnectionString"]!));
        
        return services;
    }
}