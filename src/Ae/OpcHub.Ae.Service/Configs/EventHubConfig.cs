using System;
using System.Collections.Generic;
using System.Configuration;

namespace OpcHub.Ae.Service.Configs
{
    public static class EventHubConfig
    {
        static EventHubConfig()
        {
            string stationName = ConfigurationManager.AppSettings["ae_station_name"].Trim();
            string ip = ConfigurationManager.AppSettings["ae_ip"].Trim();
            string progId = ConfigurationManager.AppSettings["ae_prog_id"].Trim();
            string uid = ConfigurationManager.AppSettings["ae_uid"]?.Trim();
            string pwd = ConfigurationManager.AppSettings["ae_pwd"]?.Trim();
            string eventSourcePrefixes = ConfigurationManager.AppSettings["ae_event_filter_source_start_with"]?.Trim();
            string eventMessagePrefixes = ConfigurationManager.AppSettings["ae_event_filter_message_start_with"]?.Trim();

            string healthMonitorInterval = ConfigurationManager.AppSettings["ae_health_monitor_interval"].Trim();
            string getStatusRetryTimes = ConfigurationManager.AppSettings["ae_get_status_retry_times"].Trim();
            string getStatusRetryInterval = ConfigurationManager.AppSettings["ae_get_status_retry_interval"].Trim();
            string wdtTags = ConfigurationManager.AppSettings["ae_wdt_event_tags"]?.Trim();
            string wdtInterval = ConfigurationManager.AppSettings["ae_wdt_interval"]?.Trim();

            string middlewareWebApiUrl = ConfigurationManager.AppSettings["event_hub_middleware_web_api_url"].Trim();
            string eventPushRetryTimes = ConfigurationManager.AppSettings["event_push_retry_times"].Trim();
            string eventPushRetryInterval = ConfigurationManager.AppSettings["event_push_retry_interval"].Trim();
            string healthPushRetryTimes = ConfigurationManager.AppSettings["health_push_retry_times"].Trim();
            string healthPushRetryInterval = ConfigurationManager.AppSettings["health_push_retry_interval"].Trim();
            
            string apiUrl = ConfigurationManager.AppSettings["web_api_url"].Trim();

            Opc = new OpcConfig(
                stationName.ToUpper(),
                $"opcae://{ip}/{progId}",
                uid,
                pwd,
                GetConfiguredAeCategories(),
                GetConfiguredStringArray(eventSourcePrefixes),
                GetConfiguredStringArray(eventMessagePrefixes));

            Push = new PushConfig(
                middlewareWebApiUrl,
                int.Parse(eventPushRetryTimes),
                int.Parse(eventPushRetryInterval),
                int.Parse(healthPushRetryTimes),
                int.Parse(healthPushRetryInterval));

            Health = new HealthConfig(
                int.Parse(healthMonitorInterval),
                int.Parse(getStatusRetryTimes),
                int.Parse(getStatusRetryInterval),
                wdtInterval == null || wdtInterval.Trim().Length == 0 ? 0 : int.Parse(wdtInterval.Trim()),
                GetConfiguredWDTEventTags(wdtTags));

            ApiUrl = apiUrl;
        }

        public static OpcConfig Opc { get; }

        public static PushConfig Push { get; }

        public static HealthConfig Health { get; }

        public static string ApiUrl { get; }

        public static void Initialize() { }

        private static List<AeEventCategoryConfig> GetConfiguredAeCategories()
        {
            List<AeEventCategoryConfig> categories = new List<AeEventCategoryConfig>();
            AeEventCategoryConfig categoryConfig = new AeEventCategoryConfig
            {
                Category = AeCategory.SequenceMessage,
                Attributes = new List<AeEventAttributeConfig>
                {
                    new AeEventAttributeConfig { Attribute = AeAttribute.StationName, DataType = typeof(string) },
                    new AeEventAttributeConfig { Attribute = AeAttribute.StationTimeGMT, DataType = typeof(DateTime) }
                }
            };
            categories.Add(categoryConfig);

            return categories;
        }

        private static List<string> GetConfiguredStringArray(string fullText)
        {
            List<string> configuredTexts = new List<string>();
            if (fullText == null || fullText.Trim().Length == 0) return configuredTexts;

            string[] splitArray = fullText.Split(',');
            foreach (var text in splitArray)
            {
                if (text.Trim().Length > 0)
                    configuredTexts.Add(text.Trim());
            }

            return configuredTexts;
        }

        private static List<string> GetConfiguredWDTEventTags(string wdtTags)
        {
            List<string> configuredWDTTags = new List<string>();
            if (wdtTags == null || wdtTags.Trim().Length == 0) return configuredWDTTags;

            string[] tags = wdtTags.Split(',');
            foreach (var tag in tags)
            {
                if (tag.Trim().Length > 0)
                    configuredWDTTags.Add(tag.Trim());
            }

            return configuredWDTTags;
        }
    }
}
