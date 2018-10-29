using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestWindowsService
{
    partial class FileInputMonitorService : ServiceBase
    {
        private FileInputMonitor fileInputMonitor;

        public FileInputMonitorService()
        {
            InitializeComponent();
        }

        //for running as console app then need to exclude Program.cs and change project properties
        //private static void Main(string[] args)
        //{
        //    var service = new FileInputMonitorService();
        //    if (Environment.UserInteractive)
        //    {
        //        service.OnStart(args);
        //        Console.WriteLine("Press any key to stop the program");
        //        Console.Read();
        //        service.OnStop();
        //    }
        //    else
        //    {
        //        Run(service);
        //    }
        //}

        protected override void OnStart(string[] args)
        {
            Library.WriteErrorLog("Service is started for file watching");
            Library.LogEvent("Service is started for file watching at "+ DateTime.Now.ToString());
            fileInputMonitor = new FileInputMonitor();
        }

        protected override void OnStop()
        {
            fileInputMonitor.OnStopFileMonitoring();
            Library.WriteErrorLog("Service is stopping for file watching");
            Library.LogEvent("Service is stopping for file watching at "+ DateTime.Now.ToString());
        }
    }
}
