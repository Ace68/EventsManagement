using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.Entities.Entities;
using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;

namespace EventsManagement.Events.Entities.Helpers;

public static class CommunityEventHelper
{
    public static CommunityEventDto ToDto(this CommunityEvent communityEvent)
    {
        var dto = CommunityEventDto.CreateCommunityEvent(new CommunityEventId(communityEvent.Id.Value), communityEvent.EventName, communityEvent.EventDescription,
            communityEvent.EventVenue, communityEvent.EventDate);

        dto.AddEventOrganizers(communityEvent.EventOrganizers.Select(eo => eo.ToDto(new CommunityEventId(communityEvent.Id.Value))).ToList());

        return dto;
    }

    private static OrganizersDto ToDto(this EventOrganizer organizer, CommunityEventId eventId) =>
        OrganizersDto.CreateOrganizer(eventId, organizer);
    
    public static CommunityEvent ToAggregate(this CommunityEventDto communityEventDto)
    {
        return CommunityEvent.Create(
            new CommunityEventId(communityEventDto.Id.ToString()),
            new EventName(communityEventDto.EventName),
            new EventDescription(communityEventDto.EventDescription),
            new EventVenue(communityEventDto.EventVenue),
            communityEventDto.EventOrganizers.Select(eo => eo.ToAggregate()).ToList(),
            new EventDate(communityEventDto.EventDate)
        );
    }
    
    private static EventOrganizer ToAggregate(this OrganizersDto organizerDto) =>
        new (organizerDto.OrganizerName);

    public static CommunityEventJson ToJson(this CommunityEventDto communityEventDto)
    {
        return new CommunityEventJson
        {
            EventId = communityEventDto.Id.ToString(),
            EventName = communityEventDto.EventName,
            Description = communityEventDto.EventDescription,
            Venue = communityEventDto.EventVenue,
            Date = communityEventDto.EventDate,
            Organizers = communityEventDto.EventOrganizers != null
                ? communityEventDto.EventOrganizers.Select(eo => eo.OrganizerName).ToList()
                : []
        };
    }
}