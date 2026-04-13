using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.Entities.Entities;
using EventsManagement.Shared.DomainIds;

namespace EventsManagement.Notifications.Entities.Helpers;

public static class EventsSchedulerHelper
{
    public static EventsSchedulerDto ToDto(this EventsScheduler entity)
    {
        return EventsSchedulerDto.Create(
            new CommunityEventId(entity.Id.Value),
            entity.EventName,
            entity.EventVenue,
            entity.EventDate,
            entity.EventState);
    }
}