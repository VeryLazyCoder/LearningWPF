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

        public ICommand SetRecordsWindowCommand { get; set; } = new RelayCommand(SwitchRecordsWindow);
        public ICommand SetGameWindowCommand { get; set; } = new RelayCommand(SwitchGameWindow);
        public ICommand ShowMessageCommand { get; set; } =
            new RelayCommand(() => MessageBox.Show("Это функция пока недоступна"));

        private static void SwitchRecordsWindow()
        {
            var secondWindow = new RecordsWindow();
            var viewModel = new RecordsWindowViewModel();
            secondWindow.DataContext = viewModel;
            Application.Current.MainWindow?.Close();
            secondWindow.Show();
        }

        private static void SwitchGameWindow()
        {
            var secondWindow = new GameWindow();
            var viewModel = new GameWindowViewModel();
            secondWindow.DataContext = viewModel;
            Application.Current.MainWindow?.Close();
            secondWindow.Show();
        }
    }
}
