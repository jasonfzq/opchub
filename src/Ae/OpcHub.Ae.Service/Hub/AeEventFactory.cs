using Opc.Ae;
using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Configs;
using System;
using System.Linq;

namespace OpcHub.Ae.Service.Hub
{
    public static class AeEventFactory
    {
        public static AeEvent Build(EventNotification notification)
        {
            AeEvent aeEvent = null;
            if (notification == null) return aeEvent;

            AeEventCategoryConfig category = EventHubConfig.Opc.AeCategories.FirstOrDefault(c => (int)c.Category == notification.EventCategory);
            if (category == null)
            {
                Log.InvalidEvent(notification.SourceID, notification.Message, notification.Time, $"the event category ({notification.EventCategory}) is invalid.");
            }
            else
            {
                string stationNameOfFCS = string.Empty;
                DateTime? stationTimeGMT = null;

                var configuredAttributes = category.Attributes.ToList();
                for (int i = 0; i < configuredAttributes.Count; i++)
                {
                    var configuredAttribute = configuredAttributes[i];
                    if (notification.Attributes.Count > i)
                    {
                        switch (configuredAttribute.Attribute)
                        {
                            case AeAttribute.StationName:
                                stationNameOfFCS = notification.Attributes[i] as string;
                                break;
                            case AeAttribute.StationTimeGMT:
                                if (notification.Attributes[i] is DateTime)
                                    stationTimeGMT = (DateTime)notification.Attributes[i];
                                break;
                        }
                    }
                }

                if (stationTimeGMT == null)
                {
                    Log.InvalidEvent(notification.SourceID, notification.Message, notification.Time, "the attribute '179:Station time(GMT)' can't be found");
                }
                else
                {
                    aeEvent = new AeEvent(
                        notification.SourceID,
                        notification.Message,
                        notification.Time,
                        stationTimeGMT.Value,
                        EventHubConfig.Opc.AeStationName,
                        stationNameOfFCS);

                    if (aeEvent.StationTimeGMT == DateTime.MinValue)
                        Log.InvalidEvent(notification.SourceID, notification.Message, notification.Time, $"Station time(GMT) is {aeEvent.StationTimeGMT}. (event will still be sent to event hub middleware)");

                    if (Math.Abs((aeEvent.StationTimeGMT - DateTime.UtcNow).TotalMinutes) > 2)
                        Log.InvalidEvent(notification.SourceID, notification.Message, notification.Time, $"The difference between Station time(GMT) and current time is more than 2 minutes. (event will still be sent to event hub middleware)");
                }
            }

            return aeEvent;
        }
    }
}
