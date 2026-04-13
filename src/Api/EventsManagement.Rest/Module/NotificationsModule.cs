using EventsManagement.Notifications.Facade;
using EventsManagement.Notifications.Facade.Endpoints;

namespace EventsManagement.Rest.Module;

public class NotificationsModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddNotifications(builder.Configuration);
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapNotificationsEndpoints();
        
        return app;
    }
}