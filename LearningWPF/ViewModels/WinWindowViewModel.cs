using LearningWPF.ViewModels.ViewModelBase;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System;
using LearningWPF.Models;
using LearningWPF.Infrastructure;

namespace LearningWPF.ViewModels
{
    internal class WinWindowViewModel : ViewModel
    {
        public ICommand UpdateRecordDbCommand { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
                _staticName = value;
            }
        }

        public string ButtonContext => "Записать рекорд";

        private static int _mapVariant;
        private static string _staticName;
        private static int _score;
        private string _name;

        public string Text => $"Ты победил за {_score} ходов, " +
                              $"если хочешь увековечить себя в базе данных, введи своё имя и нажми кнопку";

        public WinWindowViewModel() { }

        public WinWindowViewModel(int mapVariant, int score)
        {
            _mapVariant = mapVariant;
            _score = score;
            UpdateRecordDbCommand = new RelayCommand(AddRecordToDb);
        }

        private static void AddRecordToDb()
        {
            var userData = new UserData(_staticName, _score, DateTime.Now);
            RecordsRepository.UpdateBase(userData, _mapVariant);
            new SwitchToMenuWindowCommand().Execute(userData);
        }
    }
}
