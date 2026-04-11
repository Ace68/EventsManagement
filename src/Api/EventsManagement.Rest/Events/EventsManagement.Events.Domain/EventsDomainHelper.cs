using EventsManagement.Events.Domain.CommandHandlers;
using EventsManagement.Events.SharedKernel.Messages.Commands;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Messages.Commands;

namespace EventsManagement.Events.Domain;

public static class EventsDomainHelper
{
    public static IServiceCollection AddEventsDomain(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandlerAsync<CreateCommunityEvent>, CreateCommunityEventCommandHandler>();
        
        return services;
    }
}