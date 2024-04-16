using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LearningWPF.Models.DBData;

namespace LearningWPF.Models
{
    public static class RecordsRepository
    {
        private static int _maxID;
        private static List<UserData> _userRecords;
        private const string CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;Initial Catalog=Records;Integrated Security=True";
        //@"Server=ByashaLaptop\SQLEXPRESS, 49172;Database=Reckords;User Id=TEST2;Password=12345;";
        private static Dictionary<string, Func<int, string>> _filterQueries = 
            new ()
        {

            ["Показывать рекорды только этого аккаунта"] = (mapNumber) =>
                $"select top 10 NickName,score,ScoreDate,AccountLevel from GameAccounts " +
                $"join Record on GameAccounts.AccountID=Record.AccountID" +
                $" where GameAccounts.AccountID = {CurrentUser.Account.ID} and mapNumber = {mapNumber} order by score ASC",

            ["Показывать все ваши рекорды"] = (mapNumber) => 
                $"select top 10 NickName,score,ScoreDate,AccountLevel from GameAccounts" +
                    $" join Record on GameAccounts.AccountID = Record.AccountID " +
                    $"where UserID = {CurrentUser.UserID} and mapNumber = {mapNumber} order by score ASC",
                
            ["Показывать рекорды других пользователей"] = (mapNumber) => 
                $"select top 10 NickName, score, ScoreDate,AccountLevel from GameAccounts " +
                $"join Record on GameAccounts.AccountID = Record.AccountID " +
                $"where mapNumber = {mapNumber} order by score ASC"
        };

        static RecordsRepository()
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();
            using SqlCommand command = new("select max(RecordID) from Record", connection);
            _maxID = (int)(command.ExecuteScalar() ?? 1);
            _userRecords = new List<UserData>();
        }

        public static List<UserData> LoadRecords(int mapID, string userFilter)
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();

            var sqlQuery = _filterQueries[userFilter].Invoke(mapID);
            using SqlCommand command = new(sqlQuery, connection);
            using var reader = command.ExecuteReader();

            _userRecords = new List<UserData>();
            while (reader.Read())
            {
                var name = (string)reader["NickName"];
                var score = (int)reader["score"];
                var date = (DateTime)reader["ScoreDate"];
                var level = (int)reader["AccountLevel"];
                _userRecords.Add(new UserData(name, score, date, level));
            }
            reader.Close();
            return _userRecords;

        }

        public static void UpdateBase(UserData newRow)
        {
            using SqlConnection connection = new(CONNECTION_STRING);
            connection.Open();

            var insertQuery = $"insert into Record values ({++_maxID}, {CurrentUser.Account.ID}, {newRow.Score}," +
                              $" {newRow.MapVariant}, " +
                                 $"'{newRow.Date}')";
            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.ExecuteNonQuery();

            if (newRow.IsLevelUp)
            {
                insertCommand.CommandText = $"update GameAccounts set AccountLevel = {CurrentUser.Account.Level + 1} " +
                                            $"where AccountID = {CurrentUser.Account.ID}";
                insertCommand.ExecuteNonQuery();
            }
            _userRecords.Add(newRow);
            _userRecords = _userRecords.OrderBy(x => x.Score).ToList();

            connection.Close();
        }


    }
}
