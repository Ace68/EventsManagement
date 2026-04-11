using EventsManagement.Events.Facade;
using EventsManagement.Events.Facade.Endpoints;

namespace EventsManagement.Rest.Module;

public class EventsModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddEvents();
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapEventsEndpoints();
        
        return app;
    }
}