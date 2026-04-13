using EventsManagement.Notifications.Entities.Dtos;
using EventsManagement.Notifications.Entities.Entities;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.ExternalContracts;

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
    
    public static EventsSchedulerJson ToJson(this EventsSchedulerDto dto)
    {
        return new EventsSchedulerJson
        {
            EventId = dto.Id.ToString(),
            EventName = dto.EventName,
            EventVenue = dto.EventVenue,
            EventDate = dto.EventDate,
            EventState = dto.EventState
        };
    }
}