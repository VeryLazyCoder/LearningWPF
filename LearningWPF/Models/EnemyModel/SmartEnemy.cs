using System.Collections.Generic;
using System.IO;

namespace LearningWPF.Models
{
    public class SmartEnemy : IEnemy
    {
        private readonly Dictionary<Point, Point> _track;
        private readonly GameMap _map;
        private readonly Point[] _offsetPoints;
        private readonly Point _nullPoint = new(-1, -1);

        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        private Point StartPointForBfs => Position;

        public SmartEnemy(Point position, GameMap map)
        {
            _map = map;
            Position = position;
            PreviousPosition = position;
            _offsetPoints = new Point[]
            {
                new(-1, 0),
                new(1, 0),
                new(0, -1),
                new(0, 1)
            };
            _track = new Dictionary<Point, Point>();
        }

        public void Move(Point playerPosition)
        {
            FormPathToPlayer(playerPosition);
            (PreviousPosition, Position) = (Position, GetNextPointToPlayer(playerPosition));
        }
        
        public bool CollisionWithPlayer(Point playerPosition) =>
            playerPosition == Position || playerPosition == PreviousPosition;

        public char GetEnemySymbol() => 'S';

        private Point GetNextPointToPlayer(Point playerPosition)
        {
            var pathItem = playerPosition;
            var path = new List<Point>();

            while (pathItem != _nullPoint)
            {
                path.Add(pathItem);
                pathItem = _track[pathItem];
            }
            path.Reverse();
            return path.Count > 1 ? path[1] : Position;
        }

        private void FormPathToPlayer(Point playerPosition)
        {
            _track.Clear();
            _track[StartPointForBfs] = _nullPoint;
            var visited = new HashSet<Point>();
            var pointQueue = new Queue<Point>();
            pointQueue.Enqueue(StartPointForBfs);

            while (pointQueue.Count != 0)
            {
                var point = pointQueue.Dequeue();

                if (visited.Contains(point) || !_map.IsNotWall(point))
                    continue;
                if (point == playerPosition)
                    break;

                foreach (var p in _offsetPoints)
                {
                    var nextPoint = point + p;
                    pointQueue.Enqueue(nextPoint);
                    if (!visited.Contains(nextPoint))
                        _track[nextPoint] = point;
                }
                visited.Add(point);
            }
        }
    }
}

