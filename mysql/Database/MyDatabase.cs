using AltV.Net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mysql.Database
{
    class MyDatabase
    {
        public static MyDatabase DB { get; set; }

        private string ConnectionString { get; set; }

        public MySqlConnection Connection { get; set; }


        public MyDatabase()
        {
            ConnectionString = "SERVER=host;DATABASE=db;UID=user;PASSWORD=password;";
            Connection = new MySqlConnection(ConnectionString);
            DB = this;
            CreateTables(); 
        }

        public MySqlConnection GetConnection()
        {
            return Connection;
        }

        public void CreateTables()
        {
            try
            {
                Connection.Open();
                MySqlCommand command = Connection.CreateCommand();

                command.CommandText = "CREATE TABLE IF NOT EXISTS players (" + // As example
                    "id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY, " +
                    "name VARCHAR(40) NOT NULL," +
                    "email VARCHAR(40) NOT NULL," +
                    "password VARCHAR(128) NOT NULL," +
                    "cash INT(12) NOT NULL DEFAULT 0," +
                    "bank INT(12) NOT NULL DEFAULT 0," +
                    "black_money INT(12) NOT NULL DEFAULT 0," +
                    ")";
                command.ExecuteNonQuery();
                Connection.Close();

            } 
            catch (Exception ex)
            {
                Alt.Log(ex.StackTrace);
                Alt.Log(ex.Message);
            }
        }
    }
}
