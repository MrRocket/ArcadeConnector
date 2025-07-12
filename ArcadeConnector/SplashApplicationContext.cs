using ArcadeConnector;
using System.Windows.Forms;

public class SplashApplicationContext : ApplicationContext
{
    private frmSplash splash;
    private Form1 mainForm;

    public SplashApplicationContext()
    {
        splash = new frmSplash();
        splash.SplashComplete += (s, e) =>
        {
            splash.Close();
            splash.Dispose();

            mainForm = new Form1();
            mainForm.FormClosed += (s2, e2) => ExitThread();
            mainForm.Show();
        };

        splash.Show();
    }
}
