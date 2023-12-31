﻿using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
using System.Windows;
using System.Windows.Input;

namespace LearningWPF.ViewModels
{
    internal class ConfigurationWindowViewModel : ViewModel
    {
        public ObservableCollection<int> Maps { get; private set; } = new(Enumerable.Range(1, 3));
        public ObservableCollection<int> Enemies { get; private set; } = new(Enumerable.Range(0,10));
        
        private int _mapVariant = 1;
        private int _numberOfEnemies = 2;

        public int MapVariant
        {
            get => _mapVariant;
            set => Set(ref _mapVariant, value);
        }

        public int NumberOfEnemies
        {
            get => _numberOfEnemies;
            set => Set(ref _numberOfEnemies, value);
        }

        public ICommand SwitchToGameWindowCommand { get; }

        public ConfigurationWindowViewModel()
        {
            SwitchToGameWindowCommand = new RelayCommand(SwitchToGameWindow);
        }

        private void SwitchToGameWindow()
        {
            var viewModel = new GameWindowViewModel(_mapVariant, _numberOfEnemies);
            var secondWindow = new GameWindow(viewModel);
            secondWindow.Show();
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
        }
    }
}
