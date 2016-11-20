using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ChatDB
{
    public class User
    {
        public string DisplayName => FirstName + " " + LastName;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }

        private string _hashedPassword;

        public static User Create(string firstname, string lastname, string username, string password)
        {
            var newUser = new User
            {
                FirstName = firstname,
                LastName = lastname,
                Username = username,
                _hashedPassword = Hash(password)
            };

            newUser.Save();

            return newUser;
        }

        public static User Get(string username)
        {
            User user = null;

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT username, ime, priimek, geslo FROM Uporabnik WHERE username=@username", conn);
            command.Parameters.AddWithValue("@username", username);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Username = (string)reader["username"],
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        _hashedPassword = (string)reader["geslo"]
                    };
                }
            }

            conn.Close();

            return user;
        }

        public static List<User> GetAll()
        {
            var list = new List<User>();

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT username, ime, priimek, geslo FROM Uporabnik", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new User
                    {
                        Username = (string)reader["username"],
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        _hashedPassword = (string)reader["geslo"]
                    };

                    list.Add(user);
                }
            }

            conn.Close();

            return list;
        }

        private static string Hash(string s)
        {
            var md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(s);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

        private bool VerifyPassword(string password)
        {
            return _hashedPassword == Hash(password);
        }

        private void Save()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "BEGIN TRANSACTION "+
                "IF EXISTS (SELECT * FROM Uporabniki WITH (updlock,serializable) WHERE username=@username) "+
                "BEGIN "+
                "   UPDATE Uporabniki SET ime=@firstname, priimek=@lastname, geslo=@hash "+
                "   WHERE username=@username "+
                "END "+
                "ELSE "+
                "BEGIN "+
                "   INSERT INTO Uporabniki (username, ime, priimek, geslo) "+
                "   VALUES (@username, @firstname, @lastname, @hash) "+
                "END "+
                "COMMIT TRANSACTION",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@firstname", this.FirstName);
            command.Parameters.AddWithValue("@lastname", this.LastName);
            command.Parameters.AddWithValue("@hash", this._hashedPassword);

            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}