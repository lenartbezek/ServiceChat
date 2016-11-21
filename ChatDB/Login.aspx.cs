using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatDB
{
	public partial class Login : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectFromLoginPage(UsernameField.Text, true);
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            var list = RegisterNameField.Text.Split(' ').ToList<string>();
            var firstName = list[0];
            var lastName = list[1];
            ChatDB.User.Create(firstName, lastName, RegisterUsernameField.Text, RegisterPasswordField.Text);
        }
	}
}