using EventsManagement.Notifications.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagement.Notifications.Infrastructure.Mapping;

public class EventsSchedulerDtoMapping : IEntityTypeConfiguration<EventsSchedulerDto>
{
    public void Configure(EntityTypeBuilder<EventsSchedulerDto> builder)
    {
        builder.ToTable("EventsScheduler", "dbo");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired()
            .HasMaxLength(36);
        
        builder.Property(t => t.EventName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.EventVenue)
            .IsRequired()
            .HasMaxLength(100);
    }
}