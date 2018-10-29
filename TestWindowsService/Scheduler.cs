using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TestWindowsService
{
    public partial class Scheduler : ServiceBase
    {
        private Timer timer1 = null;
        public Scheduler()
        {
            InitializeComponent();
        }

        ////for running as console app then need to exclude Program.cs and change project properties
        //private static void Main(string[] args)
        //{
        //    var service = new Scheduler();
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
            this.timer1 = new Timer();
            this.timer1.Interval = 30000; //every 30 secs
            this.timer1.Elapsed += Timer1_Elapsed;
            this.timer1.Enabled = true;

            //========== for debug purpose ============
            //#if DEBUG
            //base.RequestAdditionalTime(600000); // 600*1000ms = 10 minutes timeout
            //Debugger.Launch(); // launch and attach debugger
            //#endif
            Library.WriteErrorLog("Test window service starte");

            //base.OnStart(args);
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Write code here to do some job depends on your requirement

            Library.WriteErrorLog("Timer ticked and some job has been done successfully");
            
        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            Library.WriteErrorLog("Test window service stopped");
        }
    }
}
