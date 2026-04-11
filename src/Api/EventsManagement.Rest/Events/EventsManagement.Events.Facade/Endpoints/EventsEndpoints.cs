using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
}