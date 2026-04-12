using EventsManagement.Events.Entities.Entities;
using EventsManagement.Events.Infrastructure.Repository;
using EventsManagement.Shared.Persister;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Events.Infrastructure;

public static class InfrastructureHelper
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EventsManagementContext>(options => 
            options.UseSqlServer(configuration["EventsManagement:SqlServer:ConnectionString"]!));
        
        services.AddScoped<IEventsManagementRepository<CommunityEvent>, CommunityEventsRepository>();
        
        return services;
    }
}