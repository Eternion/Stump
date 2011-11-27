using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stump.Tools.ClientPatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled Exception : \n" + e.ExceptionObject, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
