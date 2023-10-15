using System;

namespace LearningWPF.Models
{
    public class CommonEnemy : IEnemy
    {
        public Action<Point, Point, char> PositionChanged;

        private readonly GameMap _map;
        private readonly Random _random;
        private readonly Point[] _offsetPoints;

        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }

        public CommonEnemy(Point position, GameMap map, Action<Point, Point, char> positionChanged)
        {
            PositionChanged = positionChanged;
            _map = map;
            Position = position;
            PreviousPosition = position;
            PositionChanged += positionChanged;
            _random = new Random();
            _offsetPoints = new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
            PositionChanged?.Invoke(PreviousPosition, Position, 'C');
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

            PositionChanged?.Invoke(PreviousPosition, Position, 'C');
        }

        public bool CollisionWithPlayer(Point playerPosition) =>
            playerPosition == Position || playerPosition == PreviousPosition;
    }
}

