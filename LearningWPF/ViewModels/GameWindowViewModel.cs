using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = LearningWPF.Models.Point;

namespace LearningWPF.ViewModels
{
    public class GameWindowViewModel : ViewModel
    {
        public static ObservableCollection<BitmapImage> ImageList { get; private set; } = new();

        private static char[,] Map;
        private static GameRound Round;
        private static RandomEventsHandler EventsHandler;

        private int _columnsCount;
        private string _playerStats;

        public int ColumnsCount
        {
            get => _columnsCount;
            set => Set(ref _columnsCount, value);
        }

        public string PlayerStats
        {
            get => _playerStats;
            set => Set(ref _playerStats, value);
        }

        public ICommand KeyCommand { get; private set; } = new RelayCommand<string>(OnKeyDown);

        public GameWindowViewModel(int mapVariant, int numberOfEnemies)
        {
            var map = GameMap.CreateMap(mapVariant);
            Map = map.Map;
            ColumnsCount = Map.GetLength(1);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
            Round = new GameRound(map, numberOfEnemies, OnPositionChanged);
            Round.GameWin += () => MessageBox.Show("Victory!");
            Round.GameLoose += () => MessageBox.Show("Loose!!!");
            Round.StatsChanged += text => PlayerStats = text;
            EventsHandler = new RandomEventsHandler((text) => MessageBox.Show(text), Round);
        }

        public GameWindowViewModel(){}

        private static void OnKeyDown(string key)
        {
            EventsHandler.TryRaiseEvent();
            Round.GetNextTurn(GetKey(key));
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
            '@' => new BitmapImage(new Uri("/Images/dog.bmp", UriKind.Relative)),
            'X' => new BitmapImage(new Uri("/Images/chest.bmp", UriKind.Relative)),
            'O' => new BitmapImage(new Uri("/Images/opened.bmp", UriKind.Relative)),
            'C' => new BitmapImage(new Uri("/Images/enemy.bmp", UriKind.Relative)),
            'S' => new BitmapImage(new Uri("/Images/smart.bmp", UriKind.Relative)),
            'A' => new BitmapImage(new Uri("/Images/armor.bmp", UriKind.Relative)),
            'D' => new BitmapImage(new Uri("/Images/damage.bmp", UriKind.Relative)),
            'H' => new BitmapImage(new Uri("/Images/health.bmp", UriKind.Relative)),
            'W' or '|' or '-' => new BitmapImage(new Uri("/Images/wall.bmp", UriKind.Relative)),
            _ => new BitmapImage(new Uri("/Images/Grass.bmp", UriKind.Relative)),
        };

        private static ConsoleKey GetKey(string key) => key switch
        {
            "W" => ConsoleKey.W,
            "A" => ConsoleKey.A,
            "S" => ConsoleKey.S,
            "D" => ConsoleKey.D,
            _ => ConsoleKey.Spacebar
        };

        private static void OnPositionChanged(Point previous, Point current, char gameObject)
        {
            var index = previous.X * Map.GetLength(1) + previous.Y;
            ImageList[index] = GetBitmap(Map[previous.X, previous.Y]);

            index = current.X * Map.GetLength(1) + current.Y;
            ImageList[index] = GetBitmap(gameObject);
        }
    }
}

