using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpcHub.Da.Service.Utils;

namespace OpcHub.Da.Service.Health
{
    public class HealthMonitor : IDisposable
    {
        #region Fields
        private readonly SafeTimer _timer;
        #endregion
        
        static HealthMonitor()
        {
            Current = new HealthMonitor();
        }

        public static HealthMonitor Current { get; }

        #region IDisposable Methods
        public void Dispose()
        {
        }
        #endregion
    }
}
