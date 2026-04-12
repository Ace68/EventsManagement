using EventsManagement.Events.Domain.CommandHandlers;
using EventsManagement.Events.SharedKernel.Messages.Commands;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Messages.Commands;

namespace EventsManagement.Events.Domain;

public static class DomainHelper
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandlerAsync<CreateCommunityEvent>, CreateCommunityEventCommandHandler>();
        
        return services;
    }
}