using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Security;
using ServiceChat.Models;

namespace ServiceChat
{
    public class Authentication
    {
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
                    return AuthenticateBasicHttp() ?? AuthenticateCookie();
                case AuthenticationType.BasicHttp:
                    return AuthenticateBasicHttp();
                case AuthenticationType.SessionCookie:
                    return AuthenticateCookie();
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

                var account = Account.Get(name);
                return account.VerifyPassword(password) ? account : null;
            }
            catch
            {
                return null;
            }
        }

        private static Account AuthenticateCookie()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Expires > DateTime.Now) return null;

            try
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null) return null;

                var name = new FormsIdentity(authTicket).Name;
                return Account.Get(name);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a session cookie for this user. To be called at login.
        /// </summary>
        public static void CreateAuthCookie(Account account)
        {
            var cookie = FormsAuthentication.GetAuthCookie(account.Username, true);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Removes the session cookie for this user. To be called at logout.
        /// </summary>
        public static void ExpireAuthCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
                cookie.Expires = DateTime.Now.AddDays(-10);
        }

    }
}