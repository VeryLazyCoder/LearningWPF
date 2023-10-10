﻿using LearningWPF.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;

namespace LearningWPF.ViewModels
{
    internal class GameWindowViewModel : ViewModel
    {
        public static ObservableCollection<BitmapImage> ImageList { get; private set; }

        private static readonly char[,] _map =
        {
            {'W','W','W','W','W',},
            {'W',' ','X',' ','W',},
            {'W','P',' ',' ','W',},
            {'W',' ',' ',' ','W',},
            {'W',' ',' ',' ','W',},
            {'W','W','W','W','W',},
        };
        private int _columnsCount;
        private Point _playersCoordinates = new (2, 1);

        public int ColumnsCount
        {
            get => _columnsCount;
            set => Set(ref _columnsCount, value);
        }

        public ICommand KeyCommand { get; private set; } = new RelayCommand<string>(OnKeyDown);

        private static void OnKeyDown(string key)
        {
            if (key == "Space")
            {
                _map[2, 1] = ' ';
                ImageList.Clear();
                FillImageList();
                
            }
        }

        public GameWindowViewModel()
        {
            ColumnsCount = _map.GetLength(1);
            ImageList = new ObservableCollection<BitmapImage>();
            FillImageList();
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
            'X' => new BitmapImage(new Uri("/Images/chest.bmp", UriKind.Relative)),
            'W' => new BitmapImage(new Uri("/Images/wall.bmp", UriKind.Relative)),
            _ => new BitmapImage(new Uri("/Images/Grass.bmp", UriKind.Relative)),
        };
    }
}

