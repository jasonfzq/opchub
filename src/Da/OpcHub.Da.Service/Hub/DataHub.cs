using System;
using System.Collections.Generic;
using System.Linq;
using Opc;
using Opc.Da;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Service.Hub
{
    public class DataHub
    {
        #region Fields

        private DaServer _server;
        private DaServerInitializeState _initializeState;
        private static readonly object DA_SERVER_INIT_LOCKER = new object();

        #endregion

        #region Ctor

        static DataHub()
        {
            Current = new DataHub();
        }

        private DataHub()
        {
            _server = InitializeDaServer();
            _initializeState = DaServerInitializeState.Done;
        }

        #endregion

        #region Properties

        //public IDaServer Server { get; private set; }
        public static DataHub Current { get; }

        #endregion

        #region Methods

        public void ReCreateDaServer()
        {
            lock (DA_SERVER_INIT_LOCKER)
            {
                _initializeState = DaServerInitializeState.Initializing;

                DaServer newServer = null;
                try
                {
                    Log.DaServer("Start to re-create Hub.DaServer.");
                    newServer = new DaServer(OnDaServerShutdown);

                    // Dispose the existing server
                    DisposeDaServer();
                    Log.DaServer("The existing Hub.DaServer instance is disposed.");

                    // To use the new server
                    _server = newServer;
                    Log.DaServer("The new Hub.DaServer has replaced the existing instance.");
                }
                catch (Exception ex)
                {
                    Log.DaServer("DataHub.ReCreateDaServer failed.", isError: true);
                    Log.Error("DataHub.ReCreateDaServer failed.", ex);

                    newServer?.Dispose();
                    newServer = null;
                }
                finally
                {
                    _initializeState = DaServerInitializeState.Done;
                }
            }
        }

        public bool IsDaServerCreated(out string failedReason)
        {
            failedReason = null;
            if (_server == null)
                failedReason = "DaServer instance in DataHub isn't created or it has been disposed.";

            return _server != null;
        }

        public bool TryGetDaServerStatus(out ServerStatus serverStatus, out string failedReason)
        {
            serverStatus = null;
            failedReason = null;

            bool isSucceeded = true;
            if (_server == null)
            {
                failedReason = "DaServer instance in DataHub has been disposed.";
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

        public ServerStatus GetDaServerStatus()
        {
            return _server?.GetServerStatus();
        }

        public DaServerInitializeState GetDaServerInitializeState()
        {
            lock (DA_SERVER_INIT_LOCKER)
            {
                return _initializeState;
            }
        }

        #endregion

        #region Read & Write

        public ReadCommandResult Read(List<string> tags, bool shortPolling)
        {
            if (tags == null || tags.Count == 0) return null;
            tags = tags.Distinct().ToList();

            List<Item> items = tags.Select(tag => tag.ToOpcType()).ToList();
            List<ItemValueResult> results = _server.Read(items, shortPolling);

            bool succeeded = results.Count == items.Count && results.All(result => result.ResultID == ResultID.S_OK);
            List<ReadItemValue> itemValues = results.Select(r => new ReadItemValue
            {
                ItemName = r.ItemName,
                ItemPath = r.ItemPath,
                ClientHandle = r.ClientHandle,
                ServerHandle = r.ServerHandle,
                ItemValue = r.Value,
                Timestamp = r.Timestamp
            }).ToList();

            return succeeded
                ? ReadCommandResult.Successful(itemValues)
                : ReadCommandResult.Failed(
                    itemValues,
                    results.Where(result => result.ResultID != ResultID.S_OK)
                        .Select(result => new ItemCommandFailureReason(result.ItemName, result.ResultID.ToString()))
                        .ToList());
        }

        public WriteCommandResult Write(List<WriteItemValue> itemValues, bool shortPolling)
        {
            if (itemValues == null || itemValues.Count == 0) return null;

            List<ItemValue> items = itemValues.Select(iv => iv.ToOpcType()).ToList();
            List<IdentifiedResult> results = _server.Write(items, shortPolling);

            bool succeeded = results.Count == items.Count && results.All(result => result.ResultID == ResultID.S_OK);
            return succeeded
                ? WriteCommandResult.Successful()
                : WriteCommandResult.Failed(results
                    .Where(result => result.ResultID != ResultID.S_OK)
                    .Select(result => new ItemCommandFailureReason(result.ItemName, result.ResultID.ToString()))
                    .ToList());
        }

        #endregion

        #region Private Methods

        private DaServer InitializeDaServer()
        {
            DaServer daServer = null;
            try
            {
                Log.DaServer("Start to create Hub.DaServer.");
                daServer = new DaServer(OnDaServerShutdown);
            }
            catch (Exception ex)
            {
                Log.DaServer("DataHub.InitializeDaServer failed.", isError: true);
                Log.Error("DataHub.InitializeDaServer failed.", ex);

                daServer?.Dispose();
                daServer = null;
            }

            return daServer;
        }

        private void OnDaServerShutdown(string reason)
        {
            // ServerShutdown event is caught for information only, it won't be triggered to HealthMonitor
            DisposeDaServer();
        }

        #endregion

        #region IDisposable Methods

        public void Dispose()
        {
            Log.DaServer("Start to dispose DataHub instance.");
            DisposeDaServer();
        }

        private void DisposeDaServer()
        {
            _server?.Dispose();
            _server = null;
            Log.DaServer("DaServer instance in DataHub has been disposed.");
        }

        #endregion
    }

    public enum DaServerInitializeState
    {
        Initializing,
        Done
    }
}