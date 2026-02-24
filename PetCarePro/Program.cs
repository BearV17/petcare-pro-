using System;
using System.Windows.Forms;

namespace PetCarePro
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Initialize database
            Data.DatabaseHelper.InitializeDatabase();
            
            // Show login form first
            Application.Run(new Forms.LoginForm());
        }
    }
}
