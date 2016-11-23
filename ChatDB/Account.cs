using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Runtime.Caching;

namespace ChatDB
{
    /// <summary>
    /// A class for handling all user accounts.
    /// Implements full CRUD behaviour.
    /// </summary>
    public class Account
    {
        public string DisplayName => FirstName.Length > 0 ? FirstName + " " + LastName : Username;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }

        private static readonly CacheItemPolicy _cachePolicy = new CacheItemPolicy
        {
            SlidingExpiration = new TimeSpan(0, 30, 0)
        };

        private string _hashedPassword;

        /// <summary>
        /// Creates a new User with given data and returns it.
        /// Caches returned object.
        /// <exception cref="InvalidUsernameException">Throws InvalidUsernameException if username does not meet the requirements.</exception>
        /// <exception cref="InvalidPasswordException">Throws InvalidPasswordException if password does not meet the requirements.</exception>
        /// <exception cref="UsernameDuplicateException">Throws UsernameDuplicateException if username is already taken.</exception>
        /// </summary>
        public static Account Create(string firstname, string lastname, string username, string password)
        {
            if (username.Length < 4 ||
                Encoding.UTF8.GetByteCount(username) != username.Length)
                throw new InvalidUsernameException();

            var numericCharCount = 0;
            var uppercaseCharCount = 0;
            foreach (char c in password.ToCharArray())
            {
                if (Char.IsNumber(c)) numericCharCount++;
                if (Char.IsUpper(c)) uppercaseCharCount++;
            }

            if (password.Length < 8 ||
                !(password.Contains("?") ||
                  password.Contains(".") ||
                  password.Contains("*") ||
                  password.Contains("!") ||
                  password.Contains(":"))||
                numericCharCount < 2 ||
                uppercaseCharCount < 2)
                throw new InvalidPasswordException();

            var newUser = new Account
            {
                FirstName = firstname,
                LastName = lastname,
                Username = username,
                _hashedPassword = Hash(password)
            };

            try
            {
                newUser.Create();
                return newUser;
            }
            catch (SqlException)
            {
                throw new UsernameDuplicateException();
            }
        }

        /// <summary>
        /// Finds and returns User with given username.
        /// Caches returned object.
        /// <exception cref="UsernameNotFoundException">Throws UsernameNotFoundException if username is not found.</exception>
        /// </summary>
        public static Account Get(string username)
        {
            if (MemoryCache.Default.Contains(username))
                return (Account)MemoryCache.Default.Get(username);

            Account user = null;

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT username, ime, priimek, geslo FROM Uporabnik WHERE username=@username", conn);

            command.Parameters.AddWithValue("@username", username);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new Account
                    {
                        Username = (string)reader["username"],
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        _hashedPassword = (string)reader["geslo"]
                    };
                    
                    MemoryCache.Default.Set("user-"+user.Username, user, _cachePolicy);
                }
            }

            conn.Close();

            if (user == null)
                throw new UsernameNotFoundException();

            return user;
        }

        /// <summary>
        /// Returns a list of all registered users.
        /// Caches returned objects.
        /// </summary>
        public static List<Account> GetAll()
        {
            var list = new List<Account>();

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT username, ime, priimek, geslo FROM Uporabnik", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new Account
                    {
                        Username = (string)reader["username"],
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        _hashedPassword = (string)reader["geslo"]
                    };

                    MemoryCache.Default.Set("user-" + user.Username, user, _cachePolicy);
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
       
        /// <summary>
        /// Verifies the password against the stored hash.
        /// Returns true if valid.
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return _hashedPassword == Hash(password);
        }

        private void Update()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "UPDATE Uporabnik SET ime = @firstname, priimek = @lastname, geslo = @hash "+
                "WHERE username=@username",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@firstname", this.FirstName);
            command.Parameters.AddWithValue("@lastname", this.LastName);
            command.Parameters.AddWithValue("@hash", this._hashedPassword);

            command.ExecuteNonQuery();

            conn.Close();

            MemoryCache.Default.Set("user-" + Username, this, _cachePolicy);
        }

        private void Create()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "INSERT INTO Uporabnik (username, ime, priimek, geslo) "+
                "VALUES (@username, @firstname, @lastname, @hash) ",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@firstname", this.FirstName);
            command.Parameters.AddWithValue("@lastname", this.LastName);
            command.Parameters.AddWithValue("@hash", this._hashedPassword);

            command.ExecuteNonQuery();

            conn.Close();

            MemoryCache.Default.Set("user-" + Username, this, _cachePolicy);
        }

        private void Delete()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "DELETE FROM Uporabnik " +
                "WHERE username=@username",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@firstname", this.FirstName);
            command.Parameters.AddWithValue("@lastname", this.LastName);
            command.Parameters.AddWithValue("@hash", this._hashedPassword);

            command.ExecuteNonQuery();

            conn.Close();

            MemoryCache.Default.Remove("user-" + Username);
        }

        public class InvalidUsernameException : Exception
        {
            public override string Message => "Username must be at least four characters long and contain only ASCII characters.";
        }

        public class InvalidPasswordException : Exception
        {
            public override string Message => "Password must contain at least one haiku.";
        }

        public class UsernameDuplicateException : Exception
        {
            public override string Message => "Account with this username already exists.";
        }

        public class UsernameNotFoundException : Exception
        {
            public override string Message => "Account with this username doesn't exist.";
        }
    }
}