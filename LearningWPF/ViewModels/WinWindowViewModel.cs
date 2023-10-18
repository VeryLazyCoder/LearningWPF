using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LearningWPF.ViewModels.ViewModelBase;

namespace LearningWPF.ViewModels
{
    internal class WinWindowViewModel : ViewModel
    {
        public ICommand UpdateRecordDbCommand { get; private set; }
        
        public WinWindowViewModel() { }

        public WinWindowViewModel(int mapVariant)
        {

        }
    }
}
