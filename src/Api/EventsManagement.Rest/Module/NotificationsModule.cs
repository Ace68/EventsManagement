using EventsManagement.Notifications.Facade;

namespace EventsManagement.Rest.Module;

public class NotificationsModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddNotifications();
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        return app;
    }
}