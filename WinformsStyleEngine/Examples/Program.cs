using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsStyleEngine;

namespace Examples
{
    static class Program
    {
        public static IStyleEngine StyleEngine { get; set; }
        private static Form _frmMain { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form _frmMain = new FormMain();
            StyleEngine = new StyleEngine();
            StyleEngine.ApplyStyle(_frmMain);
            _frmMain.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(_frmMain);
        }
    }
}
