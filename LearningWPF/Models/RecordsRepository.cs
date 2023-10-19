using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LearningWPF.Models
{
    public static class RecordsRepository
    {
        private static int _maxID;
        private static List<UserData> _userRecords;
        private const string CONNECTION_STRING = 
            @"Server=ByashaLaptop\SQLEXPRESS, 49172;Database=Reckords;User Id=TEST2;Password=qwerty12345;";

        static RecordsRepository()
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();
            using SqlCommand command = new("select max(Id) from Reckord", connection);
            _maxID = (int)(command.ExecuteScalar() ?? 1);
            _userRecords = new List<UserData>();
        }

        public static List<UserData> LoadRecords(int mapID)
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();

            var sqlQuery = @$"SELECT TOP 10 * FROM Reckord where maptype = {mapID} ORDER BY Score ASC";
            using SqlCommand command = new(sqlQuery, connection);
            using var reader = command.ExecuteReader();

            _userRecords = new List<UserData>();
            while (reader.Read())
            {
                var name = (string)reader["Nickname"];
                var score = (int)reader["Score"];
                var date = (DateTime)reader["ScoreDate"];
                _userRecords.Add(new UserData(name, score, date));
            }
            reader.Close();
            return _userRecords;
        }

        public static void UpdateBase(UserData newRow, int mapVariant)
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();

            var insertQuery = $"insert into Reckord values ({++_maxID}, '{newRow.Name}', {newRow.Score}, " +
                                 $"'{newRow.Date}', {mapVariant})";
            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.ExecuteNonQuery();

            _userRecords.Add(newRow);
            _userRecords = _userRecords.OrderBy(x => x.Score).ToList();

            connection.Close();
        }
    }
}
