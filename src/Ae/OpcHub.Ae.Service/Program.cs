using System;
using System.ServiceProcess;

namespace OpcHub.Ae.Service
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            ServiceX s = new ServiceX();
            s.StartService();

            Console.WriteLine(@"OpcHub.Ae.Service is started......");
            Console.ReadLine();

            s.StopService();
#else
            ServiceBase[] servicesToRun = new ServiceBase[] { new ServiceX() };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
