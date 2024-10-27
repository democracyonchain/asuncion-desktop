using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsuncionDesktop.Presentation.Forms
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 1200; // 3 segundos
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            this.Close();
        }

        private void SplashScreenForm_Load(object sender, EventArgs e)
        {

        }
    }

}
