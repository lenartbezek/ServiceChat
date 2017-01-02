using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace ServiceChat.Models
{
    /*
CREATE TABLE [dbo].[Uporabnik] (
    [username] VARCHAR (50)  NOT NULL,
    [ime]      NVARCHAR (50) NULL,
    [priimek]  NVARCHAR (50) NOT NULL,
    [geslo]    VARCHAR (250) NOT NULL,
	[admin]    BIT  NOT NULL DEFAULT 0,
    PRIMARY KEY CLUSTERED ([username] ASC)
);
    */

    /// <summary>
    /// A class for handling all user accounts.
    /// Implements full CRUD behaviour.
    /// </summary>
    [DataContract]
    public class Account
    {
        [DataMember]
        public string DisplayName
        {
            get
            {
                return !string.IsNullOrEmpty(FirstName)
                    ? $"{FirstName} {LastName}"
                    : Username;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                var list = value.Split(' ').ToList();
                FirstName = list[0];
                LastName = list.Count > 1 ? list[1] : "";
                for (var i = 2; i < list.Count; i++)
                    LastName += " " + list[i];
            }
        }

        [DataMember]
        public string Username { get; }

        [DataMember]
        public bool Admin { get; set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private static readonly CacheItemPolicy CachePolicy = new CacheItemPolicy
        {
            SlidingExpiration = new TimeSpan(0, 30, 0)
        };

        private string _hashedPassword;

        /// <summary>
        /// Verifies password format and sets the password hash.
        /// Returns false if password format is invalid.
        /// </summary>
        /// <param name="password">New password.</param>
        /// <returns>Is successful.</returns>
        public bool SetPassword(string password)
        {
            if (password == null) return false;

            var numericCharCount = 0;
            var uppercaseCharCount = 0;
            foreach (var c in password)
            {
                if (char.IsNumber(c)) numericCharCount++;
                if (char.IsUpper(c)) uppercaseCharCount++;
            }

            if (password.Length < 8 ||
                !(password.Contains("?") ||
                  password.Contains(".") ||
                  password.Contains("*") ||
                  password.Contains("!") ||
                  password.Contains(":")) ||
                numericCharCount < 2 ||
                uppercaseCharCount < 1)
                return false;

            _hashedPassword = Hash(password);
            return true;
        }

        /// <summary>
        /// Verifies the password against the stored hash.
        /// Returns true if valid.
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return _hashedPassword == Hash(password);
        }

        /// <summary>
        /// Creates new account with given username.
        /// Intended for user registration.
        /// Does not cache it or save it to database.
        /// To save it to the database, call `account.Create()`.
        /// <exception cref="InvalidUsernameException"></exception>
        /// /// <exception cref="InvalidPasswordException"></exception>
        /// </summary>
        public Account(string username, string password, string displayName=null)
        {
            if (username.Length < 4 ||
                Encoding.UTF8.GetByteCount(username) != username.Length)
                throw new InvalidUsernameException();

            Username = username;
            DisplayName = displayName;

            var validPassword = SetPassword(password);

            if (!validPassword) throw new InvalidPasswordException();
        }

        /// <summary>
        /// Private constructor for Account type.
        /// Intended for loading user accounts from the database.
        /// </summary>
        /// <param name="username"></param>
        private Account(string username)
        {
            Username = username;
        }

        /// <summary>
        /// Finds and returns User with given username.
        /// Caches returned object.
        /// </summary>
        public static Account Get(string username)
        {
            if (MemoryCache.Default.Contains("user-" + username))
                return (Account)MemoryCache.Default.Get("user-" + username);

            Account user = null;

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand("SELECT username, ime, priimek, geslo, admin FROM Uporabnik WHERE username=@username", conn);

            command.Parameters.AddWithValue("@username", username);
            
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new Account((string)reader["username"])
                    {
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        Admin = (bool)reader["admin"],
                        _hashedPassword = (string)reader["geslo"]
                    };
                    
                    MemoryCache.Default.Set("user-"+user.Username, user, CachePolicy);
                }
            }

            conn.Close();

            return user;
        }

        /// <summary>
        /// Returns a list of all registered users.
        /// Caches returned objects.
        /// </summary>
        public static List<Account> GetAll()
        {
            var list = new List<Account>();

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand("SELECT username, ime, priimek, geslo, admin FROM Uporabnik", conn);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new Account((string)reader["username"])
                    {
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        Admin = (bool)reader["admin"],
                        _hashedPassword = (string)reader["geslo"]
                    };

                    MemoryCache.Default.Set("user-" + user.Username, user, CachePolicy);
                    list.Add(user);
                }
            }

            conn.Close();

            return list;
        }

        private static string Hash(string s)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(s);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Saves the changes to this Account to the database.
        /// </summary>
        public void Update()
        {
            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "UPDATE Uporabnik SET ime = @firstname, priimek = @lastname, geslo = @hash, admin = @admin "+
                "WHERE username=@username",
                conn);
            command.Parameters.AddWithValue("@username", Username);
            command.Parameters.AddWithValue("@firstname", FirstName ?? "");
            command.Parameters.AddWithValue("@lastname", LastName ?? "");
            command.Parameters.AddWithValue("@admin", Admin);
            command.Parameters.AddWithValue("@hash", _hashedPassword);

            command.ExecuteNonQuery();

            conn.Close();

            MemoryCache.Default.Set("user-" + Username, this, CachePolicy);
        }

        /// <summary>
        /// Creates a new user account in the database.
        /// Adds this user to the memory cache.
        /// </summary>
        public void Create()
        {
            if (_hashedPassword == null)
                throw new InvalidOperationException("Account cannot be saved with no password set.");

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "INSERT INTO Uporabnik (username, ime, priimek, geslo, admin) "+
                "VALUES (@username, @firstname, @lastname, @hash, @admin) ",
                conn);
            command.Parameters.AddWithValue("@username", Username);
            command.Parameters.AddWithValue("@firstname", FirstName ?? "");
            command.Parameters.AddWithValue("@lastname", LastName ?? "");
            command.Parameters.AddWithValue("@admin", Admin);
            command.Parameters.AddWithValue("@hash", _hashedPassword);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new UsernameDuplicateException();
            }

            conn.Close();

            MemoryCache.Default.Set("user-" + Username, this, CachePolicy);
        }

        /// <summary>
        /// Deletes an account and all it's messages.
        /// </summary>
        public void Delete()
        {
            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "DELETE FROM Uporabnik " + 
                "WHERE username=@username " + 
                "DELETE FROM Pogovor " + 
                "WHERE username=@username ",
                conn);
            command.Parameters.AddWithValue("@username", Username);
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