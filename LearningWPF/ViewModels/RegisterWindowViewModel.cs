using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.Models.DBData;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
using static System.Formats.Asn1.AsnWriter;

namespace LearningWPF.ViewModels
{
    internal class RegisterWindowViewModel : ViewModel
    {
        private string _userName;
        private string _password;
        private string _userLogin;
        private bool _isRegisterMode;
        private bool _isAuthorizationMode;
        private bool _isStartMode = true;

        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string UserLogin
        {
            get => _userLogin;
            set => Set(ref _userLogin, value);
        }

        public bool IsRegisterMode
        {
            get => _isRegisterMode;
            set => Set(ref _isRegisterMode, value);
        }

        public bool IsAuthorizationMode
        {
            get => _isAuthorizationMode;
            set => Set(ref _isAuthorizationMode, value);
        }

        public bool IsStartMode
        {
            get => _isStartMode;
            set => Set(ref _isStartMode, value);
        }

        public ICommand SignIn { get; }
        public ICommand SignUp { get; }
        public ICommand ToAuthorizationGrid { get; }
        public ICommand ToRegistrationGrid { get; }
        public ICommand ToStartGrid { get; }
        public ICommand MoveFocusCommand { get; }

        public RegisterWindowViewModel()
        {
            ToRegistrationGrid = new RelayCommand(() =>
            {
                IsStartMode = false;
                IsRegisterMode = true;
            });

            ToAuthorizationGrid = new RelayCommand(() =>
            {
                IsStartMode = false;
                IsAuthorizationMode = true;
                var loginBox = Application.Current.MainWindow.FindName("LoginBox") as TextBox;
                loginBox.Focus();
            });

            ToStartGrid = new RelayCommand(() =>
            {
                IsAuthorizationMode = false;
                IsRegisterMode = false;
                IsStartMode = true;
            });

            SignIn = new RelayCommand(async () =>
            {

                try
                {
                    var handler = await Task.Run(() => AuthorizationHandler.SignIn(UserLogin, Password));
                    SwitchToUserAccountsWindow(handler);
                }
                catch (Exception e)
                {
                    new ShowMessageBoxCommand().Execute(e.Message);
                }
            });

            SignUp = new RelayCommand(() =>
            {
                try
                {
                    var handler = AuthorizationHandler.SignUp(UserName, UserLogin, Password);
                    new ShowMessageBoxCommand().Execute("Вы успешно зарегистрировались");
                    SwitchToUserAccountsWindow(handler);
                }
                catch (Exception e)
                {
                    new ShowMessageBoxCommand().Execute(e.Message);
                }
            });

            MoveFocusCommand = new RelayCommand<KeyEventArgsInfo>((e) =>
            {
                if (e.PressedKey != Key.Enter)
                    return;

                var window = Application.Current.MainWindow;
                switch (e.Source)
                {
                    case "LoginBox":
                    {
                        if (window.FindName("PasswordBox") is TextBox passwordTextBox)
                            passwordTextBox.Focus();
                        break;
                    }
                    case "PasswordBox":
                        SignIn.Execute(null);
                        break;
                }
            });
        }

        private void SwitchToUserAccountsWindow(AuthorizationHandler handler)
        {
            var viewModel = new UserAccountsWindowViewModel(handler);
            var secondWindow = new UserAccountsWindow()
            {
                DataContext = viewModel
            };
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
            secondWindow.Show();
        }
    }
}
