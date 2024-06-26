using System;
using System.Data.Common;
using MySql.Data.MySqlClient;


namespace ChatroomWithUserIdentification
{
    public class SignUpAndSignIn
    {


        public bool SignIn(string username, string password)
        {



            if (VerifyPasswordFromDatabase(username, password))
            {
                Console.WriteLine("Password is valid. Proceed!");
                return true;
            }

            Console.WriteLine("Invalid username or password. Try again.");

            return false;

        }

        public bool CreateNewAccount(string newUsername, string newPassword)
        {

            //Rules: no preexisting usernames, no blank
            string connectionString = "Server=localhost;Database=users;User Id=root;Password=;";

            using (DbConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Does this work?");
                    connection.Open();
                    string queryCheck = "SELECT COUNT(*) FROM user WHERE username = @Username";
                    MySqlCommand commandCheck = new MySqlCommand(queryCheck, (MySqlConnection)connection);
                    commandCheck.Parameters.AddWithValue("@Username", newUsername);
                    

                    object count = commandCheck.ExecuteScalar();
                    if (newUsername != "" && int.Parse(count.ToString()) == 0)
                    {
                        Console.WriteLine(count);
                        string query = "INSERT INTO user (username, password) VALUES (@Username, @Password)";
                        MySqlCommand command = new MySqlCommand(query,(MySqlConnection) connection);
                        command.Parameters.AddWithValue("@Username", newUsername);
                        command.Parameters.AddWithValue("@Password", newPassword);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {

                            Console.WriteLine("Account created successfully!");
                            return true;
                        }
                    }
                    
                    Console.WriteLine("Failed to create account.");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return false;
                }
            }
        }

        public bool VerifyPasswordFromDatabase(string username, string password)
        {
            
            string connectionString = "Server=localhost;Database=users;User Id=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM user WHERE username = @Username AND password = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    object count = command.ExecuteScalar();
                    return count != null && int.Parse(count.ToString()) > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
}
