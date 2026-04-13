namespace EventsManagements.InMemoryBroker;

public record EventHubParameters(string EventHubConnectionString, string EventHubName, 
    string BlobStorageConnectionString, string BlobStorageContainerName);