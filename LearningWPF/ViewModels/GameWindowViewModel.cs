using GalaSoft.MvvmLight.Command;
using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using LearningWPF.Models;
using Point = LearningWPF.Models.Point;

namespace LearningWPF.ViewModels
{
    internal class GameWindowViewModel : ViewModel
    {
        public static ObservableCollection<BitmapImage> ImageList { get; private set; } = new();

        private static char[,] Map;
        private int _columnsCount;
        private static GameRound Round;

        public int ColumnsCount
        {
            get => _columnsCount;
            set => Set(ref _columnsCount, value);
        }

        public ICommand KeyCommand { get; private set; } = new RelayCommand<string>(OnKeyDown);

        public GameWindowViewModel(int mapVariant)
        {
            Round = new GameRound(mapVariant, 2);
            Round.GameWin += () => MessageBox.Show("Victory!");
            Round.GameLoose += () => MessageBox.Show("Loose!!!");
            Round.PositionChanged += (previous, current, symbol) =>
            {
                var index = previous.X * Map.GetLength(1) + previous.Y;
                ImageList[index] = GetBitmap(Map[previous.X, previous.Y]);

                index = current.X * Map.GetLength(1) + current.Y;
                ImageList[index] = GetBitmap(symbol);
            };
            Map = Round.Map.Map;
            ColumnsCount = Map.GetLength(1);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
        }

        private static void OnKeyDown(string key)
        {
            Round.GetNextTurn(GetKey(key));

            

            //ImageList.Clear();
            //FillImageList();
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
    }
}

