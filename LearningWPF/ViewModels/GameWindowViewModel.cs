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
        private static Point _playersCoordinates = new (2, 1);

        public int ColumnsCount
        {
            get => _columnsCount;
            set => Set(ref _columnsCount, value);
        }

        public ICommand KeyCommand { get; private set; } = new RelayCommand<string>(OnKeyDown);

        public GameWindowViewModel()
        {
            Map = GameMap.CreateMap(1).Map;
            ColumnsCount = Map.GetLength(0);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
        }

        private static void OnKeyDown(string key)
        {
            var offset = GetOffsetPoint(key);
            var nextPosition = _playersCoordinates + offset;

            Map[_playersCoordinates.Y, _playersCoordinates.X] = ' ';
            Map[nextPosition.Y, nextPosition.X] = 'P';


            _playersCoordinates = nextPosition;

            ImageList.Clear();
            FillImageList();
        }

        private static void FillImageList()
        {
            for (var row = 0; row < Map.GetLength(1); row++)
                for (var col = 0; col < Map.GetLength(0); col++)
                    ImageList.Add(GetBitmap(Map[col, row]));
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
    }
}

