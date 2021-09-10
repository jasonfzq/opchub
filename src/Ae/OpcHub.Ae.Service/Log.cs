using log4net;
using Opc.Ae;
using OpcHub.Ae.Contract;
using System;

namespace OpcHub.Ae.Service
{
    public static class Log
    {
        private static readonly ILog _errorLog = LogManager.GetLogger("Error");
        private static readonly ILog _infoLog = LogManager.GetLogger("Info");
        private static readonly ILog _aeServerLog = LogManager.GetLogger("AeServer");
        private static readonly ILog _healthLog = LogManager.GetLogger("Health");
        private static readonly ILog _rawEventLog = LogManager.GetLogger("RawEvents");
        private static readonly ILog _filteredEventLog = LogManager.GetLogger("FilteredEvents");
        private static readonly ILog _invalidEventLog = LogManager.GetLogger("InvalidEvents");
        private static readonly ILog _enqueueEventLog = LogManager.GetLogger("EnqueueEvents");
        private static readonly ILog _dequeueEventLog = LogManager.GetLogger("DequeueEvents");
        private static readonly ILog _notifiedEventLog = LogManager.GetLogger("NotifiedEvents");
        private static readonly ILog _notifyFailedEventLog = LogManager.GetLogger("NotifyFailedEvents");

        public static void Error(object message, Exception exception)
        {
            SafeLog(() => _errorLog.Error(message, exception));
        }

        public static void Info(object message)
        {
            SafeLog(() => _infoLog.Info(message));
        }

        public static void AeServer(string message, bool isError = false)
        {
            if (isError)
                SafeLog(() => _aeServerLog.Error(message));
            else
                SafeLog(() => _aeServerLog.Info(message));
        }

        public static void Health(string action, AeHealthInfo healthInfo, string errorMessage = null)
        {
            if (errorMessage == null)
            {
                SafeLog(() =>
                {
                    if (healthInfo == null)
                    {
                        _healthLog.Info($"{action}, healthInfo is null");
                    }
                    else
                    {
                        _healthLog.Info(healthInfo.State == AeState.Normal
                            ? $"{action}, {healthInfo.StationName}, {healthInfo.State}"
                            : $"{action}, {healthInfo.StationName}, {healthInfo.State}, {healthInfo.FailureType}, {healthInfo.FailureReason}");
                    }
                });
            }
            else
            {
                SafeLog(() =>
                {
                    if (healthInfo == null)
                    {
                        _healthLog.Error($"{action}, healthInfo is null, {errorMessage}");
                    }
                    else
                    {
                        _healthLog.Error(healthInfo.State == AeState.Normal
                            ? $"{action}, {healthInfo.StationName}, {healthInfo.State}, {errorMessage}"
                            : $"{action}, {healthInfo.StationName}, {healthInfo.State}, {healthInfo.FailureType}, {healthInfo.FailureReason}, {errorMessage}");
                    }
                });
            }
        }

        public static void RawEvent(EventNotification notification)
        {
            SafeLog(() => _rawEventLog.Info(notification == null
                ? "notification is null"
                : $"{notification.SourceID}, {notification.Message}, {notification.Time}, {notification.Quality}, {notification.ClientHandle}, {notification.EventCategory}, {notification.EventType}"));
        }

        public static void FilteredEvent(string filterType, EventNotification notification)
        {
            SafeLog(() => _filteredEventLog.Info(notification == null
                ? $"{filterType}, notification is null"
                : $"{filterType}, {notification.SourceID}, {notification.Message}, {notification.Time}, {notification.Quality}, {notification.ClientHandle}, {notification.EventCategory}, {notification.EventType}"));
        }

        public static void InvalidEvent(string source, string message, DateTime eventTime, string reason)
        {
            SafeLog(() => _invalidEventLog.Info($"({source}, {message}, {eventTime}), {reason}"));
        }

        public static void EnqueueEvent(AeEvent aeEvent)
        {
            SafeLog(() => _enqueueEventLog.Info($"{aeEvent.Source}, {aeEvent.Message}, {aeEvent.EventTime}, (GMT){aeEvent.StationTimeGMT}, {aeEvent.StationNameOfOpcServer}, {aeEvent.StationNameOfFCS}"));
        }

        public static void DequeueEvent(AeEvent aeEvent)
        {
            SafeLog(() => _dequeueEventLog.Info($"{aeEvent.Source}, {aeEvent.Message}, {aeEvent.EventTime}, (GMT){aeEvent.StationTimeGMT}, {aeEvent.StationNameOfOpcServer}, {aeEvent.StationNameOfFCS}"));
        }

        public static void NotifiedEvent(AeEvent aeEvent, string failedReason = null)
        {
            if (string.IsNullOrEmpty(failedReason))
                SafeLog(() => _notifiedEventLog.Info($"{aeEvent.Source}, {aeEvent.Message}, {aeEvent.EventTime}, (GMT){aeEvent.StationTimeGMT}, {aeEvent.StationNameOfOpcServer}, {aeEvent.StationNameOfFCS}"));
            else
                SafeLog(() => _notifiedEventLog.Error($"{aeEvent.Source}, {aeEvent.Message}, {aeEvent.EventTime}, (GMT){aeEvent.StationTimeGMT}, {aeEvent.StationNameOfOpcServer}, {aeEvent.StationNameOfFCS}, {failedReason}"));
        }

        public static void NotifyFailedEvent(AeEvent aeEvent)
        {
            SafeLog(() => _notifyFailedEventLog.Info($"{aeEvent.Source}, {aeEvent.Message}, {aeEvent.EventTime}, (GMT){aeEvent.StationTimeGMT}, {aeEvent.StationNameOfOpcServer}, {aeEvent.StationNameOfFCS}"));
        }

        private static void SafeLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                try
                {
                    ILog log = LogManager.GetLogger("Error");
                    log.Error(ex);
                }
                catch
                {
                }
            }
        }
    }
}
