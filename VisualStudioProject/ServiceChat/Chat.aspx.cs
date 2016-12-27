using System;
using System.Collections.Generic;
using System.Web.UI;

namespace ServiceChat
{
    public partial class Chat : Page
    {
        public List<Message> MessageList { get; } = Message.GetAll();

        private Account _account;

        protected void Page_Load(object sender, EventArgs e)
        {
            _account = Account.Authenticate(Account.AuthenticationType.SessionCookie);
            if (_account == null)
                Response.RedirectToRoute("Login");
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            var m = MessageField.Text.Trim();
            if (m.Length > 0)
                MessageList.Add(Message.Create(Context.User.Identity.Name, m));

            MessageField.Text = "";
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Default");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            _account.RemoveSessionCookie();
            Response.RedirectToRoute("Login");
        }
    }
}