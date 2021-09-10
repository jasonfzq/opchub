using Opc;
using Opc.Da;
using OpcHub.Da.Service.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using OpcHub.Da.Service.Utils;
using Factory = OpcCom.Factory;
using Server = Opc.Da.Server;

namespace OpcHub.Da.Service.Hub
{
    public class DaServer : IDisposable
    {
        #region Fields

        private readonly Action<string> _daServerShutdown;
        private Server _server;
        private SafeTimer _wdtTimer;

        #endregion

        #region Ctor

        public DaServer(Action<string> actionOfDaServerShutdown)
        {
            Log.DaServer("Constructing Hub.DaServer().");

            _daServerShutdown = actionOfDaServerShutdown;

            CreateServer();
            Connect();
            AddSubscription();

            _wdtTimer = new SafeTimer(WriteWatchDog, DataHubConfig.Health.WDTInterval);
            _wdtTimer.Run();

            Log.DaServer("Construct Hub.DaServer() succeeded.");
        }

        #endregion

        #region Properties

        public DateTime TimeOfTheLastSuccessfulWDT { get; private set; } = DateTime.Now;

        #endregion

        #region Event Handlers

        private void OnServerShutdown(string reason)
        {
            Log.DaServer($"OnServerShutdown event occurred, reason: {reason}.");
            _daServerShutdown?.Invoke(reason);
        }

        #endregion

        #region Methods

        public ServerStatus GetServerStatus()
        {
            ServerStatus status = null;
            if (_server == null) return null;

            var retriedTimes = 1;
            while (retriedTimes <= DataHubConfig.Health.GetStatusRetryTimes)
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

                Thread.Sleep(DataHubConfig.Health.GetStatusRetryInterval);
            }

            return status;
        }

        public List<ItemValueResult> Read(List<Item> items, bool shortPolling)
        {
            var results = new List<ItemValueResult>();
            if (items == null || items.Count == 0) return results;

            var maxItemsReadPerBatch = DataHubConfig.Opc.MaxItemsReadPerBatch;
            var totalRounds = (int) Math.Ceiling((decimal) items.Count / maxItemsReadPerBatch);
            for (var i = 0; i < totalRounds; i++)
            {
                var bathItems = items
                    .Skip(maxItemsReadPerBatch * i)
                    .Take(maxItemsReadPerBatch).ToArray();

                Log.Read(bathItems, shortPolling);
                ItemValueResult[] batchResults = _server.Read(bathItems);
                results.AddRange(batchResults);
                Log.Read(batchResults, shortPolling);
            }

            return results;
        }

        public List<IdentifiedResult> Write(List<ItemValue> items, bool shortPolling)
        {
            var results = new List<IdentifiedResult>();
            if (items == null || items.Count == 0) return results;

            var maxItemsWritePerBatch = DataHubConfig.Opc.MaxItemsWritePerBatch;
            var totalRounds = (int) Math.Ceiling((decimal) items.Count / maxItemsWritePerBatch);
            for (var i = 0; i < totalRounds; i++)
            {
                var bathItems = items
                    .Skip(maxItemsWritePerBatch * i)
                    .Take(maxItemsWritePerBatch).ToArray();

                Log.Write(bathItems, shortPolling);
                IdentifiedResult[] batchResults = _server.Write(bathItems);
                results.AddRange(batchResults);
                Log.Write(batchResults, shortPolling);
            }

            return results;
        }

        #endregion

        #region Private Methods

        private void CreateServer()
        {
            _server = new Server(new Factory(), new URL(DataHubConfig.Opc.DaUrl));
            Log.DaServer("Create Opc.Da.Server succeeded.");
        }

        private void Connect()
        {
            NetworkCredential credential = null;
            if (!string.IsNullOrEmpty(DataHubConfig.Opc.DaUID) &&
                !string.IsNullOrEmpty(DataHubConfig.Opc.DaPWD))
                credential = new NetworkCredential(DataHubConfig.Opc.DaUID, DataHubConfig.Opc.DaPWD);

            _server.Connect(new ConnectData(credential));
            _server.ServerShutdown += OnServerShutdown;

            Log.DaServer("Connect Opc.Da.Server succeeded.");
        }

        private void AddSubscription()
        {
        }

        private void WriteWatchDog()
        {
            if (DataHubConfig.Health.WDTDataTags == null || DataHubConfig.Health.WDTDataTags.Count == 0) return;

            try
            {
                List<ItemValue> itemValues = new List<ItemValue>();
                foreach (string tag in DataHubConfig.Health.WDTDataTags)
                {
                    itemValues.Add(new ItemValue
                    {
                        ClientHandle = Guid.NewGuid(),
                        ItemPath = string.Empty,
                        ItemName = tag,
                        Value = DataHubConfig.Health.WDTTagValue
                    });
                }

                Log.WDT(itemValues);
                IdentifiedResult[] writeResults = _server.Write(itemValues.ToArray());
                Log.WDT(writeResults);

                if (writeResults != null && writeResults.Any(r => r.ResultID == ResultID.S_OK))
                {
                    Log.WDT("Succeeded.");
                    TimeOfTheLastSuccessfulWDT = DateTime.Now;
                }
                else
                {
                    Log.WDT("Failed.");
                }
            }
            catch (Exception ex)
            {
                Log.WDT("WriteWatchDog error occurred.", ex);
                Log.Error("WriteWatchDog error occurred.", ex);
            }
        }

        #endregion

        #region IDisposable Methods

        public void Dispose()
        {
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
                Log.Error("DaServer.Dispose(), dispose _server failed.", ex);
            }

            try
            {
                _wdtTimer?.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("DaServer.Dispose(), dispose _timer failed.", ex);
            }

            _server = null;
            _wdtTimer = null;
        }

        #endregion
    }
}