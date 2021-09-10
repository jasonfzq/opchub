using System.Collections.Generic;

namespace OpcHub.Ae.Service.Configs
{
    public class HealthConfig
    {
        public HealthConfig(
            int healthMonitorInterval,
            int getStatusRetryTimes,
            int getStatusRetryInterval,
            int wdtInterval,
            List<string> wdtEventTags)
        {
            AeHealthMonitorInterval = healthMonitorInterval;
            GetStatusRetryTimes = getStatusRetryTimes;
            GetStatusRetryInterval = getStatusRetryInterval;
            WDTInterval = wdtInterval;
            WDTEventTags = new List<string>(wdtEventTags);
        }

        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int AeHealthMonitorInterval { get; }

        public int GetStatusRetryTimes { get; }

        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int GetStatusRetryInterval { get; }


        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int WDTInterval { get; set; }

        public IReadOnlyCollection<string> WDTEventTags { get; }
    }
}
