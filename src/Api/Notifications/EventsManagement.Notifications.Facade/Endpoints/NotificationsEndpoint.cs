using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Notifications.Facade.Endpoints;

public static class NotificationsEndpoint
{
    public static WebApplication MapNotificationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/notifications")
            .WithTags("Events");
        
        group.MapGet("/", HandleGetScheduler)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get scheduler of community events")
            .WithDescription(
                "Retrieves the scheduler of the community events. This endpoint is used to get a paginated list of community events.")
            .WithName("GetScheduler");
        
        return app;
    }
    
    private static async Task<IResult> HandleGetScheduler(
        INotificationsFacade notificationsFacade,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var scheduler = await notificationsFacade.GetSchedulerAsync(page, pageSize, cancellationToken);
        
        return scheduler.Match(
            Results.Ok, 
            Results.NotFound);
    }
}