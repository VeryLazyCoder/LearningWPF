using System;
using System.Windows;
using System.Windows.Input;

namespace LearningWPF.Infrastructure
{
    internal class ShowMessageBoxCommand : ICommand
    {
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? stringToShow)
        {
            if(stringToShow is string text)
                MessageBox.Show(text);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
