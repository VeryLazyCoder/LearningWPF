﻿using LearningWPF.ViewModels.ViewModelBase;
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

        private static int _mapVariant;
        private static string _staticName;
        private static int _score;
        private string _name;
        public string Text => "Это победа, если хочешь увековечить себя в базе данных, введи своё имя и нажми кнопку";
        public WinWindowViewModel() { }

        public WinWindowViewModel(int mapVariant, int score)
        {
            _mapVariant = mapVariant;
            _score = score;
            UpdateRecordDbCommand = new RelayCommand(OnButtonClick);
        }

        private static void OnButtonClick()
        {
            var userdata = new UserData(_staticName, _score, DateTime.Now);
            RecordsRepository.UpdateBase(userdata, _mapVariant);
            new SwitchToMenuWindowCommand().Execute(userdata);
        }
    }
}
