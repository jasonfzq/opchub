using OpcHub.Ae.Service.Configs;

namespace OpcHub.Ae.Service.Api
{
    public static class ApiUrl
    {
        public static readonly string NOTIFY_EVENT = $"{EventHubConfig.Push.EventHubMiddlewareWebApiUrl}/NotifyEvent";
        public static readonly string NOTIFY_HEALTH_STATUS = $"{EventHubConfig.Push.EventHubMiddlewareWebApiUrl}/NotifyHealthStatus";

        public static bool IsWebApiConfigured()
        {
            return EventHubConfig.Push.EventHubMiddlewareWebApiUrl != null &&
                   EventHubConfig.Push.EventHubMiddlewareWebApiUrl.Trim().Length > 0;
        }
    }
}
