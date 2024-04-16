using System;

namespace LearningWPF.Models;

public readonly struct UserData
{
    public int MapVariant { get; init; }
    public string UserName { get; init; }
    public int Score { get; init; }
    public DateTime Date { get; init; }
    public bool IsLevelUp { get; init; }
    public int Level { get; init; }

    public UserData(int score, DateTime date, int mapVariant, bool isLevelUp)
    {
        
        Score = score;
        Date = date;
        MapVariant = mapVariant;
        IsLevelUp = isLevelUp;
    }

    public UserData(string name, int score, DateTime date, int level)
    {
        UserName = name;
        Score = score;
        Date = date;
        Level = level;
    }

    public override string ToString()
    {
        return $"Игрок {UserName} {Level} уровня победил за {Score} ходов. Рекорд был установлен {Date}";
    }
}