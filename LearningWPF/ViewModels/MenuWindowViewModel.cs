using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;

namespace LearningWPF.ViewModels
{
    internal class MenuWindowViewModel : ViewModel
    {
        private string _text = "Перейти к рекордам";

        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }

        public ICommand SetRecordsWindowCommand { get; set; }
        public ICommand SetGameWindowCommand { get; set; }
        public ICommand ShowMessageCommand { get; set; }

        public MenuWindowViewModel()
        {
            SetRecordsWindowCommand = new RelayCommand(SwitchRecordsWindow);
            ShowMessageCommand = new RelayCommand(() => MessageBox.Show("Это функция пока недоступна"));
            SetGameWindowCommand = new RelayCommand(SwitchGameWindow);
        }

        private void SwitchRecordsWindow()
        {
            var secondWindow = new RecordsWindow();
            var viewModel = new RecordsWindowViewModel();
            secondWindow.DataContext = viewModel;
            Application.Current.MainWindow.Close();
            secondWindow.Show();
        }

        private void SwitchGameWindow()
        {
            var secondWindow = new GameWindow();
            var viewModel = new GameWindowViewModel();
            secondWindow.DataContext = viewModel;
            Application.Current.MainWindow.Close();
            secondWindow.Show();
        }
    }
}
