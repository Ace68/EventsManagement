namespace EventsManagement.Shared.Enums;

public sealed class EventState(int id, string name) : Enumeration(id, name)
{
    public static EventState Created = new (1, nameof(Created).ToLowerInvariant());
    public static EventState Published = new (2, nameof(Published).ToLowerInvariant());
    public static EventState Started = new (3, nameof(Started).ToLowerInvariant());
    public static EventState Closed = new (4, nameof(Started).ToLowerInvariant());
    
    public static IEnumerable<EventState> List() => new[] { Created, Published, Started, Closed };
    
    public static EventState FromName(string name)
    {
        var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return state ??
               throw new Exception($"Possible values for EventState: {string.Join(",", List().Select(s => s.Name))}");
    }

    public static EventState From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        return state ??
               throw new Exception($"Possible values for EventState: {string.Join(",", List().Select(s => s.Name))}");
    }
}