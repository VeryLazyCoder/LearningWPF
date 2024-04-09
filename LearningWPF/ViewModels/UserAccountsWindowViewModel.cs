using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Models;
using LearningWPF.Models.DBData;

namespace LearningWPF.ViewModels
{
    internal class UserAccountsWindowViewModel : ViewModel
    {

        public ObservableCollection<UserAccount> Accounts { get; }

        private bool _isCreatingAccountMode = false;
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

        public string NickName
        {
            get => _nickName;
            set => Set(ref _nickName, value);
        }

        public ICommand ConfirmChosenAccount { get; }
        public ICommand CreateNewAccountCommand { get; }
        public ICommand BackToAccountsCommand { get; }
        public ICommand ConfirmNewAccountCommand { get; }


        private UserAccount _chosenAccount;
        public UserAccountsWindowViewModel(AuthorizationHandler handler)
        {
            _handler = handler;
            Accounts = new ObservableCollection<UserAccount>(_handler.GetConnectedToUserAccounts());
            ConfirmChosenAccount =
                new RelayCommand(() =>
                {
                    ChosenAccount.Register();
                    new SwitchToMenuWindowCommand().Execute(null);
                });
            CreateNewAccountCommand = new RelayCommand(() => IsCreatingAccountMode = true);
            BackToAccountsCommand = new RelayCommand(() => IsCreatingAccountMode = false);
            ConfirmNewAccountCommand = new RelayCommand(async() =>
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
        }

        public UserAccountsWindowViewModel()
        {

        }
    }
}
