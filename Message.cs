using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web;
using static ChatDB.Account;

namespace ChatDB
{
    /*
     * CREATE TABLE [dbo].[Pogovor] (
     * [id]       INT          NOT NULL,
     * [username] VARCHAR (50) NOT NULL,
     * [besedilo] TEXT         NOT NULL,
     * [time] DATETIME NOT NULL, 
     * PRIMARY KEY CLUSTERED ([id] ASC)
     * );
     */

    /// <summary>
    /// A class for handling all messages.
    /// Implements full CRUD behaviour.
    /// </summary>
    public class Message
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public string Username { get; private set; }
        public Account Author { get; private set; }
        public DateTime Time { get; private set; }

        public static Message Create(string username, string text)
        {
            var newMessage = new Message
            {
                Username = username,
                Text = text,
                Time = DateTime.UtcNow

            };

            newMessage.Create();
            return newMessage;
            
        }

        /// <summary>
        /// Finds and returns Message with given id.
        /// <exception cref="IdNotFoundException">Throws IdNotFoundException if id is not found.</exception>
        /// </summary>
        public static Message Get(int id)
        {
            Message message = null;

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "SELECT username, besedilo, time "+
                "FROM Pogovor "+
                "WHERE id=@id", 
                conn);

            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    message = new Message
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = (DateTime)reader["time"]
                    };
                }
            }

            conn.Close();

            if (message == null)
                throw new IdNotFoundException(); 

            return message;
        }

        /// <summary>
        /// Returns a list of all messages.
        /// </summary>
        public static List<Message> GetAll()
        {
            var list = new List<Message>();

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "SELECT username, besedilo, time "+
                "FROM Pogovor", 
                conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = (DateTime)reader["time"]
                    };

                    list.Add(message);
                }
            }

            conn.Close();

            return list;
        }

        /// <summary>
        /// Returns a list of all messages with author's name and surname before them.
        /// </summary>
        public static List<Message> GetAllWithAuthors()
        {
            var list = new List<Message>();

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "SELECT u.ime, u.priimek, p.username, p.besedilo "+
                "FROM Uporabnik AS u "+
                "INNER JOIN Pogovor AS p "+
                "ON u.username=p.id; ", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message
                    {
                        Username = (string)reader["ime"] + " " + (string)reader["priimek"]+ " " + (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = (DateTime)reader["time"]
                    };

                    list.Add(message);
                }
            }

            conn.Close();

            return list;
        }

        private void Create()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "INSERT INTO Pogovor (username, besedilo, time) " +
                "VALUES (@username, @besedilo, @time) ",
                conn);

            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@besedilo", this.Text);
            command.Parameters.AddWithValue("@time", this.Time);


            command.ExecuteNonQuery();

            conn.Close();
        }

        private void Update()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "UPDATE Pogovor SET besedilo = @besedilo,  time = @time" +
                "WHERE username=@username",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@besedilo", this.Text);
            command.Parameters.AddWithValue("@time", this.Time);

            command.ExecuteNonQuery();

            conn.Close();
        }

        private void Delete()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "DELETE FROM Pogovor " +
                "WHERE username=@username",
                conn);
            command.Parameters.AddWithValue("@username", this.Username);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public class IdNotFoundException : Exception
        {
            public override string Message => "Message with this id doesn't exist.";
        }
    }
}