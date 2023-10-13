using System;

namespace LearningWPF.Models
{
    public class CommonEnemy : IEnemy
    {
        private readonly GameMap _map;
        private readonly Random _random;
        private readonly Point[] _offsetPoints;

        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }

        public CommonEnemy(Point position, GameMap map)
        {
            _map = map;
            Position = position;
            PreviousPosition = position;
            _random = new Random();
            _offsetPoints = new Point[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
        }

        public void Move(Point playerPosition)
        {
            while (true)
            {
                var nextPosition = Position + _offsetPoints[_random.Next(_offsetPoints.Length)];

                if (_map.IsNotWall(nextPosition))
                {
                    (Position, PreviousPosition) = (nextPosition, Position);
                    break;
                }
            }
        }

        public bool CollisionWithPlayer(Point playerPosition) =>
            playerPosition == Position || playerPosition == PreviousPosition;

        public char GetEnemySymbol() => 'C';
    }
}

