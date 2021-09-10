namespace OpcHub.Ae.Service.Configs
{
    public class PushConfig
    {
        public PushConfig(
            string webApiUrl, 
            int eventPushRetryTimes, int eventPushRetryInterval,
            int healthPushRetryTimes, int healthPushRetryInterval)
        {
            EventHubMiddlewareWebApiUrl = webApiUrl;
            EventPushRetryTimes = eventPushRetryTimes;
            EventPushRetryInterval = eventPushRetryInterval;
            HealthPushRetryTimes = healthPushRetryTimes;
            HealthPushRetryInterval = healthPushRetryInterval;
        }

        public string EventHubMiddlewareWebApiUrl { get; }

        public int EventPushRetryTimes { get; }

        /// <summary>
        /// Retry interval (millisecond)
        /// </summary>
        public int EventPushRetryInterval { get; }

        public int HealthPushRetryTimes { get; }

        /// <summary>
        /// Retry interval (millisecond)
        /// </summary>
        public int HealthPushRetryInterval { get; }
    }
}
