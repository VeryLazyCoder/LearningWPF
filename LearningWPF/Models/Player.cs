using System;
using System.Collections.Generic;
using System.Text;
using LearningWPF.Infrastructure;

namespace LearningWPF.Models
{
    public class Player : Fighter
    {
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        public int MovesAvailable { get; set; }
        public int TreasureCount { get; private set; }
        public bool IsDead => MovesAvailable <= 0 || Health <= 0;

        private readonly Action<Point, Point, char> _positionChanged;
        private readonly List<Fighter> _enemyFighters;
        private readonly Random _random;

        public Player(Point position, int moves, Action<Point, Point, char> positionChanged) : 
            base(150, 2, 25)
        {
            _positionChanged = positionChanged;
            _random = new Random();
            Position = position;
            PreviousPosition = position;
            MovesAvailable = moves;
            _positionChanged(PreviousPosition, Position, 'P');

            _enemyFighters = new List<Fighter>
            {
                new( 200f + _random.Next(-20, 21),
                    2 + _random.Next(-1, 2), 75f + _random.Next(-7, 8)),
                new(10f, 9.25f, 100f),
                new(500f, 2, 25f),
                new( 1000f, 7, 3.5f)
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
            _positionChanged(PreviousPosition, Position, 'P');
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

        public override string ToString()
        {
            return $"Здоровье {Health:F0}\t\tБроня {Armor:F2}\t\tУрон {Damage:F0} " +
                   $"\nОсталось ходов {MovesAvailable}\t\t\t\tСобрано сокровищ {TreasureCount}";
        }

        public void FightWithEnemy()
        {
            var result = new StringBuilder();
            var enemyFighter = GetRandomEnemy();
            result.AppendLine(enemyFighter.GetFighterStats());
            LaunchFight(enemyFighter);
            result.AppendLine(GetFightResult());
            new ShowMessageBoxCommand().Execute(result.ToString());
            _positionChanged(PreviousPosition, Position, 'P');
        }


        private Fighter GetRandomEnemy()
        {
            if (_enemyFighters.Count <= 1)
                AddSecretEnemies();

            return _enemyFighters[_random.Next(_enemyFighters.Count)];
        }

        private string GetFightResult() =>
            IsDead ? "Вы проиграли" : "Вы победили этого противника, пока что...";

        private void LaunchFight(Fighter enemyFighter)
        {
            LaunchBattleCycle(enemyFighter);

            if (Health > 0)
                _enemyFighters.Remove(enemyFighter);
        }

        private void LaunchBattleCycle(Fighter enemyFighter)
        {
            while (Health > 0 && enemyFighter.Health > 0)
            {
                enemyFighter.TakeDamage(Damage);
                TakeDamage(enemyFighter.Damage);
            }
        }

        private void AddSecretEnemies()
        {
            var random = new Random();
            _enemyFighters.Add(new Fighter( 4294967F, 0, 4294967F));
            _enemyFighters.Add(new Fighter(random.Next(0, 4294967),
                random.Next(0, 11),
                random.Next(0, 4294967)));
        }
    }
}
