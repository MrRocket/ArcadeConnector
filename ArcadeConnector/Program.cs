using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcadeConnector
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Aracde Connector program start up:
            // Load the splash handler, it runs the splash screen then fades out.
            // Closes and finally loads the main form.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SplashApplicationContext());
        }
    }
}

