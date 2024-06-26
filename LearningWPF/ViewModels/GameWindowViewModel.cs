﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using LearningWPF.Infrastructure;
using LearningWPF.Models;
using LearningWPF.ViewModels.ViewModelBase;
using LearningWPF.Views;
using Point = LearningWPF.Models.Point;

namespace LearningWPF.ViewModels;

public class GameWindowViewModel : ViewModel
{
    public ObservableCollection<BitmapImage> ImageList { get; } = new();

    private readonly char[,] _map;
    private readonly GameRound _round;
    private readonly RandomEventsHandler _eventsHandler;

    private int _columnsCount;
    private string _playerStats;
    private readonly int _startMoves;

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

    public ICommand KeyCommand { get; }

    public GameWindowViewModel(int mapVariant, int numberOfEnemies)
    {
        KeyCommand = new RelayCommand<string>(OnKeyDown);
        var map = GameMap.CreateMap(mapVariant);
        _map = map.Map;
        ColumnsCount = _map.GetLength(1);
        ImageList = new ObservableCollection<BitmapImage>();
        FillImageList();
        _round = new GameRound(map, numberOfEnemies, OnPositionChanged);
        _round.GameWin += () => SwitchToWinWindow(mapVariant, _round.UserScore);
        _round.GameLoose += () =>
        {
            new ShowMessageBoxCommand().Execute("Вы проиграли, какая жалость");
            new SwitchToMenuWindowCommand().Execute(this);
        };
        _round.StatsChanged += text => PlayerStats = text;
        _eventsHandler = new RandomEventsHandler(_round);
        _playerStats = _round.Player.ToString();
        _startMoves = map.MovesAvailable;
    }

    public GameWindowViewModel()
    {
    }

    private void OnKeyDown(string key)
    {
        _eventsHandler.TryRaiseEvent();
        _round.GetNextTurn(GetKey(key));
    }

    private void FillImageList()
    {
        for (var row = 0; row < _map.GetLength(0); row++)
        for (var col = 0; col < _map.GetLength(1); col++)
            ImageList.Add(GetBitmap(_map[row, col]));
    }

    private static BitmapImage GetBitmap(char symbol) => symbol switch
    {
        'P' => new BitmapImage(new Uri("/Images/peasant.bmp", UriKind.Relative)),
        '@' => new BitmapImage(new Uri("/Images/dog.bmp", UriKind.Relative)),
        'X' => new BitmapImage(new Uri("/Images/chest.bmp", UriKind.Relative)),
        'O' => new BitmapImage(new Uri("/Images/opened.bmp", UriKind.Relative)),
        'C' => new BitmapImage(new Uri("/Images/enemy.bmp", UriKind.Relative)),
        'S' => new BitmapImage(new Uri("/Images/smart.bmp", UriKind.Relative)),
        'A' => new BitmapImage(new Uri("/Images/armor.bmp", UriKind.Relative)),
        'D' => new BitmapImage(new Uri("/Images/damage.bmp", UriKind.Relative)),
        'H' => new BitmapImage(new Uri("/Images/health.bmp", UriKind.Relative)),
        'W' or '|' or '-' => new BitmapImage(new Uri("/Images/wall.bmp", UriKind.Relative)),
        _ => new BitmapImage(new Uri("/Images/grass.bmp", UriKind.Relative))
    };

    private ConsoleKey GetKey(string key) => key switch
    {
        "W" => ConsoleKey.W,
        "A" => ConsoleKey.A,
        "S" => ConsoleKey.S,
        "D" => ConsoleKey.D,
        _ => ConsoleKey.Spacebar
    };

    private void OnPositionChanged(Point previous, Point current, char gameObject)
    {
        var index = previous.X * _map.GetLength(1) + previous.Y;
        ImageList[index] = GetBitmap(_map[previous.X, previous.Y]);

        index = current.X * _map.GetLength(1) + current.Y;
        ImageList[index] = GetBitmap(gameObject);
    }

    private void SwitchToWinWindow(int mapVariant, int score)
    {
        var viewModel = new WinWindowViewModel(mapVariant, score, _startMoves);
        var secondWindow = new WinWindow
        {
            DataContext = viewModel
        };
        foreach (Window window in Application.Current.Windows)
            if (window.DataContext != viewModel)
                window.Close();
        secondWindow.Show();
    }
}