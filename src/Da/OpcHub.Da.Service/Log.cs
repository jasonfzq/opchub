using log4net;
using Opc;
using Opc.Da;
using System;
using System.Collections.Generic;

namespace OpcHub.Da.Service
{
    public static class Log
    {
        private static readonly ILog _errorLog = LogManager.GetLogger("Error");
        private static readonly ILog _infoLog = LogManager.GetLogger("Info");
        private static readonly ILog _daServerLog = LogManager.GetLogger("DaServer");
        private static readonly ILog _healthLog = LogManager.GetLogger("Health");
        private static readonly ILog _readLog = LogManager.GetLogger("Read");
        private static readonly ILog _writeLog = LogManager.GetLogger("Write");
        private static readonly ILog _shortPollingReadLog = LogManager.GetLogger("ShortPollingRead");
        private static readonly ILog _shortPollingWriteLog = LogManager.GetLogger("ShortPollingWrite");
        private static readonly ILog _wdtLog = LogManager.GetLogger("WDT");

        public static void Error(object message, Exception exception)
        {
            SafeLog(() => _errorLog.Error(message, exception));
        }

        public static void Info(object message)
        {
            SafeLog(() => _infoLog.Info(message));
        }

        public static void DaServer(string message, bool isError = false)
        {
            SafeLog(() =>
            {
                if (isError)
                    SafeLog(() => _daServerLog.Error(message));
                else
                    SafeLog(() => _daServerLog.Info(message));
            });
        }

        public static void Read(Item[] items, bool shortPolling)
        {
            if (items == null || items.Length == 0) return;

#if DEBUG
            foreach (var item in items)
            {
                Console.WriteLine($@"Read Item: {item.ItemName}");
            }
#endif

            SafeLog(() =>
            {
                foreach (var item in items)
                {
                    if (shortPolling)
                        _shortPollingReadLog.Info($@"Read Item: {item.ItemName.PadRight(25)}");
                    else
                        _readLog.Info($@"Read Item: {item.ItemName.PadRight(25)}");
                }
            });
        }

        public static void Read(ItemValueResult[] results, bool shortPolling)
        {
            if (results == null || results.Length == 0) return;

#if DEBUG
            foreach (var result in results)
            {
                Console.WriteLine(string.Format(
                    @"Read Item: {0} Value: {1} Quality: {2} Timestamp: {3:dd/MM/yyyy HH:mm:ss:fff}",
                    result.ItemName.PadRight(25),
                    (result.Value?.ToString() ?? string.Empty).PadRight(20),
                    result.Quality.ToString().PadRight(6),
                    result.Timestamp));
            }
#endif

            SafeLog(() =>
            {
                foreach (var result in results)
                {
                    if (shortPolling)
                        _shortPollingReadLog.Info(string.Format(
                            "Read Item: {0} Value: {1} Quality: {2} Timestamp: {3:dd/MM/yyyy HH:mm:ss:fff}",
                            result.ItemName.PadRight(25),
                            (result.Value?.ToString() ?? string.Empty).PadRight(20),
                            result.Quality.ToString().PadRight(6),
                            result.Timestamp));
                    else
                        _readLog.Info(string.Format(
                            "Read Item: {0} Value: {1} Quality: {2} Timestamp: {3:dd/MM/yyyy HH:mm:ss:fff}",
                            result.ItemName.PadRight(25),
                            (result.Value?.ToString() ?? string.Empty).PadRight(20),
                            result.Quality.ToString().PadRight(6),
                            result.Timestamp));
                }
            });
        }

        public static void Write(ItemValue[] itemValues, bool shortPolling)
        {
            if (itemValues == null || itemValues.Length == 0) return;

#if DEBUG
            foreach (var item in itemValues)
            {
                Console.WriteLine(
                    $@"Write Item: {item.ItemName.PadRight(25)} Value: {(item.Value?.ToString() ?? string.Empty).PadRight(20)}");
            }
#endif

            SafeLog(() =>
            {
                foreach (var item in itemValues)
                {
                    if (shortPolling)
                        _shortPollingWriteLog.Info(string.Format(
                            "Write Item: {0} Value: {1}",
                            item.ItemName.PadRight(25),
                            (item.Value?.ToString() ?? string.Empty).PadRight(20)));
                    else
                        _writeLog.Info(string.Format(
                            "Write Item: {0} Value: {1}",
                            item.ItemName.PadRight(25),
                            (item.Value?.ToString() ?? string.Empty).PadRight(20)));
                }
            });
        }

        public static void Write(IdentifiedResult[] results, bool shortPolling)
        {
            if (results == null || results.Length == 0) return;

#if DEBUG
            foreach (var result in results)
            {
                Console.WriteLine($@"Write Item: {result.ItemName.PadRight(25)} ResultID: {result.ResultID.ToString()}");
            }
#endif

            SafeLog(() =>
            {
                foreach (var result in results)
                {
                    if (shortPolling)
                        _shortPollingWriteLog.Info(string.Format(
                            "Write Item: {0} ResultID: {1}",
                            result.ItemName.PadRight(25),
                            result.ResultID.ToString()));
                    else
                        _writeLog.Info(string.Format(
                            "Write Item: {0} ResultID: {1}",
                            result.ItemName.PadRight(25),
                            result.ResultID.ToString()));
                }
            });
        }

        public static void WDT(List<ItemValue> itemValues)
        {
            if (itemValues == null || itemValues.Count == 0) return;

            SafeLog(() =>
            {
                foreach (var item in itemValues)
                {
                    _wdtLog.Info(string.Format(
                        "Write Item: {0} Value: {1}",
                        item.ItemName.PadRight(25),
                        (item.Value?.ToString() ?? string.Empty).PadRight(20)));
                }
            });
        }

        public static void WDT(IdentifiedResult[] results)
        {
            if (results == null || results.Length == 0) return;

            SafeLog(() =>
            {
                foreach (var result in results)
                {
                    _wdtLog.Info(string.Format(
                        "Write Item: {0} ResultID: {1}",
                        result.ItemName.PadRight(25),
                        result.ResultID.ToString()));
                }
            });
        }

        public static void WDT(object message, Exception ex = null)
        {
            SafeLog(() =>
            {
                if (ex == null)
                    _wdtLog.Info(message);
                else
                    _wdtLog.Error(message, ex);
            });
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
                    var log = LogManager.GetLogger("Error");
                    log.Error(ex);
                }
                catch
                {
                }
            }
        }
    }
}