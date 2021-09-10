using Opc.Ae;
using OpcHub.Ae.Service.Configs;
using System.Linq;

namespace OpcHub.Ae.Service.Hub
{
    public static class AeEventFilter
    {
        public static bool Filter(EventNotification notification)
        {
            bool isFiltered = false;

            if (notification == null)
            {
                isFiltered = true;
                Log.FilteredEvent("Null notification", notification);
            }
            else if (EventHubConfig.Health.WDTEventTags != null &&
                EventHubConfig.Health.WDTEventTags.Count > 0 &&
                EventHubConfig.Health.WDTEventTags.Any(wdtTag => notification.SourceID == wdtTag))
            {
                // Filter the WDT tags
                isFiltered = true;
                Log.FilteredEvent("WDT", notification);
            }
            else if (EventHubConfig.Opc.EventSourcePrefixes != null &&
                EventHubConfig.Opc.EventSourcePrefixes.Count > 0 &&
                EventHubConfig.Opc.EventSourcePrefixes.All(prefix => !notification.SourceID.StartsWith(prefix)))
            {
                // Filter by sourceID prefixes
                isFiltered = true;
                Log.FilteredEvent("Source prefix", notification);
            }
            else if (EventHubConfig.Opc.EventMessagePrefixes != null &&
                     EventHubConfig.Opc.EventMessagePrefixes.Count > 0 &&
                     EventHubConfig.Opc.EventMessagePrefixes.All(prefix => !notification.Message.StartsWith(prefix)))
            {
                // Filter by message prefixes
                isFiltered = true;
                Log.FilteredEvent("Message Prefix", notification);
            }

            return isFiltered;
        }
    }
}
