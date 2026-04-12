using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.ReadModel.Queries;
using EventsManagement.Events.ReadModel.QueryHandlers;
using EventsManagement.Events.SharedKernel.Messages.Queries;
using EventsManagement.Shared.Handlers;
using EventsManagement.Shared.Persister;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagement.Events.ReadModel;

public static class ReadModelHelper
{
    public static IServiceCollection AddReadModel(this IServiceCollection services)
    {
        services.AddScoped<IEventsManagementQuery<CommunityEventDto>, CommunityEventsQuery>();
        services.AddScoped<IQueryHandlerAsync<GetCommunityEvents, CommunityEventDto>, GetCommunityEventsQueryHandler>();
        
        return services;
    }
}