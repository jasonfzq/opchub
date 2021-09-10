using Microsoft.Owin.Hosting;
using OpcHub.Da.Service.Configs;
using OpcHub.Da.Service.Health;
using System;
using System.ServiceProcess;
using OpcHub.Da.Service.Hub;

namespace OpcHub.Da.Service
{
    partial class ServiceX : ServiceBase
    {
        private IDisposable _webApp;
        private DataHub _dataHub;
        private HealthMonitor _healthMonitor;

        public ServiceX()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
                StartService();
            }
            catch (Exception ex)
            {
                DisposeResources();

                Log.Error("ServiceX.StartService failed.", ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        public void StartService()
        {
            DataHubConfig.Initialize();

            _webApp = WebApp.Start<Startup>(url: $"{DataHubConfig.ApiUrl}");
            _dataHub = DataHub.Current;
            _healthMonitor = HealthMonitor.Current;

            Log.Info("OpcHub.Da.Service started.");
        }

        public void StopService()
        {
            DisposeResources();
            Log.Info("OpcHub.Da.Service stopped.");
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("AppDomain.CurrentDomain.UnhandledException", e.ExceptionObject as Exception);
        }

        private void DisposeResources()
        {
            #region DataHub
            try
            {
                _dataHub.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("ServiceX.DisposeResources, dispose _dataHub failed.", ex);
            }
            finally
            {
                _dataHub = null;
            }
            #endregion

            #region WebApp
            try
            {
                _webApp.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("ServiceX.DisposeResources, dispose _webApp failed.", ex);
            }
            finally
            {
                _webApp = null;
            }
            #endregion

            #region HealthMonitor
            try
            {
                _healthMonitor.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("ServiceX.DisposeResources, dispose _healthMonitor failed.", ex);
            }
            finally
            {
                _healthMonitor = null;
            }
            #endregion
        }
    }
}
