using System.Collections.Generic;

namespace OpcHub.Da.Service.Configs
{
    public class HealthConfig
    {
        public HealthConfig(
            int healthMonitorInterval,
            int getStatusRetryTimes,
            int getStatusRetryInterval,
            int wdtInterval,
            List<string> wdtDataTags,
            string wdtTagValue)
        {
            HealthMonitorInterval = healthMonitorInterval;
            GetStatusRetryTimes = getStatusRetryTimes;
            GetStatusRetryInterval = getStatusRetryInterval;
            WDTInterval = wdtInterval;
            WDTDataTags = new List<string>(wdtDataTags);
            WDTTagValue = wdtTagValue;
        }

        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int HealthMonitorInterval { get; }

        public int GetStatusRetryTimes { get; }

        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int GetStatusRetryInterval { get; }

        /// <summary>
        /// Interval (millisecond)
        /// </summary>
        public int WDTInterval { get; set; }

        public IReadOnlyCollection<string> WDTDataTags { get; }
        
        public string WDTTagValue { get; set; }
    }
}
