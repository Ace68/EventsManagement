using EventsManagement.Rest.Module;

var builder = WebApplication.CreateBuilder(args);

// Explicit composition-root pattern for better control and visibility of module registration and configuration
builder.RegisterModules([
    new CorsModule(),
    new LoggingModule(),
    new InfrastructureModule(),
    new OpenApiModule(),
    new EventsModule(),
    new AttendeesModule(),
    new NotificationsModule()
]);

var app = builder.Build();

app.ConfigureModules();

await app.RunAsync();
