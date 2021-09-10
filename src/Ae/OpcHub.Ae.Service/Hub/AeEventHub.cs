using Opc.Ae;
using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Api;
using OpcHub.Ae.Service.Configs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OpcHub.Ae.Service.Hub
{
    public class AeEventHub : IDisposable
    {
        #region Fields
        private AeServer _server;
        private Task _dequeueTask;
        private AeServerInitializeState _initializeState;

        private static readonly object AE_SERVER_INIT_LOCKER = new object();
        #endregion

        #region Ctor
        static AeEventHub()
        {
            Current = new AeEventHub();
        }

        private AeEventHub()
        {
            _server = InitializeAeServer();
            _dequeueTask = Task.Factory.StartNew(DequeueAndNotify, TaskCreationOptions.LongRunning);
            _dequeueTask.ConfigureAwait(false);

            _initializeState = AeServerInitializeState.Done;
        }
        #endregion

        #region Properties
        public static AeEventHub Current { get; }
        #endregion

        #region Methods
        public void ReCreateAeServer()
        {
            lock (AE_SERVER_INIT_LOCKER)
            {
                _initializeState = AeServerInitializeState.Initializing;

                AeServer newServer = null;
                try
                {
                    Log.AeServer("Start to re-create Hub.AeServer.");
                    newServer = new AeServer(OnAeServerShutdown);

                    // Dispose the existing server
                    DisposeAeServer();
                    Log.AeServer("The existing Hub.AeServer instance is disposed.");

                    // To use the new server
                    _server = newServer;
                    Log.AeServer("The new Hub.AeServer has replaced the existing instance.");
                }
                catch (Exception ex)
                {
                    Log.AeServer("AeEventHub.ReCreateAeServer failed.", isError: true);
                    Log.Error("AeEventHub.ReCreateAeServer failed.", ex);

                    newServer?.Dispose();
                    newServer = null;
                }
                finally
                {
                    _initializeState = AeServerInitializeState.Done;
                }
            }
        }

        public bool IsAeServerCreated(out string failedReason)
        {
            failedReason = null;
            if (_server == null)
                failedReason = "AeServer instance in AeEventHub isn't created or it has been disposed.";

            return _server != null;
        }

        public bool TryGetAeServerStatus(out ServerStatus serverStatus, out string failedReason)
        {
            serverStatus = null;
            failedReason = null;

            bool isSucceeded = true;
            if (_server == null)
            {
                failedReason = "AeServer instance in AeEventHub has been disposed.";
                isSucceeded = false;
            }
            else
            {
                serverStatus = _server.GetServerStatus();
                if (serverStatus == null)
                {
                    failedReason = "Call _server.GetStatus() failed.";
                    isSucceeded = false;
                }
            }

            return isSucceeded;
        }

        public ServerStatus GetAeServerStatus()
        {
            return _server?.GetServerStatus();
        }

        public DateTime GetTimeOfTheLastEvent()
        {
            return _server?.TimeOfTheLastEvent ?? DateTime.MinValue;
        }

        public AeServerInitializeState GetAeServerInitializeState()
        {
            lock (AE_SERVER_INIT_LOCKER)
            {
                return _initializeState;
            }
        }

        public List<AeEvent> GetQueuedEvents()
        {
            return _server?.GetQueuedEvents() ?? new List<AeEvent>();
        }
        #endregion

        #region Private Methods
        private AeServer InitializeAeServer()
        {
            AeServer aeServer = null;
            try
            {
                Log.AeServer("Start to create Hub.AeServer.");
                aeServer = new AeServer(OnAeServerShutdown);
            }
            catch (Exception ex)
            {
                Log.AeServer("AeEventHub.InitializeAeServer failed.", isError: true);
                Log.Error("AeEventHub.InitializeAeServer failed.", ex);

                aeServer?.Dispose();
                aeServer = null;
            }

            return aeServer;
        }

        private void OnAeServerShutdown(string reason)
        {
            // ServerShutdown event is caught for information only, it won't be triggered to HealthMonitor
            DisposeAeServer();
        }

        private void DequeueAndNotify()
        {
            while (true)
            {
                try
                {
                    if (_server != null)
                    {
                        AeEvent aeEvent;
                        while (_server.TryDequeue(out aeEvent))
                        {
                            Notify(aeEvent).Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error occured in DequeueAndNotify().", ex);
                }

                Thread.Sleep(500);
            }
        }

        private async Task Notify(AeEvent aeEvent)
        {
            if (!ApiUrl.IsWebApiConfigured()) return;

            int retriedTimes = 1;
            while (retriedTimes <= EventHubConfig.Push.EventPushRetryTimes)
            {
                try
                {
                    using (HttpClient http = new HttpClient())
                    {
                        Log.NotifiedEvent(aeEvent);
                        if (retriedTimes > 1)
                            http.Timeout = TimeSpan.FromSeconds(3);

                        var response = await http.PostAsJsonAsync(ApiUrl.NOTIFY_EVENT, aeEvent);
                        response.EnsureSuccessStatusCode();
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Log.NotifiedEvent(aeEvent, $"send to event hub middleware failed (retry counter={retriedTimes})");
                    Log.Error($"Notify event ({aeEvent?.Source}, {aeEvent?.Message}) to event hub middleware failed (retry counter={retriedTimes}).", ex);
                    retriedTimes++;

                    if (retriedTimes > EventHubConfig.Push.EventPushRetryTimes)
                    {
                        Log.NotifyFailedEvent(aeEvent);
                        break;
                    }
                }

                Thread.Sleep(EventHubConfig.Push.EventPushRetryInterval);
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Log.AeServer("Start to dispose AeEventHub instance.");

            DisposeAeServer();

            _dequeueTask?.Dispose();
            _dequeueTask = null;
            Log.AeServer("_dequeueTask instance in AeEventHub has been disposed.");
        }

        private void DisposeAeServer()
        {
            _server?.Dispose();
            _server = null;
            Log.AeServer("AeServer instance in AeEventHub has been disposed.");
        }
        #endregion
    }

    public enum AeServerInitializeState
    {
        Initializing,
        Done
    }
}
