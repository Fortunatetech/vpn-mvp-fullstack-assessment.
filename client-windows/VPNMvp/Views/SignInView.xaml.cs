using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection; // << add
using VPNMvp.ViewModels;

namespace VPNMvp.Views
{
    public partial class SignInView : UserControl
    {
        private readonly SignInViewModel _vm;
        public SignInView()
        {
            InitializeComponent();
            _vm = App.Services.GetRequiredService<SignInViewModel>();
            DataContext = _vm;

            // wire password box to VM on change
            PasswordBox.PasswordChanged += (s, e) => _vm.Password = PasswordBox.Password;

            _vm.LoginSucceeded += OnLoginSucceeded;
        }

        private void OnLoginSucceeded()
        {
            // navigate to NodeListView
            var nodeList = App.Services.GetRequiredService<NodeListView>();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    var cc = mainWindow.FindName("MainContent") as ContentControl;
                    if (cc != null) cc.Content = nodeList;
                    else mainWindow.Content = nodeList;
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // click handled by command; this exists so PasswordBox updates earlier
        }
    }
}
