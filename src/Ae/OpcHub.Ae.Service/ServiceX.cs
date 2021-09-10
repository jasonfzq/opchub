using Microsoft.Owin.Hosting;
using OpcHub.Ae.Service.Configs;
using OpcHub.Ae.Service.Health;
using OpcHub.Ae.Service.Hub;
using System;
using System.ServiceProcess;

namespace OpcHub.Ae.Service
{
    partial class ServiceX : ServiceBase
    {
        private IDisposable _webApp;
        private AeEventHub _eventHub;
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
            EventHubConfig.Initialize();

            _webApp = WebApp.Start<Startup>(url: $"{EventHubConfig.ApiUrl}");
            _eventHub = AeEventHub.Current;
            _healthMonitor = HealthMonitor.Current;

            Log.Info("OpcHub.Ae.Service started.");
        }

        public void StopService()
        {
            DisposeResources();
            Log.Info("OpcHub.Ae.Service stopped.");
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("AppDomain.CurrentDomain.UnhandledException", e.ExceptionObject as Exception);
        }

        private void DisposeResources()
        {
            #region EventHub
            try
            {
                _eventHub.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("ServiceX.DisposeResources, dispose _eventHub failed.", ex);
            }
            finally
            {
                _eventHub = null;
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
