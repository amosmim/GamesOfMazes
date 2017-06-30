using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace MazeWebAPI.Models
{
    public class UsersModel
    {
        /// <summary>
        /// Preforms a check to see if user's exists inside the DB.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>true if exists otherwise false</returns>
        public bool Login(string username, string password)
        {
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionString"]))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", this.Hash(password));
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    // user's exists
                    if (reader.Read())
                    {
                        return true;
                    }

                    // user's not found
                    return false;
                } catch (SqlException)
                {
                    // error
                    return false;
                }
            }
        }

        /// <summary>
        /// SHA1 function.
        /// </summary>
        /// <param name="input">data</param>
        /// <returns>hashed string</returns>
        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                // hash the bytes
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Register a new user to the DB.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="email">email</param>
        /// <returns>1 if success, -1 or -2 in cases of error</returns>
        public int Register(string username, string password, string email)
        {
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionString"]))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id FROM Users WHERE Username = @username";
                command.Parameters.AddWithValue("@username", username);
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                try
                {
                    var result = command.ExecuteScalar();
                    // user's exists
                    if (result != null)
                    {
                        return -1;
                    }
                } catch (SqlException)
                {
                    // error
                    return -2;
                }

                // user's not exists and we can register it
                command.Parameters.Clear(); // clear parameters from the query batch
                command.CommandText = "INSERT INTO Users (Username, Password, Email) VALUES (@username, @password, @email)";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", this.Hash(password));
                command.Parameters.AddWithValue("@email", email);

                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // run command that we don't expect output from
                int results;
                try
                {
                    results = command.ExecuteNonQuery();
                } catch (SqlException)
                {
                    return -2;
                }

                return results;
            }
        }

        /// <summary>
        /// Get user id by username.
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user id, -1 if error</returns>
        private int GetUsersID(string username)
        {
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionString"]))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id FROM Users WHERE Username = @username";
                command.Parameters.AddWithValue("@username", username);
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var id = command.ExecuteScalar();

                // error on getting player id
                if (id == null)
                {
                    return -1;
                }

                return (int)id;
            }
        }

        /// <summary>
        /// Register multiplayer game data.
        /// </summary>
        /// <param name="player1">host</param>
        /// <param name="player2">guest</param>
        /// <param name="winner">winner</param>
        /// <returns>true if success, false otherwise</returns>
        public bool RegisterGameResults(string player1, string player2, string winner)
        {
            int firstID = this.GetUsersID(player1);
            int secondID = this.GetUsersID(player2);
            int winnerID = (player1 == winner) ? firstID : secondID;

            // in case of error
            if (firstID < 0 || secondID < 0)
            {
                return false;
            }

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionString"]))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO Games (Player1ID, Player2ID, Winner) VALUES (@firstID, @secondID, @winnerID)";
                command.Parameters.AddWithValue("@firstID", firstID);
                command.Parameters.AddWithValue("@secondID", secondID);
                command.Parameters.AddWithValue("@winnerID", winnerID);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // run command that we don't expect output from
                int results;
                try
                {
                    results = command.ExecuteNonQuery();
                    // error == we want to add one row and got less than one rows in results
                    if (results < 1)
                    {
                        return false;
                    }
                }
                catch (SqlException)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Retrieve a users ranking.
        /// </summary>
        /// <returns>a ranking list</returns>
        public JArray GetGameResults()
        {
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionString"]))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"SELECT Users.Username, Users.id, COUNT(CASE Games.Winner WHEN Users.id THEN 1 ELSE null END) AS Wins, COUNT(Games.Id) AS NumOfGames
                                        FROM Users LEFT OUTER JOIN Games
                                        ON(Users.Id = Games.Player1ID OR Users.id = Games.Player2ID)
                                        GROUP BY Users.Username, Users.Id
                                        ORDER BY Wins DESC";
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (SqlException)
                {
                    return null;
                }

                JArray results = new JArray();
                while (reader.Read())
                {
                    JObject row = new JObject();
                    row["Username"] = reader["Username"].ToString();
                    row["Wins"] = reader["Wins"].ToString();
                    row["Losses"] = (reader.GetInt32(3) - reader.GetInt32(2)).ToString();

                    results.Add(row);
                }

                return results;
            }
        }
    }
}