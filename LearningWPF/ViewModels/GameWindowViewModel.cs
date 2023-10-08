using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LearningWPF.ViewModels.ViewModelBase;
namespace LearningWPF.ViewModels
{
    internal class GameWindowViewModel : ViewModel
    {
        private Color[,] _colorArray;

        public GameWindowViewModel()
        {
            // Инициализируйте массив цветов здесь, например, случайными цветами
            _colorArray = new Color[3, 3]
            {
                { Colors.Red, Colors.Green, Colors.Blue },
                { Colors.Yellow, Colors.Orange, Colors.Purple },
                { Colors.Gray, Colors.Brown, Colors.Pink }
            };

            ColorList = new ObservableCollection<Color>();
            for (int row = 0; row < _colorArray.GetLength(0); row++)
            {
                for (int col = 0; col < _colorArray.GetLength(1); col++)
                {
                    ColorList.Add(_colorArray[row, col]);
                }
            }
        }

        public ObservableCollection<Color> ColorList { get; }
    }
}

