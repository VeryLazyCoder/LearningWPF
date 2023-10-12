using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearningWPF.ViewModels
{
    internal class RecordsWindowViewModel : ViewModel
    {
        private string _title = "Нажмите кнопку и здесь будут рекорды";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public ICommand ShowRecordsWithoutConverter { get; private set; }
        public ICommand SwitchWindowCommand { get; set; }

        

        private void SwitchWindow()
        {
            var secondWindow = new MenuWindow();
            var viewModel = new MenuWindowViewModel();
            secondWindow.DataContext = viewModel;
            secondWindow.Show();
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
        }

        public RecordsWindowViewModel() 
        {
            ShowRecordsWithoutConverter =
                new RelayCommand<string>(async (a) => await ShowRecordsAsync(int.Parse(a)));
            SwitchWindowCommand = new RelayCommand(SwitchWindow);
        }

        private async Task ShowRecordsAsync(int mapID)
        {
            Title = "Подождите, рекорды загружаются...";

            var records = await Task.Run(() => RecordsRepository.LoadRecords(mapID));

            var message = string.Join(Environment.NewLine, records.Select(x => x.ToString()));
            Title = message;
        }
    }
}
