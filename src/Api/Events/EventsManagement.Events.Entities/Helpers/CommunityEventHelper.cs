using EventsManagement.Events.Entities.Dtos;
using EventsManagement.Events.Entities.Entities;
using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;

namespace EventsManagement.Events.Entities.Helpers;

public static class CommunityEventHelper
{
    public static CommunityEventDto ToDto(this CommunityEvent communityEvent)
    {
        return new CommunityEventDto
        {
            EventName = communityEvent.EventName.Value,
            EventDescription = communityEvent.EventDescription.Value,
            EventVenue = communityEvent.EventVenue.Value,
            EventDate = communityEvent.EventDate.Value,
            EventOrganizers = communityEvent.EventOrganizers.Select(eo => eo.ToDto(communityEvent.EventId)).ToList()
        };
    }

    private static OrganizersDto ToDto(this EventOrganizer organizer, CommunityEventId eventId) =>
        OrganizersDto.CreateOrganizer(eventId, organizer);
}