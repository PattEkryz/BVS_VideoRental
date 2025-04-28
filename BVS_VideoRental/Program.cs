using System;
using System.Windows.Forms;
using BVS_VideoRental.Forms;

namespace BVS_VideoRental
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show splash screen or loading form if needed
            // Application.Run(new LoadingForm());

            // Show login form first
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK && loginForm.AuthenticatedUser != null)
            {
                // Based on user role, show appropriate form
                if (loginForm.AuthenticatedUser.Role == "Admin")
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    Application.Run(new ClientForm());
                }
            }
        }
    }
}