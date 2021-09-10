using Opc.Ae;
using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Configs;
using OpcHub.Ae.Service.Hub;
using System;

namespace OpcHub.Ae.Service.Health
{
    public class HealthDetector
    {
        public AeHealthInfo GetHealthStatus()
        {
            string stationName = EventHubConfig.Opc.AeStationName;
            AeState state = AeState.Normal;
            AeFailureType failureType = AeFailureType.None;
            string failureReason = null;

            if (!CheckIfAeServerIsCreated(out failureReason))
            {
                state = AeState.Failed;
                failureType = AeFailureType.OPCServerNotConnected;
            }
            else if (!CheckIfWDTEventReceived())
            {
                // To query opc status may take more time than expected, so to check WDT before GetStatus().
                state = AeState.Failed;
                failureType = AeFailureType.AeNoWDTEventReceived;
                failureReason = "No WDT Events Received";
            }
            else if (!CheckIfAeServerIsRunning(out failureReason))
            {
                state = AeState.Failed;
                failureType = AeFailureType.OPCServerNotConnected;
            }

            return new AeHealthInfo(stationName, state, failureType, failureReason, DateTime.Now);
        }

        private bool CheckIfAeServerIsCreated(out string reason)
        {
            return AeEventHub.Current.IsAeServerCreated(out reason);
        }

        private bool CheckIfAeServerIsRunning(out string reason)
        {
            reason = string.Empty;

            ServerStatus serverStatus;
            string failedReason;
            if (!AeEventHub.Current.TryGetAeServerStatus(out serverStatus, out failedReason))
            {
                reason = failedReason;
                return false;
            }

            if (serverStatus.ServerState != ServerState.Running)
            {
                reason = $"Current A&E server status is {serverStatus.ServerState.ToString()}";
                return false;
            }

            return true;
        }

        private bool CheckIfWDTEventReceived()
        {
            // If WDT tags weren't configured or WDT interval is 0, always return true
            if (EventHubConfig.Health.WDTEventTags.Count == 0 ||
                EventHubConfig.Health.WDTInterval == 0) return true;

            DateTime timeOfLastEvent = AeEventHub.Current.GetTimeOfTheLastEvent();

            double abnormalMilliseconds =
                EventHubConfig.Health.WDTInterval +
                EventHubConfig.Health.AeHealthMonitorInterval +
                60 * 1000;
            return (DateTime.Now - timeOfLastEvent).TotalMilliseconds < abnormalMilliseconds;
        }
    }
}
