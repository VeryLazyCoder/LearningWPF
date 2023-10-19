using GalaSoft.MvvmLight.Command;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using LearningWPF.Infrastructure;
using Point = LearningWPF.Models.Point;
using LearningWPF.Views;

namespace LearningWPF.ViewModels
{
    public class GameWindowViewModel : ViewModel
    {
        public static ObservableCollection<BitmapImage> ImageList { get; private set; } = new();

        private static char[,] _map;
        private static GameRound _round;
        private static RandomEventsHandler _eventsHandler;

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
            _map = map.Map;
            ColumnsCount = _map.GetLength(1);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
            _round = new GameRound(map, numberOfEnemies, OnPositionChanged);
            _round.GameWin += () => SwitchToWinWindow(mapVariant, _round.UserScore);
            _round.GameLoose += () =>
            {
                MessageBox.Show("Loose!!!");
                new SwitchToMenuWindowCommand().Execute(this);
            };
            _round.StatsChanged += text => PlayerStats = text;
            _eventsHandler = new RandomEventsHandler(_round);
        }

        public GameWindowViewModel(){}

        private static void OnKeyDown(string key)
        {
            _eventsHandler.TryRaiseEvent();
            _round.GetNextTurn(GetKey(key));
        }

        private static void FillImageList()
        {
            for (var row = 0; row < _map.GetLength(0); row++)
                for (var col = 0; col < _map.GetLength(1); col++)
                    ImageList.Add(GetBitmap(_map[row, col]));
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
            var index = previous.X * _map.GetLength(1) + previous.Y;
            ImageList[index] = GetBitmap(_map[previous.X, previous.Y]);

            index = current.X * _map.GetLength(1) + current.Y;
            ImageList[index] = GetBitmap(gameObject);
        }

        private static void SwitchToWinWindow(int mapVariant, int score)
        {
            var viewModel = new WinWindowViewModel(mapVariant, score);
            var secondWindow = new WinWindow()
            {
                DataContext = viewModel
            };
            foreach (Window window in Application.Current.Windows)
                if (window.DataContext != viewModel)
                    window.Close();
            secondWindow.Show();
        }
    }
}

