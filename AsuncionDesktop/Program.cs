using AsuncionDesktop.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsuncionDesktop
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            SplashScreenForm splash = new SplashScreenForm();
            splash.ShowDialog();

            LoginForm login = new LoginForm();
            System.Windows.Forms.Application.Run(login);
            
            
        }
    }
}
