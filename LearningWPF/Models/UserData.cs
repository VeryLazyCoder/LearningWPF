using System;

namespace LearningWPF.Models;

public readonly struct UserData
{
    public string Name { get; init; }
    public int Score { get; init; }
    public DateTime Date { get; init; }

    public UserData(string name, int score, DateTime date)
    {
        Name = name;
        Score = score;
        Date = date;
    }

    public override string ToString()
    {
        return $"Игрок {Name} победил за {Score} ходов. Рекорд был установлен {Date}";
    }
}