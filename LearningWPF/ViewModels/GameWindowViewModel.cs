using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using LearningWPF.Models;

namespace LearningWPF.ViewModels
{
    internal class GameWindowViewModel : ViewModel
    {
        public static ObservableCollection<BitmapImage> ImageList { get; private set; } = new();

        private static char[,] Map;
        private int _columnsCount;
        private static Point _playersCoordinates;

        public int ColumnsCount
        {
            get => _columnsCount;
            set => Set(ref _columnsCount, value);
        }

        public ICommand KeyCommand { get; private set; } = new RelayCommand<string>(OnKeyDown);

        public GameWindowViewModel(int mapVariant)
        {
            Map = GameMap.CreateMap(mapVariant).Map;
            ColumnsCount = Map.GetLength(1);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
            FindPlayerPosition();
        }

        private static void OnKeyDown(string key)
        {
            var offset = GetOffsetPoint(key);
            var nextPosition = _playersCoordinates + offset;

            Map[_playersCoordinates.X, _playersCoordinates.Y] = ' ';
            Map[nextPosition.X, nextPosition.Y] = 'P';

            //var index = _playersCoordinates.X * Map.GetLength(1) + _playersCoordinates.Y;
            //ImageList[index] = new BitmapImage(new Uri("/Images/Grass.bmp", UriKind.Relative));
            _playersCoordinates = nextPosition;
            //index = _playersCoordinates.X * Map.GetLength(1) + _playersCoordinates.Y;
            //ImageList[index] = new BitmapImage(new Uri("/Images/Peasant.bmp", UriKind.Relative));

            ImageList.Clear();
            FillImageList();
        }

        private static void FillImageList()
        {
            for (var row = 0; row < Map.GetLength(0); row++)
                for (var col = 0; col < Map.GetLength(1); col++)
                    ImageList.Add(GetBitmap(Map[row, col]));
        }

        private static BitmapImage GetBitmap(char symbol) => symbol switch
        {
            'P' => new BitmapImage(new Uri("/Images/Peasant.bmp", UriKind.Relative)),
            'X' => new BitmapImage(new Uri("/Images/chest.bmp", UriKind.Relative)),
            'W' or '|' or '-' => new BitmapImage(new Uri("/Images/wall.bmp", UriKind.Relative)),
            _ => new BitmapImage(new Uri("/Images/Grass.bmp", UriKind.Relative)),
        };

        private static Point GetOffsetPoint(string pressedKeyLiteral) => pressedKeyLiteral switch
        {
            "W" => new Point(-1, 0),
            "S" => new Point(1, 0),
            "A" => new Point(0, -1),
            "D" => new Point(0, 1),
            _ => new Point(0, 0),
        };

        private static void FindPlayerPosition()
        {
            for (var x = 0; x < Map.GetLength(0); x++)
                for (var y = 0; y < Map.GetLength(1); y++)
                    if (Map[x, y] == 'P')
                        _playersCoordinates = new Point(x, y);
        }
    }
}

