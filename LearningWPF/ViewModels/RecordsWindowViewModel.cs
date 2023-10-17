using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public RecordsWindowViewModel() 
        {
            ShowRecordsWithoutConverter =
                new RelayCommand<string>(async (a) => await ShowRecordsAsync(int.Parse(a)));
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
