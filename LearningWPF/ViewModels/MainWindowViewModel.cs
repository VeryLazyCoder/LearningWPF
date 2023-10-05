using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LearningWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Window Colors
        DispatcherTimer _timer;
        List<Brush> _brushes = new()
        {
            Brushes.Red,
            Brushes.Orange,
            Brushes.Orchid,
            Brushes.Aqua,
            Brushes.Green,
        };
        private Brush _color;

        public Brush Color
        {
            get => _color;
            set => Set(ref _color, value);
        } 
        #endregion
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
            #region Timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += (a, b) => Color = _brushes[new Random().Next(0, _brushes.Count)];
            _timer.Start(); 
            #endregion
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
