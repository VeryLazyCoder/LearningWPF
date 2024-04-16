using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningWPF.ViewModels
{
    internal class RecordsWindowViewModel : ViewModel
    {
        public ObservableCollection<string> Filters { get; } = new ()
        {
            "Показывать рекорды только этого аккаунта",
            "Показывать все ваши рекорды",
            "Показывать рекорды других пользователей",
        };
        private string _title = "Нажмите кнопку и здесь будут рекорды";
        private string _chosenFilter;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string ChosenFilter
        {
            get => _chosenFilter;
            set => Set(ref _chosenFilter, value);
        }

        public ICommand ShowRecordsCommand { get; private set; }

        public RecordsWindowViewModel() 
        {
            ShowRecordsCommand =
                new RelayCommand<string>(async (a) => await ShowRecordsAsync(int.Parse(a)));
            ChosenFilter = Filters[0];
        }

        private async Task ShowRecordsAsync(int mapID)
        {
            Title = "Подождите, рекорды загружаются...";

            var records = await Task.Run(() => RecordsRepository.LoadRecords(mapID, ChosenFilter));

            var message = string.Join(Environment.NewLine, records.Select(x => x.ToString()));
            Title = message;
        }
    }
}
