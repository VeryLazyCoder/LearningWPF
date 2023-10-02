using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearningWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _title = "Нажмите кнопку и здесь будут рекорды";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private int _mapID;

        public int SelectedMapId
        {
            get => _mapID;
            set => Set(ref _mapID, value);
        }

        public ICommand ShowRecordsCommand { get; private set; }
        public ICommand CloseApplicationCommand { get; private set; }
        public ICommand ShowRecordsWithoutConverter { get; private set; }

        public MainWindowViewModel()
        {
            ShowRecordsCommand = new RelayCommand(async () => await ShowRecordsAsync());
            CloseApplicationCommand = new RelayCommand(Application.Current.Shutdown);
            ShowRecordsWithoutConverter = new RelayCommand<string>(async (a) => await ShowRecordsAsync(int.Parse(a)));
        }

        private async Task ShowRecordsAsync()
        {
            Title = "Подождите, рекорды загружаются...";

            var records = await Task.Run(() => RecordsRepository.LoadRecords(_mapID));

            string message = string.Join(Environment.NewLine, records.Select(x => x.ToString()));
            Title = message;
        }

        private async Task ShowRecordsAsync(int mapID)
        {
            Title = "Подождите, рекорды загружаются...";

            var records = await Task.Run(() => RecordsRepository.LoadRecords(mapID));

            string message = string.Join(Environment.NewLine, records.Select(x => x.ToString()));
            Title = message;
        }
    }
}
