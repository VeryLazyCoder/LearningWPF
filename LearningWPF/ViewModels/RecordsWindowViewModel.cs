using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
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
    internal class RecordsWindowViewModel : ViewModel
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

        public ICommand ShowRecordsWithoutConverter { get; private set; }
        public ICommand SwitchWindowCommand { get; set; }

        

        private void SwitchWindow()
        {
            var secondWindow = new MenuWindowxaml();
            var viewModel = new MenuWindowViewModel();
            secondWindow.DataContext = viewModel;
            secondWindow.Show();
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
        }

        public MenuWindowViewModel MenuViewModel { get; set; }
        public RecordsWindowViewModel() 
        {
            ShowRecordsWithoutConverter =
                new RelayCommand<string>(async (a) => await ShowRecordsAsync(int.Parse(a)));
            SwitchWindowCommand = new RelayCommand(SwitchWindow);
            #region Timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += (a, b) => Color = _brushes[new Random().Next(0, _brushes.Count)];
            _timer.Start(); 
            #endregion
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
