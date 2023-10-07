using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;

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

        public ICommand SwitchWindowCommand { get; set; }
        public ICommand ShowMessageCommand { get; set; }

        public MenuWindowViewModel()
        {
            SwitchWindowCommand = new RelayCommand(SwitchWindow);
            ShowMessageCommand = new RelayCommand(() => MessageBox.Show("Это функция пока недоступна"));
        }

        private void SwitchWindow()
        {
            var secondWindow = new RecordsWindow();
            var viewModel = new RecordsWindowViewModel();
            secondWindow.DataContext = viewModel;
            Application.Current.MainWindow.Close();
            secondWindow.Show();
        }
    }
}
