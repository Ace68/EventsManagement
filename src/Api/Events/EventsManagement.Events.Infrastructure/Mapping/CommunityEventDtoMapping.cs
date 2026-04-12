using EventsManagement.Events.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagement.Events.Infrastructure.Mapping;

public class CommunityEventDtoMapping : IEntityTypeConfiguration<CommunityEventDto>
{
    public void Configure(EntityTypeBuilder<CommunityEventDto> builder)
    {
        builder.ToTable("CommunityEvents", "dbo");
        builder.HasKey(x => x.Id);
        
        builder.Property(t => t.Id)
            .IsRequired()
            .HasMaxLength(36);
        
        builder.Property(t => t.EventName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.EventVenue)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.EventDescription)
            .IsRequired()
            .HasMaxLength(200);
    }
}