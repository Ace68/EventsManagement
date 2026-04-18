using EventsManagement.Shared.ExternalContracts;
using EventsManagement.Shared.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Notifications.Facade.Endpoints;

public static class NotificationsEndpoint
{
    public static WebApplication MapNotificationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/notifications")
            .WithTags("Notifications");
        
        group.MapPost("/", HandleAddToScheduler)
            .AddEndpointFilter<ValidationFilter<AddCommunityEventToSchedulerJson>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Adds a community event to scheduler")
            .WithDescription(
                "Adds a community event to the scheduler. This endpoint is used to add a community event to the scheduler.")
            .WithName("AddCommunityEventToScheduler");
        
        group.MapGet("/", HandleGetScheduler)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get scheduler of community events")
            .WithDescription(
                "Retrieves the scheduler of the community events. This endpoint is used to get a paginated list of community events.")
            .WithName("GetScheduler");
        
        return app;
    }
    
    private static async Task<IResult> HandleAddToScheduler(
        INotificationsFacade notificationsFacade,
        AddCommunityEventToSchedulerJson body,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var addToScheduler = await notificationsFacade.AddCommunityEventToSchedulerAsync(body, cancellationToken);
        
        return addToScheduler.Match(Results.Ok, Results.NotFound);
    }
    
    private static async Task<IResult> HandleGetScheduler(
        INotificationsFacade notificationsFacade,
        CancellationToken cancellationToken,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var scheduler = await notificationsFacade.GetSchedulerAsync(page, pageSize, cancellationToken);
        
        return scheduler.Match(
            Results.Ok, 
            Results.NotFound);
    }
}