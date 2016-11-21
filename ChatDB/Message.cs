using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            throw new NotImplementedException();
        }

        public static Message Get(int id)
        {
            throw new NotImplementedException();
        }

        public static List<Message> GetAll()
        {
            throw new NotImplementedException();
        }

        public static List<Message> GetAllWithAuthors()
        {
            throw new NotImplementedException();
        }

        private void Create()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }
    }
}