using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace ServiceChat
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
        public string DisplayName => FirstName.Length > 0 
            ? $"{FirstName} {LastName}" 
            : Username;

        [DataMember]
        public string FirstName { get; private set; }
        [DataMember]
        public string LastName { get; private set; }
        [DataMember]
        public string Username { get; private set; }
        [DataMember]
        public bool Admin { get; set; }

        private static readonly CacheItemPolicy CachePolicy = new CacheItemPolicy
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

        public enum AuthenticationType
        {
            Any = 0,
            BasicHttp = 1,
            SessionCookie = 2
        }

        /// <summary>
        /// Authenticates current user.
        /// Returns Account if successful and null otherwise.
        /// </summary>
        public static Account Authenticate(AuthenticationType type = AuthenticationType.Any)
        {
            switch (type)
            {
                case AuthenticationType.Any:
                    return AuthenticateBasicHttp() ?? AuthenticateSessionCookie();
                case AuthenticationType.BasicHttp:
                    return AuthenticateBasicHttp();
                case AuthenticationType.SessionCookie:
                    return AuthenticateSessionCookie();
                default:
                    return null;
            }
        }

        private static Account AuthenticateBasicHttp()
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader == null) return null;

            var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);
            string credentials;
            // RFC 2617 sec 1.2, "scheme" name is case-insensitive
            if (authHeaderVal.Scheme.Equals("basic",
                    StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                credentials = authHeaderVal.Parameter;
            else
                return null;

            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                var separator = credentials.IndexOf(':');
                var name = credentials.Substring(0, separator);
                var password = credentials.Substring(separator + 1);

                var account = Get(name);
                return account.VerifyPassword(password) ? account : null;
            }
            catch
            {
                return null;
            }
        }

        private static Account AuthenticateSessionCookie()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return null;

            try
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null) return null;

                var name = new FormsIdentity(authTicket).Name;
                return Get(name);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a session cookie for this user. To be called at login.
        /// </summary>
        public void CreateSessionCookie()
        {
            var cookie = FormsAuthentication.GetAuthCookie(Username, true);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Removes the session cookie for this user. To be called at logout.
        /// </summary>
        public void RemoveSessionCookie()
        {
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
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

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand("SELECT username, ime, priimek, geslo, admin FROM Uporabnik WHERE username=@username", conn);

            command.Parameters.AddWithValue("@username", username);
            
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new Account
                    {
                        Username = (string)reader["username"],
                        FirstName = (string)reader["ime"],
                        LastName = (string)reader["priimek"],
                        Admin = (bool)reader["admin"],
                        _hashedPassword = (string)reader["geslo"]
                    };
                    
                    MemoryCache.Default.Set("user-"+user.Username, user, CachePolicy);
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

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand("SELECT username, ime, priimek, geslo, admin FROM Uporabnik", conn);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new Account
                    {
                        Username = (string)reader["username"],
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
        /// Verifies the password against the stored hash.
        /// Returns true if valid.
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return _hashedPassword == Hash(password);
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
            command.Parameters.AddWithValue("@firstname", FirstName);
            command.Parameters.AddWithValue("@lastname", LastName);
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
            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "INSERT INTO Uporabnik (username, ime, priimek, geslo, role, admin) "+
                "VALUES (@username, @firstname, @lastname, @hash, @role, @admin) ",
                conn);
            command.Parameters.AddWithValue("@username", Username);
            command.Parameters.AddWithValue("@firstname", FirstName);
            command.Parameters.AddWithValue("@lastname", LastName);
            command.Parameters.AddWithValue("@admin", Admin);
            command.Parameters.AddWithValue("@hash", _hashedPassword);

            command.ExecuteNonQuery();

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
@"BEGIN TRANSACTION DeleteUser

BEGIN TRY

    DELETE FROM Uporabnik
    WHERE username=@username

    DELETE FROM Pogovor
    WHERE username=@username

COMMIT TRANSACTION DeleteUser

END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION DeleteUser
END CATCH  

GO",
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