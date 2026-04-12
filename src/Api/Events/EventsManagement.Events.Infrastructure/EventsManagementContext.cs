using EventsManagement.Events.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventsManagement.Events.Infrastructure;

public class EventsManagementContext(DbContextOptions<EventsManagementContext> options) : DbContext(options)
{
    public DbSet<Entities.Dtos.CommunityEventDto> CommunityEvents { get; set; }
    public DbSet<Entities.Dtos.OrganizersDto> Organizers { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Logging configuration
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((_, level) => level == LogLevel.Information)
                .AddConsole();
        });

        optionsBuilder.UseLoggerFactory(loggerFactory);
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new CommunityEventDtoMapping());
        modelBuilder.ApplyConfiguration(new OrganizersDtoMapping());
        
        modelBuilder.Entity<Entities.Dtos.CommunityEventDto>()
            .HasMany(e => e.EventOrganizers)
            .WithOne(o => o.CommunityEvent)
            .HasForeignKey(o => o.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}