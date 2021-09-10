namespace OpcHub.Da.Service.Configs
{
    public class PushConfig
    {
        public PushConfig(
            string dataHubMiddlewareWebApiUrl,
            int dataChangePushRetryTimes,
            int dataChangePushRetryInterval,
            int healthPushRetryTimes,
            int healthPushRetryInterval)
        {
            DataHubMiddlewareWebApiUrl = dataHubMiddlewareWebApiUrl;
            DataChangePushRetryTimes = dataChangePushRetryTimes;
            DataChangePushRetryInterval = dataChangePushRetryInterval;
            HealthPushRetryTimes = healthPushRetryTimes;
            HealthPushRetryInterval = healthPushRetryInterval;
        }

        public string DataHubMiddlewareWebApiUrl { get; }

        public int DataChangePushRetryTimes { get; }

        /// <summary>
        /// Retry interval (millisecond)
        /// </summary>
        public int DataChangePushRetryInterval { get; }

        public int HealthPushRetryTimes { get; }

        /// <summary>
        /// Retry interval (millisecond)
        /// </summary>
        public int HealthPushRetryInterval { get; }
    }
}
