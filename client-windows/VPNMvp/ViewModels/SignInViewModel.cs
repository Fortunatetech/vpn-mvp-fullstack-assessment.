using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VPNMvp.Services;

namespace VPNMvp.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        public event Action? LoginSucceeded;

        public SignInViewModel(IAuthService authService)
        {
            _authService = authService;
            SignInCommand = new RelayCommand(async _ => await SignInAsync(), _ => !IsBusy);
            // initialize backing field so nullable-checker is satisfied
            _signInButtonText = "Sign In";
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set { _email = value; Raise(nameof(Email)); }
        }

        // Password is set from code-behind (PasswordBox)
        public string Password { get; set; } = "";

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                Raise(nameof(IsBusy));
                // ensure command availability updates
                if (SignInCommand is RelayCommand rc) rc.RaiseCanExecuteChanged();
                SignInButtonText = _isBusy ? "Signing..." : "Sign In";
                Raise(nameof(SignInButtonText));
            }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; Raise(nameof(ErrorMessage)); }
        }

        // initialize backing field here to avoid nullable warning
        private string _signInButtonText = "Sign In";
        public string SignInButtonText
        {
            get => _signInButtonText;
            set { _signInButtonText = value; Raise(nameof(SignInButtonText)); }
        }

        public ICommand SignInCommand { get; }

        private async Task SignInAsync()
        {
            try
            {
                ErrorMessage = "";
                IsBusy = true;

                // basic local validation
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please enter email and password.";
                    return;
                }

                var ok = await _authService.LoginAsync(Email.Trim(), Password);
                if (ok)
                {
                    // success - invoke navigation event
                    LoginSucceeded?.Invoke();
                }
                else
                {
                    ErrorMessage = "Invalid credentials.";
                }
            }
            catch (Exception ex)
            {
                // show small friendly message
                App.Current.Dispatcher.Invoke(() => MessageBox.Show($"Sign-in failed: {ex.Message}", "Error"));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
