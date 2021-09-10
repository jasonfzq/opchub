using System.Collections.Generic;
using System.Configuration;

namespace OpcHub.Da.Service.Configs
{
    public static class DataHubConfig
    {
        static DataHubConfig()
        {
            string stationName = ConfigurationManager.AppSettings["da_station_name"].Trim();
            string ip = ConfigurationManager.AppSettings["da_ip"].Trim();
            string progId = ConfigurationManager.AppSettings["da_prog_id"].Trim();
            string uid = ConfigurationManager.AppSettings["da_uid"]?.Trim();
            string pwd = ConfigurationManager.AppSettings["da_pwd"]?.Trim();
            string maxItemsReadPerBatch = ConfigurationManager.AppSettings["da_max_items_read_per_batch"].Trim();
            string maxItemsWritePerBatch = ConfigurationManager.AppSettings["da_max_items_write_per_batch"].Trim();

            string healthMonitorInterval = ConfigurationManager.AppSettings["da_health_monitor_interval"].Trim();
            string getStatusRetryTimes = ConfigurationManager.AppSettings["da_get_status_retry_times"].Trim();
            string getStatusRetryInterval = ConfigurationManager.AppSettings["da_get_status_retry_interval"].Trim();
            string wdtTags = ConfigurationManager.AppSettings["da_wdt_tags"]?.Trim();
            string wdtTagValue = ConfigurationManager.AppSettings["da_wdt_tag_value"]?.Trim();
            string wdtInterval = ConfigurationManager.AppSettings["da_wdt_interval"]?.Trim();

            string middlewareWebApiUrl = ConfigurationManager.AppSettings["data_hub_middleware_web_api_url"].Trim();
            string dataChangePushRetryTimes = ConfigurationManager.AppSettings["data_change_push_retry_times"].Trim();
            string dataChangePushRetryInterval = ConfigurationManager.AppSettings["data_change_push_retry_interval"].Trim();
            string healthPushRetryTimes = ConfigurationManager.AppSettings["health_push_retry_times"].Trim();
            string healthPushRetryInterval = ConfigurationManager.AppSettings["health_push_retry_interval"].Trim();

            string apiUrl = ConfigurationManager.AppSettings["web_api_url"].Trim();

            Opc = new OpcConfig(
                stationName.ToUpper(),
                $"opcda://{ip}/{progId}",
                uid,
                pwd,
                int.Parse(maxItemsReadPerBatch),
                int.Parse(maxItemsWritePerBatch));

            Push = new PushConfig(
                middlewareWebApiUrl,
                int.Parse(dataChangePushRetryTimes),
                int.Parse(dataChangePushRetryInterval),
                int.Parse(healthPushRetryTimes),
                int.Parse(healthPushRetryInterval));

            Health = new HealthConfig(
                int.Parse(healthMonitorInterval),
                int.Parse(getStatusRetryTimes),
                int.Parse(getStatusRetryInterval),
                wdtInterval == null || wdtInterval.Trim().Length == 0 ? 0 : int.Parse(wdtInterval.Trim()),
                GetConfiguredStringArray(wdtTags),
                wdtTagValue);

            ApiUrl = apiUrl;

        }

        public static OpcConfig Opc { get; }

        public static PushConfig Push { get; }

        public static HealthConfig Health { get; }

        public static string ApiUrl { get; }

        public static void Initialize() { }

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
    }
}
