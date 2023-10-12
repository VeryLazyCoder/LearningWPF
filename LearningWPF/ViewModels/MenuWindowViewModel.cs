using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
using System.Windows;
using System.Windows.Input;

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
        public ICommand SetConfigurationWindowCommand { get; set; } = new RelayCommand(SwitchConfigurationWindow);
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

        private static void SwitchConfigurationWindow()
        {
            var viewModel = new ConfigurationWindowViewModel();
            var secondWindow = new ConfigurationWindow()
            {
                DataContext = viewModel
            };
            Application.Current.MainWindow?.Close();
            secondWindow.Show();
        }
    }
}
