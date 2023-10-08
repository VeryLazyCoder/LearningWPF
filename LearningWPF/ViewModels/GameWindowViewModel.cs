using System;
using LearningWPF.ViewModels.ViewModelBase;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LearningWPF.ViewModels
{
    internal class GameWindowViewModel : ViewModel
    {
        private readonly Color[,] _colorArray;
        private readonly BitmapImage[,] _imageSource;

        public GameWindowViewModel()
        {
            _colorArray = new[,]
            {
                { Colors.Red, Colors.Green, Colors.Blue },
                { Colors.Yellow, Colors.Orange, Colors.Purple },
                { Colors.Gray, Colors.Brown, Colors.Pink }
            };

            _imageSource = new[,]
            {
                { new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),},
                { new BitmapImage(new Uri("/Images/cool.bmp", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),},
                { new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),new BitmapImage(new Uri("/Images/фон2.bmp", UriKind.RelativeOrAbsolute)),},
            };

            ColorList = new ObservableCollection<Color>();
            ImageList = new ObservableCollection<BitmapImage>();
            for (var row = 0; row < _colorArray.GetLength(0); row++)
            {
                for (var col = 0; col < _colorArray.GetLength(1); col++)
                {
                    ColorList.Add(_colorArray[row, col]);
                    ImageList.Add(_imageSource[row, col]);
                }
            }
        }

        public ObservableCollection<Color> ColorList { get; }
        public ObservableCollection<BitmapImage> ImageList { get; }
    }
}

