using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.Reflection;

namespace TestWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// If you run as console app then exclude Program.cs file
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                //new Scheduler()
                new FileInputMonitorService()
            };
            ServiceBase.Run(ServicesToRun);

            //try
            //{
            //    if (Environment.UserInteractive)
            //        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
            //    else
            //        ServiceBase.Run(new Scheduler());

            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        


        /*
         In the end, just to test whether or not everything went smoothly,
         copy the bin folder from your default Visual Studio Project Directory 
         (if it's on same drive as your Windows drive) and paste it on some other drive.
         Right click on your application.exe file and run it as ADMINISTRATOR. To verify whether 
         or not it's installed and running, click Window+R and type services.msc,
         press enter, it will open up the window listing all the installed services. Now, 
         search for your service name and see if it's installed and running.


         In the end, if you want to remove the Windows Service from your machine,  
         open command prompt as administrator, go to the path from where you installed it and type in:

        Command: sc delete ServiceName

         */
    }
}
