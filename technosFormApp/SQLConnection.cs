using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using technosFormApp.classes;

namespace technosFormApp
{
    internal class SQLConnection
    {

        public List<User> selectUsers(string query)
        {
            List<Dictionary<string, object>> resuls = run_select(query);
            List<User> users = new List<User>();
            foreach(Dictionary<string, object> result in resuls)
            {
                User user_ = new User(result["email"].ToString(), result["password_digest"].ToString(), Convert.ToInt32(result["id"]));
                users.Add(user_);
            }  
            return users;
        }

        public List<Word> selectWords(string query)
        {
            List<Dictionary<string, object>> resuls = run_select(query);
            List<Word> words = new List<Word>();
            foreach (Dictionary<string, object> result in resuls)
            {
                Word word_ = new Word();
                word_.word = StringOrNull(result, "word");
                word_.translation = StringOrNull(result, "translation");
                word_.id=IntOrNull(result, "id");
                word_.techno_id=IntOrNull(result, "techno_id");
                word_.techno_name = StringOrNull(result, "techno_name");
                word_.user_id = IntOrNull(result, "user_id");
                words.Add(word_);
            }
            return words;
        }

        public List<Techno> selectTechnos(string query)
        {
            List<Dictionary<string, object>> resuls = run_select(query);
            List<Techno> technos = new List<Techno>();
            foreach (Dictionary<string, object> result in resuls)
            {
                Techno techno_ = new Techno();
                techno_.techno_name = StringOrNull(result, "techno_name");
                techno_.techno_status = BoolOrNull(result, "techno_status");
                techno_.id = IntOrNull(result, "id");
                techno_.user_id = IntOrNull(result, "user_id");
                technos.Add(techno_);
            }
            return technos;
        }

        public String? StringOrNull(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key)) return dict[key].ToString();
            return null;
        }

        public int? IntOrNull(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key)) return Convert.ToInt32(dict[key]);
            return null;
        }

        public bool? BoolOrNull(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key)) return Convert.ToBoolean(dict[key]);
            return null;
        }

        public string connectionString = "Data Source=DESKTOP-C4J7U97\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
        public List<Dictionary<string, object>> run_select(string query)
        {

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row.Add(reader.GetName(i), reader.GetValue(i));
                                }
                                results.Add(row);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error executing SELECT query: " + e.Message);
            }
            return results;
        }

        public int run_command(string query)
        {
            int affectedRows = 0;
            try
            {
                // Create a new SqlConnection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the command
                        affectedRows = command.ExecuteNonQuery();

                        // Optionally, do something with the result, e.g., check how many rows were affected
                        Console.WriteLine($"{affectedRows} rows were updated.");
                    }
                }
            }
            catch (SqlException e)
            {
                // Handle any SQL errors here
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                // Handle other errors here
                Console.WriteLine($"Error: {e.Message}");
            }
            return affectedRows;
        }
    }
}

