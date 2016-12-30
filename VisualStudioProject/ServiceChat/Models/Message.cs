using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace ServiceChat.Models
{
    /*
CREATE TABLE [dbo].[Pogovor] (
    [id]       INT          IDENTITY (1, 1) NOT NULL,
    [username] VARCHAR (50) NOT NULL,
    [besedilo] TEXT         NULL,
    [time]     DATETIME     NOT NULL
    PRIMARY KEY CLUSTERED ([id] ASC)
);
     */

    /// <summary>
    /// A class for handling all messages.
    /// Implements full CRUD behaviour.
    /// </summary>
    [DataContract]
    public class Message
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Username { get; private set; }
        [DataMember]
        public DateTime Time { get; private set; }

        public Message(Account account, string text)
        {
            Username = account.Username;
            Text = text;
            Time = DateTime.UtcNow;
        }

        private Message(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Finds and returns Message with given ID.
        /// Returns null if not found.
        /// </summary>
        public static Message Get(int id)
        {
            Message message = null;

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "SELECT * " +
                "FROM Pogovor " +
                "WHERE id=@id ",
                conn);

            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    message = new Message(id)
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToUniversalTime()
                    };
                }
            }

            conn.Close();

            return message;
        }

        /// <summary>
        /// Returns a list of all messages.
        /// </summary>
        public static List<Message> GetAll()
        {
            var list = new List<Message>();

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "SELECT * " +
                "FROM Pogovor " +
                "ORDER BY time ASC ",
                conn);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message((int)reader["id"])
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToUniversalTime()
                    };

                    list.Add(message);
                }
            }

            conn.Close();

            return list;
        }

        /// <summary>
        /// Returns a list of all messages that arrived after the message with given ID.
        /// </summary>
        /// <param name="id">ID of the message.</param>
        /// <returns>List of Messages sorted from oldest to newest.</returns>
        public static List<Message> GetSince(int id)
        {
            var list = new List<Message>();

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "SELECT * " +
                "FROM Pogovor " +
                "WHERE time > "+
                    "(SELECT time " +
                    "FROM Pogovor " +
                    "WHERE id=@id ) " +
                "ORDER BY time ASC ",
                conn);

            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message((int)reader["id"])
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToUniversalTime()
                    };

                    list.Add(message);
                }
            }

            conn.Close();

            return list;
        }

        /// <summary>
        /// Returns a list of all messages since given date.
        /// </summary>
        public static List<Message> GetSince(DateTime time)
        {
            var list = new List<Message>();

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "SELECT * " +
                "FROM Pogovor " +
                "WHERE time > @time "+
                "ORDER BY time ASC ",
                conn);

            command.Parameters.AddWithValue("@time", time);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message((int)reader["id"])
                    {
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToUniversalTime()
                    };

                    list.Add(message);
                }
            }

            conn.Close();

            return list;
        }

        /// <summary>
        /// Creates a new row for the message in the database.
        /// Assigns the Id to the message.
        /// </summary>
        public void Create()
        {
            if (string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Text))
                throw new InvalidOperationException("Cannot save a message with no text or user.");

            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "INSERT INTO Pogovor (username, besedilo, time) " +
                "OUTPUT INSERTED.id " +
                "VALUES (@username, @besedilo, @time) ",
                conn);

            command.Parameters.AddWithValue("@username", Username);
            command.Parameters.AddWithValue("@besedilo", Text);
            command.Parameters.AddWithValue("@time", Time);

            Id = (int)command.ExecuteScalar();

            conn.Close();
        }

        /// <summary>
        /// Updates the text of this message.
        /// </summary>
        public void Update()
        {
            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "UPDATE Pogovor SET besedilo=@besedilo" +
                "WHERE id=@id",
                conn);
            command.Parameters.AddWithValue("@id", Id);
            command.Parameters.AddWithValue("@besedilo", Text);

            command.ExecuteNonQuery();

            conn.Close();
        }

        public void Delete()
        {
            var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            var command = new SqlCommand(
                "DELETE FROM Pogovor " +
                "WHERE id=@id",
                conn);
            command.Parameters.AddWithValue("@id", Id);

            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}