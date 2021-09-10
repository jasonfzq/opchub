using Opc;
using Opc.Ae;
using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Configs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace OpcHub.Ae.Service.Hub
{
    public class AeServer : IDisposable
    {
        #region Fields
        private readonly ConcurrentQueue<AeEvent> _eventQueue = new ConcurrentQueue<AeEvent>();

        private readonly EventType _eventType = EventType.Simple;
        private readonly Action<string> _aeServerShutdown;

        private Opc.Ae.Server _server;
        private Opc.Ae.Subscription _subscription;
        #endregion

        #region Ctor
        public AeServer(Action<string> actionOfAeServerShutdown)
        {
            Log.AeServer("Constructing Hub.AeServer().");

            _aeServerShutdown = actionOfAeServerShutdown;

            CreateServer();
            Connect();
            AddSubscription();

            Log.AeServer("Construct Hub.AeServer() succeeded.");
        }
        #endregion

        #region Properties
        public DateTime TimeOfTheLastEvent { get; private set; } = DateTime.Now;
        #endregion

        #region Methods
        public ServerStatus GetServerStatus()
        {
            ServerStatus status = null;
            if (_server == null) return null;

            int retriedTimes = 1;
            while (retriedTimes <= EventHubConfig.Health.GetStatusRetryTimes)
            {
                try
                {
                    status = _server.GetStatus();
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error($"Call _server.GetStatus() failed (retry counter={retriedTimes}).", ex);
                    retriedTimes++;
                }

                Thread.Sleep(EventHubConfig.Health.GetStatusRetryInterval);
            }

            return status;
        }

        public bool TryDequeue(out AeEvent aeEvent)
        {
            if (_eventQueue.TryDequeue(out aeEvent))
            {
                Log.DequeueEvent(aeEvent);
                return true;
            }

            return false;
        }

        public List<AeEvent> GetQueuedEvents()
        {
            return _eventQueue.ToList();
        }
        #endregion

        #region Private Methods
        private void CreateServer()
        {
            _server = new Opc.Ae.Server(new OpcCom.Factory(), new URL(EventHubConfig.Opc.AeUrl));
            Log.AeServer("Create Opc.Ae.Server succeeded.");
        }

        private void Connect()
        {
            NetworkCredential credential = null;
            if (!string.IsNullOrEmpty(EventHubConfig.Opc.AeUID) &&
                !string.IsNullOrEmpty(EventHubConfig.Opc.AePWD))
                credential = new NetworkCredential(EventHubConfig.Opc.AeUID, EventHubConfig.Opc.AePWD);

            _server.Connect(new Opc.ConnectData(credential));
            _server.ServerShutdown += OnServerShutdown;

            Log.AeServer("Connect Opc.Ae.Server succeeded.");
        }

        private void AddSubscription()
        {
            // Build subscription state
            SubscriptionState state = new SubscriptionState
            {
                Active = false,
                ClientHandle = Guid.NewGuid().ToString()
            };

            // Build subscription filters
            SubscriptionFilters filters = new SubscriptionFilters { EventTypes = (int)_eventType };
            if (EventHubConfig.Opc.AeCategories.Count > 0)
                filters.Categories.AddRange(EventHubConfig.Opc.AeCategories.Select(c => (int)c.Category).ToList());
            Log.AeServer("Create subscription filters succeeded.");

            // Build subscription
            _subscription = (Subscription)_server.CreateSubscription(state);
            _subscription.SetFilters(filters);
            Log.AeServer($"Create subscription succeeded (ClientHandle: {_subscription.ClientHandle}).");

            // Select attributes
            foreach (AeEventCategoryConfig categoryConfig in EventHubConfig.Opc.AeCategories)
            {
                if (categoryConfig.Attributes != null && categoryConfig.Attributes.Count > 0)
                {
                    _subscription.SelectReturnedAttributes(
                        (int)categoryConfig.Category,
                        categoryConfig.Attributes.Select(attr => (int)attr.Attribute).ToArray());
                }
            }
            Log.AeServer("Select attributes for subscription succeeded.");

            // Activate the subscription
            state.Active = true;
            _subscription.ModifyState((int)StateMask.Active, state);
            Log.AeServer("Activate subscription succeeded.");

            // Register event changed handler
            _subscription.EventChanged += OnEventChanged;

            Log.AeServer("Add subscription to Opc.Ae.Server completed.");
        }
        #endregion

        #region Event Handlers
        private void OnServerShutdown(string reason)
        {
            Log.AeServer($"OnServerShutdown event occured, reason: {reason}.");
            _aeServerShutdown?.Invoke(reason);
        }

        private void OnEventChanged(EventNotification[] notifications, bool refresh, bool lastRefresh)
        {
            TimeOfTheLastEvent = DateTime.Now;

            foreach (EventNotification notification in notifications)
            {
                if (AeEventFilter.Filter(notification)) continue;

                // The noise events are filtered, only log the required notification
                Log.RawEvent(notification);

                AeEvent aeEvent = AeEventFactory.Build(notification);
                if (aeEvent != null)
                {
                    _eventQueue.Enqueue(aeEvent);
                    Log.EnqueueEvent(aeEvent);
                }
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            try
            {
                if (_subscription != null)
                {
                    _subscription.EventChanged -= OnEventChanged;
                    _subscription.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error("AeServer.Dispose(), dispose _subscription failed.", ex);
            }

            try
            {
                if (_server != null)
                {
                    _server.ServerShutdown -= OnServerShutdown;
                    _server.Disconnect();
                    _server.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error("AeServer.Dispose(), dispose _server failed.", ex);
            }

            _subscription = null;
            _server = null;
        }
        #endregion
    }
}