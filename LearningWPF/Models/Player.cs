using System;
using System.Collections.Generic;
using System.Threading;

namespace LearningWPF.Models
{
    public class Player : Fighter
    {
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        public int MovesAvailable { get; set; }
        public bool IsDead => MovesAvailable <= 0 || Health <= 0;
        public int TreasureCount { get; private set; }

        private readonly List<Fighter> _enemyFighters;
        private readonly Random _random;

        public Player(Point position, int moves) : base("игрок", 150, 2, 25, "Хороший вопрос")
        {
            _random = new Random();
            Position = position;
            PreviousPosition = position;
            MovesAvailable = moves;

            _enemyFighters = new List<Fighter>
            {
                new("Сумасшедший Маньяк", 200f + _random.Next(-20, 21),
                    2 + _random.Next(-1, 2), 75f + _random.Next(-7, 8),
                    "в зависимости от степени чесания головы меняет свои характеристики"),
                new("Сын маминой подруги", 10f, 9.25f, 100f, "обладает сюжетной бронёй"),
                new("Обезьяна", 500f, 2, 25f, " мозгов нет, здоровья много"),
                new("Ноутбук ирбис", 1000f, 2, 3.5f, "легенда, если проиграет попадёт к вам на стол." +
                                                     "Вы точно хотите этого?")
            };
        }

        public void AddAdditionalTreasure() => TreasureCount++;

        public void Move(ConsoleKey pressedKey, GameMap map)
        {
            var offset = GetOffsetPoint(pressedKey);
           
            PreviousPosition = Position;
            if (map.IsNotWall(Position + offset))
                Position += offset;

            MovesAvailable--;
        }

        public string GetPlayerStatistic()
        {
            return $"Ваше здоровье {Health}\nВаш урон составляет {Damage}\n" +
                   $"Ваша броня  {Armor}\nОсталось ходов: {MovesAvailable}\nСчётчик сокровищ: {TreasureCount}";
        }

        public void RaiseStats()
        {
            Health *= 1.1f;
            Damage *= 1.1f;
            Armor += 0.2f;
        }

        private Point GetOffsetPoint(ConsoleKey pressedKey) => pressedKey switch
        {
            ConsoleKey.W => new Point(-1, 0),
            ConsoleKey.A => new Point(0, -1),
            ConsoleKey.S => new Point(1, 0),
            ConsoleKey.D => new Point(0, 1),
            _ => new Point(0, 0)
        };
    }
}
