using System;
using System.IO;

namespace LearningWPF.Models
{
    public class GameMap
    {
        public int MovesAvailable { get; }
        public int TreasuresOnTheMap { get; private set; }  

        public readonly char[,] Map;

        private readonly Random _random;

        public static GameMap CreateMap(int mapVariant)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pathToMap = mapVariant switch
            {
                1 => (Path.Combine(basePath, "Maps", "small map.txt"), 120),
                2 => (Path.Combine(basePath, "Maps", "middle map.txt"), 160),
                _ => (Path.Combine(basePath, "Maps", "large map.txt"), 300)
            };

            return new GameMap(pathToMap.Item1, pathToMap.Item2);
        }

        private GameMap(string pathToMap, int movesAvailable)
        {
            _random = new Random();
            MovesAvailable = movesAvailable;
            var file = File.ReadAllLines(pathToMap);
            var map = new char[file.Length, file[0].Length];
            for (var x = 0; x < map.GetLength(0); x++)
                for (var y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[x][y];
            Map = map;

            DrawSymbolOnEmptyCell('A');
            DrawSymbolOnEmptyCell('D');
            DrawSymbolOnEmptyCell('H');
            CountTreasures();
        }

        public char this[Point point]
        {
            get => Map[point.X, point.Y];
            set => Map[point.X, point.Y] = value;
        }

        public Point AddAdditionalTreasure()
        {
            var point = GetRandomEmptyPosition();
            this[point] = 'X';
            TreasuresOnTheMap++;
            return point;
        }

        public bool IsNotWall(Point position) => this[position] != '|' && this[position] != '-';

        public bool IsEmptyCell(Point position) => this[position] == ' ';

        public Point GetRandomEmptyPosition()
        {
            while (true)
            {
                var position = new Point(_random.Next(Map.GetLength(0)),
                    _random.Next(Map.GetLength(1)));

                if (IsEmptyCell(position))
                    return position;
            }
        }

        private void DrawSymbolOnEmptyCell(char symbol) => this[GetRandomEmptyPosition()] = symbol;
        
        private void CountTreasures()
        {
            for (var i = 0; i < Map.GetLength(0); i++)
                for (var j = 0; j < Map.GetLength(1); j++)
                    if (Map[i, j] == 'X')
                        TreasuresOnTheMap += 1;
        }
    }
}
