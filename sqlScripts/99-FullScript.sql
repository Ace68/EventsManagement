CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'CesP@ssword2026'

CREATE DATABASE SCOPED CREDENTIAL SqlWarehouseCredential
WITH 
  IDENTITY = 'SHARED ACCESS SIGNATURE',
  SECRET = 'SharedAccessSignature sr=https%3a%2f%2fces-eventhub.servicebus.windows.net%2fglobalazurehub&sig=m6U3I0hxpuagjPXCsiw1xgg%2ftqxpJvl0vpRWhLB8j5M%3d&se=1792171321&skn=ces-policy'

EXEC sys.sp_enable_event_stream

EXEC sys.sp_create_event_stream_group
  @stream_group_name      = 'ces-globalazure-2026',
  @destination_location   = 'ces-eventhub.servicebus.windows.net/globalazurehub',
  @destination_credential = SqlWarehouseCredential,
  @destination_type       = 'AzureEventHubsAmqp'

EXEC sys.sp_add_object_to_event_stream_group
  @stream_group_name      = 'ces-globalazure-2026',
  @object_name = 'dbo.CommunityEvents',
  @include_old_values = 1,
  @include_all_columns = 1

EXEC sp_help_change_feed_table @source_schema = 'dbo', @source_name = 'CommunityEvents'