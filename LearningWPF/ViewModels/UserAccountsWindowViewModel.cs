using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.Models.DBData;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningWPF.ViewModels
{
    internal class UserAccountsWindowViewModel : ViewModel
    {
        public ObservableCollection<UserAccount> Accounts { get; }

        private bool _isCreatingAccountMode;
        private bool _isAnyAccountAvailable;
        private string _nickName;
        private readonly AuthorizationHandler _handler;

        public UserAccount ChosenAccount
        {
            get => _chosenAccount;
            set => Set(ref _chosenAccount, value);
        }

        public bool IsCreatingAccountMode
        {
            get => _isCreatingAccountMode;
            set => Set(ref _isCreatingAccountMode, value);
        }

        public bool IsAnyAccountAvailable
        {
            get => _isAnyAccountAvailable;
            set => Set(ref _isAnyAccountAvailable, value);
        }

        public string NickName
        {
            get => _nickName;
            set => Set(ref _nickName, value);
        }

        public ICommand ConfirmChosenAccountCommand { get; }
        public ICommand CreateNewAccountCommand { get; }
        public ICommand BackToAccountsCommand { get; }
        public ICommand ConfirmNewAccountCommand { get; }
        public ICommand SwitchToMenuScreenCommand { get; }


        private UserAccount _chosenAccount;
        public UserAccountsWindowViewModel(AuthorizationHandler handler)
        {
            _handler = handler;
            var accounts = _handler.GetConnectedToUserAccounts();
            if (accounts.Any())
            {
                ChosenAccount = accounts[0];
                IsAnyAccountAvailable = true;
            }
            Accounts = new ObservableCollection<UserAccount>(accounts);
            ConfirmChosenAccountCommand =
                new RelayCommand(() =>
                {
                    if (!IsAnyAccountAvailable) return;
                    ChosenAccount.Register();
                    new SwitchToMenuWindowCommand().Execute(null);
                });
            CreateNewAccountCommand = new RelayCommand(() => IsCreatingAccountMode = true);
            BackToAccountsCommand = new RelayCommand(() => IsCreatingAccountMode = false);
            ConfirmNewAccountCommand = new RelayCommand(async () =>
            {
                try
                {
                    await Task.Run(() => _handler.CreateAndChooseGameAccount(NickName));
                    new SwitchToMenuWindowCommand().Execute(null);
                }
                catch (Exception e)
                {
                    new ShowMessageBoxCommand().Execute(e.Message);
                }
            });
            SwitchToMenuScreenCommand = new RelayCommand(() =>
            {
                if (IsCreatingAccountMode)
                    ConfirmNewAccountCommand.Execute(null);
                else
                    ConfirmChosenAccountCommand.Execute(null);
            });
        }

        public UserAccountsWindowViewModel()
        {
        }
    }
}
