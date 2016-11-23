using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatDB
{
    public partial class Chat : Page
    {
        public List<Message> MessageList { get; } = Message.GetAll();

        protected void Page_Load(object sender, EventArgs e)
        {

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
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}