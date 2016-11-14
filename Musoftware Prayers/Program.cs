using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Musoftware_Prayers
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Frmmain MainForm = new Frmmain();
            foreach (string arg in args)
            {
                if (arg == @"/hide")
                {
                    MainForm.WindowState = FormWindowState.Minimized;
                    MainForm.Visible = false;
                    MainForm.ShowInTaskbar = false;
                }
            }
            Application.Run(MainForm);
        }
    }
}
