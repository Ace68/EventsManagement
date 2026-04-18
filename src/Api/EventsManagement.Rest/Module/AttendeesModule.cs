using EventsManagement.Attendees.Facade;

namespace EventsManagement.Rest.Module;

public class AttendeesModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddAttendees(builder.Configuration);
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        return app;
    }
}