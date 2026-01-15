using System.Configuration;
using System.Data;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Defines the application entry point and startup behavior.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the application instance.
        /// </summary>
        public App()
        {
        }

        /// <summary>
        /// Handles application startup and opens the main window.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow().Show();
        }
    }
}
