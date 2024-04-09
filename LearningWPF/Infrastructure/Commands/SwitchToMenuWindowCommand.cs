using LearningWPF.ViewModels;
using LearningWPF.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearningWPF.Infrastructure
{
    internal class SwitchToMenuWindowCommand : ICommand
    {
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var secondWindow = new MenuWindow();
            var viewModel = new MenuWindowViewModel();
            secondWindow.DataContext = viewModel;
            secondWindow.Show();
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
