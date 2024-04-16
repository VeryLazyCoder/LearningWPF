using LearningWPF.ViewModels.ViewModelBase;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using LearningWPF.Models;
using LearningWPF.Infrastructure;
using LearningWPF.Models.DBData;

namespace LearningWPF.ViewModels
{
    internal class WinWindowViewModel : ViewModel
    {
        public ICommand UpdateRecordDbCommand { get; private set; }

        public string ButtonContext => "Записать рекорд";

        public string TextToUser
        {
            get => _textToUser;
            private set => Set(ref _textToUser, value);
        }

        private readonly int _mapVariant;
        private readonly int _score;
        private readonly int _startMovesOnMap;
        private string _textToUser;
        private Dictionary<int, double> _percentageOfMovesSpentToUpLevel;
        private bool _isLevelUp;

        public WinWindowViewModel() { }

        public WinWindowViewModel(int mapVariant, int score, int startMovesOnMap) // 90 120 75% 
        {
            _mapVariant = mapVariant;
            _score = score;
            _startMovesOnMap = startMovesOnMap;
            UpdateRecordDbCommand = new RelayCommand(AddRecordToDb);
            _percentageOfMovesSpentToUpLevel = new Dictionary<int, double>()
            {
                [1] = 0.9,
                [2] = 0.85,
                [3] = 0.8,
                [4] = 0.75,
                [5] = 0.7,
                [6] = 0.6,
                [7] = 0.5,
                [8] = 0.35,
                [9] = 0.2,
            };
            FormMessageToPlayer();
        }

        private void AddRecordToDb()
        {
            var userData = new UserData(_score, DateTime.Now, _mapVariant, _isLevelUp);
            RecordsRepository.UpdateBase(userData);
            new SwitchToMenuWindowCommand().Execute(userData);
        }

        private void FormMessageToPlayer()
        {
            var percentageOfMovesSpent = (double)_score / _startMovesOnMap;
            var userLevel = CurrentUser.Account.Level;
            _isLevelUp = percentageOfMovesSpent <= _percentageOfMovesSpentToUpLevel[userLevel];

            if (_isLevelUp)
                TextToUser = $"Вы победили за {_score} ходов. И повысили свой уровень до {userLevel + 1}. " +
                             $"Нажмите 'Записать рекорд', чтобы обновить информацию в базе";
            else
                TextToUser =
                    $"Вы победили за {_score} ходов. Чтобы повысить уровень аккаунта необходимо закончить игру" +
                    $" используя не более {_percentageOfMovesSpentToUpLevel[userLevel] * 100}% ходов " +
                    $"Нажмите 'Записать рекорд', чтобы обновить информацию в базе";
        }
    }
}
