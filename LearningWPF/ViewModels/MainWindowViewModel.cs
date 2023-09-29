using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;

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
            set
            {
                Set(ref _mapID, value);
            }
        }

        public ICommand ShowRecordsCommand { get; private set; }

        public MainWindowViewModel()
        {
            ShowRecordsCommand = new RelayCommand(async () => await ShowRecordsAsync());
        }

        private async Task ShowRecordsAsync()
        {
            Title = "Подождите, рекорды загружаются...";

            var records = await Task.Run(() => RecordsRepository.LoadRecords(_mapID));

            string message = string.Join(Environment.NewLine, records.Select(x => x.ToString()));
            Title = message;
        }
    }
}
