using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Events.Facade.Endpoints;

public static class EventsEndpoints
{
    public static WebApplication MapEventsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/events")
            .WithTags("Events");

        group.MapPost("/", HandlePostCommunityEvent)
            .AddEndpointFilter<ValidationFilter<CreateCommunityEventJson>>()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new community event")
            .WithDescription(
                "Creates a new community event. This endpoint is used to add a new community event.")
            .WithName("CreateEvent");
        
        group.MapGet("/", HandleGetCommunityEvents)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get community events")
            .WithDescription(
                "Retrieves a list of community events. This endpoint is used to get a paginated list of community events.")
            .WithName("GetEvents");
        
        return app;
    }

    private static async Task<IResult> HandlePostCommunityEvent(
        IEventsFacade eventsFacade,
        CreateCommunityEventJson body,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var createCommunityEvent = await eventsFacade.CreateCommunityEventAsync(body, cancellationToken);
        
        return createCommunityEvent.Match<IResult>(
            success =>
            {
                createCommunityEvent.TryGetValue(out string eventId);
                return Results.Created($"/v1/events/{eventId}", success);
            }, 
            Results.BadRequest);
    }
    
    private static async Task<IResult> HandleGetCommunityEvents(
        IEventsFacade eventsFacade,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var communityEvent = await eventsFacade.GetCommunityEventsAsync(page, pageSize, cancellationToken);
        
        return communityEvent.Match(
            Results.Ok, 
            Results.NotFound);
    }
}