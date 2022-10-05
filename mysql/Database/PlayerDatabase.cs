using AltV.Net;
using mysql.MyEntitys;
using mysql.Utility;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mysql.Database
{
    class PlayerDatabase
    {

        public static int CreatePlayer(string username, string email, string password)
        {
            string saltedPassword = PasswordDerivation.Derive(password);

            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO players (name, email, password) VALUES (@name, @email, @password)";
                command.Parameters.AddWithValue("@name", username);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", saltedPassword);

                command.ExecuteNonQuery();
                connection.Close();
                return (int)command.LastInsertedId;
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
            return -1;
        }

        public static void LoadPlayer(MyPlayer player)
        {
            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM players WEHERE name=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", player.DisplayName);


                using(MySqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        reader.Read();
                        player.Db_Id = reader.GetInt16("id");
                        player.Cash = reader.GetUInt32("cash");
                        player.Bank = reader.GetUInt32("bank");
                        player.BlackMoney = reader.GetUInt32("black_money");
                    }
                }

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
        }

        public static void UpdatePlayer(MyPlayer player)
        {
            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE players SET name=@name, cash=@cash, bank=@bank, black_money=@blackmoney WHERE id=@id";

                command.Parameters.AddWithValue("@id", player.Db_Id);
                command.Parameters.AddWithValue("@name", player.DisplayName);
                command.Parameters.AddWithValue("@cash", player.Cash);
                command.Parameters.AddWithValue("@bank", player.Bank);
                command.Parameters.AddWithValue("@blackmoney", player.BlackMoney);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
        }

        public static bool CheckLoginDetails(string email, string input)
        {
            string password = "";
            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT password FROM players WEHERE email=@email LIMIT 1";
                command.Parameters.AddWithValue("@email", email);


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        password = reader.GetString("password");
             
                    }
                }
                connection.Close();
                if(PasswordDerivation.Verify(password, input)) return true;
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
            return false;
        }

        public static bool DoesPlayerNameExists(string username)
        {
            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM players WEHERE name=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", username);


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
            return false;
        }

        public static bool DoesPlayerEmailExists(string email)
        {
            try
            {
                MySqlConnection connection = MyDatabase.DB.GetConnection();
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM players WEHERE email=@email LIMIT 1";
                command.Parameters.AddWithValue("@email", email);


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
            return false;
        }

    }
}
