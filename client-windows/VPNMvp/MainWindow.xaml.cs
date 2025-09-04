using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace VPNMvp
{
    public partial class MainWindow : Window
    {
        private static readonly string ErrorLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VPNMvp_startup_error.log");

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            // only run once
            Loaded -= MainWindow_Loaded;

            // Schedule resolving the SignInView after the window has rendered
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    // Resolve via DI (may throw if there's a problem in the view/viewmodel)
                    var signInView = App.Services.GetRequiredService<Views.SignInView>();
                    MainContent.Content = signInView;
                }
                catch (Exception ex)
                {
                    // Log and show a helpful message so you can paste the details here
                    var text = $"{DateTime.Now:O} - Error resolving SignInView: {ex}{Environment.NewLine}";
                    try { File.AppendAllText(ErrorLog, text); } catch { /* ignore */ }

                    MessageBox.Show($"Error loading SignIn view:\n{ex.Message}\n\nFull details saved to:\n{ErrorLog}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
