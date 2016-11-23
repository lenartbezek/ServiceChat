using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web;

namespace ChatDB
{
    /*
     * CREATE TABLE [dbo].[Pogovor] (
     * [id]       INT IDENTITY(1, 1) NOT NULL,
     * [username] VARCHAR (50)       NOT NULL,
     * [besedilo] TEXT               NOT NULL,
     * [time] DATETIME               NOT NULL,
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
                "SELECT * " +
                "FROM Pogovor " +
                "WHERE id=@id ",
                conn);

            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    message = new Message
                    {
                        Id = id,
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToUniversalTime()
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
                "SELECT * " +
                "FROM Pogovor " +
                "ORDER BY time ASC ",
                conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message
                    {
                        Id = (int)reader["id"],
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToLocalTime()
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

            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "SELECT * " +
                "FROM Pogovor " +
                "WHERE time > @time "+
                "ORDER BY time ASC ",
                conn);

            command.Parameters.AddWithValue("@time", time);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var message = new Message
                    {
                        Id = (int)reader["id"],
                        Username = (string)reader["username"],
                        Text = (string)reader["besedilo"],
                        Time = ((DateTime)reader["time"]).ToLocalTime()
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
                "OUTPUT INSERTED.id " +
                "VALUES (@username, @besedilo, @time) ",
                conn);

            command.Parameters.AddWithValue("@username", this.Username);
            command.Parameters.AddWithValue("@besedilo", this.Text);
            command.Parameters.AddWithValue("@time", this.Time);

            Id = (int)command.ExecuteScalar();

            conn.Close();
        }

        private void Update()
        {
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            SqlCommand command = new SqlCommand(
                "UPDATE Pogovor SET besedilo=@besedilo, time=@time" +
                "WHERE id=@id",
                conn);
            command.Parameters.AddWithValue("@id", this.Id);
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
                "WHERE id=@id",
                conn);
            command.Parameters.AddWithValue("@username", this.Id);

            command.ExecuteNonQuery();

            conn.Close();
        }
        public class IdNotFoundException : Exception
        {
            public override string Message => "Message with this id doesn't exist.";
        }
    }
}