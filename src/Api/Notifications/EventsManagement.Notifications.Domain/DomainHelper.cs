using EventsManagement.Notifications.Domain.CommandHandlers;
using EventsManagement.Notifications.SharedKernel.Messages.Commands;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Messages.Commands;

namespace EventsManagement.Notifications.Domain;

public static class DomainHelper
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandlerAsync<AddCommunityEventToScheduler>, AddCommunityEventCommandHandlerAsync>();
        
        return services;
    }
}