using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Api;
using OpcHub.Ae.Service.Configs;
using OpcHub.Ae.Service.Hub;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OpcHub.Ae.Service.Utils;

namespace OpcHub.Ae.Service.Health
{
    public class HealthMonitor : IDisposable
    {
        #region Fields
        private readonly SafeTimer _timer;
        private AeHealthInfo _healthInfo;
        #endregion

        #region Ctor
        static HealthMonitor()
        {
            Current = new HealthMonitor();
        }

        private HealthMonitor()
        {
            _timer = new SafeTimer(CheckHealthStatus, EventHubConfig.Health.AeHealthMonitorInterval);
            _timer.Run();
        }
        #endregion

        #region Properties
        public static HealthMonitor Current { get; }
        #endregion

        #region Methods
        public AeHealthInfo GetHealthStatus()
        {
            return _healthInfo;
        }

        private void CheckHealthStatus()
        {
            try
            {
                HealthDetector detector = new HealthDetector();
                _healthInfo = detector.GetHealthStatus();

                Log.Health("Check health status", _healthInfo);

                // Send to event hub middleware
                NotifyHealthStatus().Wait();

                // Recreate AeServer
                if (_healthInfo.State == AeState.Failed)
                {
                    // So far the below state checking is not necessary,
                    // it will be useful when changing the recreate server to async way.
                    var state = AeEventHub.Current.GetAeServerInitializeState();
                    if (state != AeServerInitializeState.Initializing)
                        AeEventHub.Current.ReCreateAeServer();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckHealthStatus failed.", ex);
            }
        }

        private async Task NotifyHealthStatus()
        {
            if (!ApiUrl.IsWebApiConfigured()) return;

            return;

            int retriedTimes = 1;
            while (retriedTimes <= EventHubConfig.Push.HealthPushRetryTimes)
            {
                try
                {
                    using (HttpClient http = new HttpClient())
                    {
                        Log.Health("Notify health status", _healthInfo);
                        if (retriedTimes > 1)
                            http.Timeout = TimeSpan.FromSeconds(3);

                        var response = await http.PostAsJsonAsync(ApiUrl.NOTIFY_HEALTH_STATUS, _healthInfo);
                        response.EnsureSuccessStatusCode();
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Log.Health("Notify health status", _healthInfo, $"Failed (retry counter={retriedTimes})");
                    Log.Error($"Notify health status to event hub middleware failed (retry counter={retriedTimes}).", ex);
                    retriedTimes++;

                    if (retriedTimes > EventHubConfig.Push.HealthPushRetryTimes)
                    {
                        Log.Health("Notify health status", _healthInfo, "Failed finally");
                        break;
                    }
                }

                Thread.Sleep(EventHubConfig.Push.HealthPushRetryInterval);
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            _timer?.Dispose();
        }
        #endregion
    }
}
