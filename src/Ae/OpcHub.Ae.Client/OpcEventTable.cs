using System;
using System.Collections.Generic;
using System.Linq;
using OpcHub.Ae.Client.Configuration;

namespace OpcHub.Ae.Client
{
    public class OpcEventTable
    {
        private readonly IReadOnlyList<AeEventData> _eventTable;

        internal OpcEventTable(AeBlockOption blockOption)
        {
            _eventTable = GetEventTable(blockOption);
        }

        public AeEventData GetEventData(string message)
        {
            return _eventTable.FirstOrDefault(ed => message.Trim().EndsWith(ed.EventCode));
        }

        private List<AeEventData> GetEventTable(AeBlockOption blockOption)
        {
            List<AeEventData> eventTable = new List<AeEventData>();
            foreach (AeBlockConfig blockConfig in blockOption.Blocks)
            {
                if (string.IsNullOrWhiteSpace(blockConfig.Name))
                    throw new InvalidOperationException("block name of the ae event isn't configured.");

                foreach (AeBlockEventConfig eventConfig in blockConfig.Events)
                {
                    if (string.IsNullOrWhiteSpace(eventConfig.Code))
                        throw new InvalidOperationException("event code of the ae event isn't configured.");
                    if (string.IsNullOrWhiteSpace(eventConfig.Callback))
                        throw new InvalidOperationException("event callback of the ae event isn't configured.");
                    if (string.IsNullOrWhiteSpace(eventConfig.Remark))
                        throw new InvalidOperationException("event remark of the ae event isn't configured.");

                    eventTable.Add(new AeEventData(
                        blockConfig.Name,
                        blockConfig.Equipment,
                        eventConfig.Code,
                        eventConfig.Callback,
                        eventConfig.Remark));
                }
            }

            return eventTable;
        }
    }
}
