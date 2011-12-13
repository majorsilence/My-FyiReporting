using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;

namespace fyiReporting.RdlDesign
{
    public class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string version = "403";// !!!! warning  !!!! string needs to be changed with when release version changes
           
            string ipcChannelPortName = string.Format("RdlProject{0}", version); 
            // Determine if an instance is already running?
            bool firstInstance;
            string mName = string.Format("Local\\RdlDesigner{0}", version);         
            //   can't use Assembly in this context
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, mName, out firstInstance);
         
            if (firstInstance)
            {   // just start up the designer when we're first in line
                Application.EnableVisualStyles();
                Application.DoEvents();
                Application.Run(new RdlDesigner(ipcChannelPortName, true));
                return;
            }

            // Process already running.   Notify other process that is might need to open another file
            string[] args = Environment.GetCommandLineArgs();

            IpcChannel clientChannel = new IpcChannel("RdlClientSend");
            ChannelServices.RegisterChannel(clientChannel);

            RdlIpcObject ipc =
            (RdlIpcObject)Activator.GetObject(
            typeof(RdlIpcObject),
            string.Format("ipc://{0}/IpcCommands", ipcChannelPortName));


            List<string> commands = new List<string>();


            commands.Add("/a");
            //sw.WriteLine("/a"); // signal that application should activate itself 
            // copy all the command line arguments process received
            for (int i = 1; i < args.Length; i++)
            {
                commands.Add(args[i]);
            }
            ipc.setCommands(commands);


        }

    }
}
