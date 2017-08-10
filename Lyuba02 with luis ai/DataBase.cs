using System;
using System.Data.SqlClient;

namespace Lyuba02_with_luis_ai
{
    [Serializable]
    class DataBase
    {

        private static string server = "your_server.database.windows.net";
        private static string user = "your_user";
        private static string password = "your_password";
        private static string database = "your_database";

        public bool IsExistNumber(string number)
        {

            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT num FROM numberstab ;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetString(0).Equals(number))
                                    return true;
                            }
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.ToString());
            }

            return false;
        }


        public void InsertInDbNumber(string number)
        {
            try
            {
                //    string query = "DROP TABLE IF EXISTS numberstab;" +
                //"CREATE TABLE numberstab ( num  nvarchar(10)          not null PRIMARY KEY ); ";
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO numberstab (num) VALUES ('" + number + "') ; ";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                
            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        
        public string AllNumbers()
        {
            string numbers = "";
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT num FROM numberstab ;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                numbers += reader.GetString(0) + " ";
                            }
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                return e.ToString();
            }

            return numbers;
        }


        public bool IsExistMail(string mail)
        {

            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT mail FROM mailstab ;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetString(0).Equals(mail))
                                    return true;
                            }
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.ToString());
            }

            return false;
        }


        public void InsertInDbMail(string mail)
        {
            try
            {
                //    string query = "DROP TABLE IF EXISTS mailstab;" +
                //"CREATE TABLE mailstab ( mail  nvarchar(50)          not null PRIMARY KEY ); ";
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO mailstab (mail) VALUES ('" + mail + "') ; ";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.ToString());
            }
        }


        public string AllMails()
        {
            string mails = "";
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = server;
                cb.UserID = user;
                cb.Password = password;
                cb.InitialCatalog = database;

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT mail FROM mailstab ;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mails += reader.GetString(0) + " ";
                            }
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                return e.ToString();
            }

            return mails;
        }


    }
}
