using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceChat
{
    public partial class AdminPanel : System.Web.UI.Page
    {
        protected Account Account;
        protected List<Account> AccountList;
        protected List<Message> MessageList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Account = Account.Authenticate(Account.AuthenticationType.SessionCookie);
            if (Account == null || !Account.Admin)
                Response.RedirectToRoute("Login");

            AccountList = Account.GetAll();
            MessageList = Message.GetAll();
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Account.RemoveSessionCookie();
            Response.RedirectToRoute("Login");
        }
    }
}