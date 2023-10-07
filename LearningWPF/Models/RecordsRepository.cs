using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LearningWPF.Models
{
    public static class RecordsRepository
    {
        private static int _maxID;
        private static int _mapID;
        private static List<UserData> _userRecords = new();
        private static string connectionString =
            @"Server=ByashaLaptop\SQLEXPRESS, 49172;Database=Reckords;User Id=TEST2;Password=qwerty12345;";

        public static List<UserData> LoadRecords(int mapID)
        {
            _mapID = mapID;
            using SqlConnection connection = new(connectionString);
            connection.Open();

            var sqlQuery = $"SELECT TOP 10 * FROM Reckord where maptype = {mapID} ORDER BY Score ASC";
            using SqlCommand command = new(sqlQuery, connection);
            using SqlDataReader reader = command.ExecuteReader();

            _userRecords = new List<UserData>();
            while (reader.Read())
            {
                string name = (string)reader["Nickname"];
                int score = (int)reader["Score"];
                DateTime date = (DateTime)reader["ScoreDate"];
                _userRecords.Add(new UserData(name, score, date));
            }
            reader.Close();

            command.CommandText = "select max(Id) from Reckord";
            _maxID = (int)(command.ExecuteScalar() ?? 1);
            return _userRecords;
        }
    }

    public struct UserData
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
}
