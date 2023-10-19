using System;
using System.Collections.Generic;
using LearningWPF.Infrastructure;

namespace LearningWPF.Models
{
    public class RandomEventsHandler
    {
        private readonly List<Event> _events = new()
        {
            new Event("К вам пришла налоговая", "пришлось потратить время на их уплату, на это ушло 5 ходов"),
            new Event("Вы нашли ковёр самолёт", " у вас стало больше времени. " +
                                                "Получите 5 ходов"),
            new Event("К вам в руки попала дополненная карта лабиринта", "на ней обнаружилось неизвестное до этого сокровище"),
            new Event("Вы протёрли свои очки и увидели нового врага", "придётся перемещаться осторожнее :)"),
            new Event("Вы нашли подозрительную синюю таблетку с лекарством",
                "здоровье увеличилось"),
            new Event("Вы нашли подозрительную красную таблетку",
                "здоровье уменьшилось")
        };
        private readonly Random _random;
        private readonly List<Action<GameRound>> _actions = new()
        {
            game => game.Player.MovesAvailable -= 5,
            game => game.Player.MovesAvailable += 5,
            game =>
            {
                var point = game.Map.AddAdditionalTreasure();
                game.PositionChanged(point, point, 'X');
            },
            game => game.AddAdditionalEnemy(),
            game => game.Player.ChangeHealthFor(25),
            game => game.Player.ChangeHealthFor(-25),
        };
        private readonly GameRound _round;

        public RandomEventsHandler(GameRound round)
        {
            _random = new Random();
            _round = round;
        }
        
        public bool TryRaiseEvent()
        {
            if (_random.Next(35) != 22) return false;
            InvokeEvent();
            return true;
        }

        public void InvokeEvent()
        {
            var eventNumber = _random.Next(_events.Count);
            _actions[eventNumber].Invoke(_round);
            new ShowMessageBoxCommand().Execute(_events[eventNumber].ToString());
        }
    }

    internal readonly struct Event
    {
        public string Description { get; init; }
        public string Consequence { get; init; }

        public Event(string description, string consequence)
        {
            Description = description;
            Consequence = consequence;
        }

        public override string ToString()
        {
            return $"{Description}. Как итог, {Consequence}";
        }
    }
}
