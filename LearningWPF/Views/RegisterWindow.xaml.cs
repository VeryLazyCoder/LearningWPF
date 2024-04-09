using LearningWPF.Infrastructure;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LearningWPF.ViewModels;

namespace LearningWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as RegisterWindowViewModel;
            var textBoxName = ((TextBox)sender).Name;
            var keyEventArgsInfo = new KeyEventArgsInfo (e.Key, textBoxName);
            viewModel.MoveFocusCommand.Execute(keyEventArgsInfo);
        }
    }
}
