using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ChatDB
{
	public partial class Login : Page
	{
        protected string RegisterSuccessMessage { get; private set; } = "";
        protected string UsernameError { get; private set; } = "";
        protected string PasswordError { get; private set; } = "";
        protected string RegisterNameError { get; private set; } = "";
        protected string RegisterUsernameError { get; private set; } = "";
        protected string RegisterPasswordError { get; private set; } = "";
        protected string RegisterPasswordRepeatError { get; private set; } = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (UsernameField.Text.Trim().Length <= 0)
                UsernameError = "Vnesite uporabniško ime";

            if (PasswordField.Text.Trim().Length <= 0)
                PasswordError = "Vnesite geslo";

            try
            {
                var account = Account.Get(UsernameField.Text);
                if (account.VerifyPassword(PasswordField.Text))
                    FormsAuthentication.RedirectFromLoginPage(account.Username, true);
                else
                    PasswordError = "Napačno geslo";
            }
            catch (Account.UsernameNotFoundException)
            {
                UsernameError = "Ta račun ne obstaja";
            }
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            var list = RegisterNameField.Text.Split(' ').ToList<string>();
            var firstName = list[0];
            var lastName = list.Count > 1 ? list[1] : "";
            for (int i = 2; i < list.Count; i++)
                lastName += " " + list[i];

            if (RegisterPasswordField.Text != RegisterPasswordRepeatField.Text)
                RegisterPasswordRepeatError = "Gesli se ne ujemata";

            try
            {
                var account = Account.Create(firstName, lastName, RegisterUsernameField.Text, RegisterPasswordField.Text);
                RegisterSuccessMessage = "Uporabniški račun uspešno ustvarjen";
            }
            catch (Account.InvalidUsernameException)
            {
                RegisterUsernameError = "Uporabniško ime mora vsebovati vsaj 4 znake in le ASCII znake";
            }
            catch (Account.InvalidPasswordException)
            {
                RegisterPasswordError = "Geslo more bit ful zajebano z ciframi in klicaji in dvema velkima črkama";
            }
            catch (Account.UsernameDuplicateException)
            {
                RegisterUsernameError = "Račun z tem uporabniškim imenom že obstaja";
            }
        }
	}
}