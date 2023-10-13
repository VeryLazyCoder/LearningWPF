using System;
using System.Collections.Generic;

namespace LearningWPF.Models
{
    public class GameRound
    {
        public Player Player { get; }
        public GameMap Map { get; }
        public bool? IsWon { get; private set; }
        public int UserScore { get; private set; }
        public event Action GameLoose;
        public event Action GameWin;
        private Action<Point, Point, char>? _positionChanged;

        private readonly int _startMoves;
        private readonly int _initialEnemiesCount;

        private readonly Dictionary<char, Action<Player, GameMap>> _actionsOnCollision = new()
        {
            ['X'] = (player, map) =>
            {
                player.AddAdditionalTreasure();
                map[player.Position] = 'O';
                player.RaiseStats();
            },
            ['A'] = (player, map) =>
            {
                player.ChangeArmorFor(3);
                map[player.Position] = ' ';
            },
            ['D'] = (player, map) =>
            {
                player.ChangeDamageFor(player.Damage / 3);
                map[player.Position] = ' ';
            },
            ['H'] = (player, map) =>
            {
                player.ChangeHealthFor(player.Health / 3);
                map[player.Position] = ' ';
            },
            ['@'] = (player, map) => player.MovesAvailable -= 10,
            [' '] = (player, map) => { },
            ['O'] = (player, map) => { },
        };
        private List<IEnemy> _enemies;
        //private RandomEventsHandler _eventHandler;

        public GameRound(GameMap map, int enemyCount, Action<Point, Point, char>? positionChanged)
        {
            _positionChanged = positionChanged;
            Map = map;
            _startMoves = Map.MovesAvailable;
            _initialEnemiesCount = enemyCount;
            _enemies = GetEnemies(enemyCount);
            Player = new Player(Map.GetRandomEmptyPosition(), _startMoves, positionChanged);
            //_eventHandler = new();
        }

        public void GetNextTurn(ConsoleKey pressedKey)
        {
            if (IsWon == true)
                GameWin?.Invoke();

            if (IsWon == true)
                GameLoose?.Invoke();

            ChangeGameState(pressedKey);
            SetRoundResultIfGameIsOver();
        }

        public void AddAdditionalEnemy() =>
            _enemies.Add(new CommonEnemy(Map.GetRandomEmptyPosition(), Map, _positionChanged));

        private void SetRoundResultIfGameIsOver()
        {
            if (IsVictoryAchieved())
                (IsWon, UserScore) = (true, _startMoves - Player.MovesAvailable);

            if (Player.IsDead)
                (IsWon, UserScore) = (false, -1);
        }

        private bool IsVictoryAchieved()
        {
            return Player.TreasureCount == Map.TreasuresOnTheMap ||
                (_enemies.Count == 0 && _initialEnemiesCount != 0);
        }

        private void ChangeGameState(ConsoleKey pressedKey)
        {
            MovePlayer(pressedKey);
            MoveEnemies();
            //_eventHandler.TryRaiseEvent(this);
            _actionsOnCollision[Map[Player.Position]].Invoke(Player, Map);
        }

        private void MovePlayer(ConsoleKey pressedKey)
        {
            Player.Move(pressedKey, Map);
            //PositionChanged(Player.PreviousPosition, Player.Position, 'P');
        }

        private void MoveEnemies()
        {
            for (var i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Move(Player.Position);
                if (_enemies[i].CollisionWithPlayer(Player.Position))
                    GameLoose?.Invoke();

                //{
                //    Player.FightWithEnemy();
                //    _enemies.RemoveAt(i);
                //    Map.DrawMap();
                //    break;
                //}
            }
        }

        private List<IEnemy> GetEnemies(int enemyCount)
        {
            var enemies = new List<IEnemy>();

            for (var i = 0; i < enemyCount; i++)
            {
                switch (i)
                {
                    case 2:
                        enemies.Add(GetEnemy(new Point(Map.Map.GetLength(0) - 2, 
                            Map.Map.GetLength(1) - 2)));
                        break;
                    case 3:
                        enemies.Add(GetEnemy(new Point(1, 1)));
                        break;
                    default:
                        enemies.Add(GetEnemy(Map.GetRandomEmptyPosition()));
                        break;
                }
            }
            return enemies;
        }
        private IEnemy GetEnemy(Point point) =>
            new Random().Next(3) == 0 ? new SmartEnemy(point, Map, _positionChanged) : 
                new CommonEnemy(point, Map, _positionChanged);

    }
}
