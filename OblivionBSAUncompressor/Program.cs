using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OblivionBSAUncompressor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var fileName = AppDomain.CurrentDomain.FriendlyName.ToLower();
            var currentGame = CurrentGame.Oblivion;
            if (Regex.IsMatch(fileName, @"(fallout|fo)\s*3"))
            {
                currentGame = CurrentGame.Fallout3;
            }
            else if (Regex.IsMatch(fileName, @"(fallout|fo|f)\s*(new\s*vegas|nv)"))
            {
                currentGame = CurrentGame.FalloutNV;
            }
            else if (fileName.IndexOf("skyrim") != -1)
            {
                currentGame = CurrentGame.Skyrim;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(currentGame));
        }
    }
}
