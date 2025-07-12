using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcadeConnector
{
    public partial class frmSplash : Form
    {
        public event EventHandler SplashComplete;

        public frmSplash()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0;

            // Start the fade-in 
            this.Load += async (s, e) =>
            {
                await FadeInAsync();
                await Task.Delay(1500); // Wait a bit after fade-in finishes
                OnSplashComplete();
            };
        }

        private async Task FadeInAsync()
        {
            while (this.Opacity < 1)
            {
                this.Opacity += 0.05;
                await Task.Delay(50);
            }
        }

        protected virtual void OnSplashComplete()
        {
            SplashComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}