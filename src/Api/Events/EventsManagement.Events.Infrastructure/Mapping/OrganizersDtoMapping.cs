using EventsManagement.Events.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagement.Events.Infrastructure.Mapping;

public class OrganizersDtoMapping : IEntityTypeConfiguration<OrganizersDto>
{
    public void Configure(EntityTypeBuilder<OrganizersDto> builder)
    {
        builder.ToTable("Organizers", "dbo");
        builder.HasKey(o => o.Id);
        
        builder.Property(t => t.Id)
            .IsRequired()
            .HasMaxLength(36);
        
        builder.Property(t => t.EventId)
            .IsRequired()
            .HasMaxLength(36);
        
        builder.Property(t => t.OrganizerName)
            .IsRequired()
            .HasMaxLength(100);
    }
}