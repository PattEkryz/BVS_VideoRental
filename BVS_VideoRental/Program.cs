using System;
using System.Windows.Forms;

namespace BVS_VideoRental
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ===== 1. Basic Windows Forms Setup =====
            Application.SetHighDpiMode(HighDpiMode.SystemAware); // Better high-DPI display support
            Application.EnableVisualStyles();                    // Modern UI styling
            Application.SetCompatibleTextRenderingDefault(false); // Better text rendering
            Application.SetDefaultFont(new System.Drawing.Font("Segoe UI", 9f)); // Consistent font

            // ===== 2. Global Error Handling =====
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ShowError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                ShowError(e.ExceptionObject as Exception);

            // ===== 3. Optional Splash Screen =====
            // Uncomment if you want a splash screen
            /*
            using (var splash = new SplashForm())
            {
                splash.ShowDialog(); // Show splash first
            }
            */

            // ===== 4. Launch Login Form =====
            Application.Run(new LoginForm());
        }

        // Helper method to show errors
        private static void ShowError(Exception ex)
        {
            MessageBox.Show(
                $"An unexpected error occurred:\n{ex?.Message}",
                "Application Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );

            // Log the error (you could add file/DB logging here)
            // System.IO.File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
        }
    }
}