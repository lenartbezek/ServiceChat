using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using ServiceChat.Models;

namespace ServiceChat
{
    public class Authentication
    {
        /// <summary>
        /// Authenticates using BasicHttp authentication from request headers.
        /// </summary>
        /// <returns>Authenticated account if successful and null otherwise.</returns>
        public static Account Authenticate()
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
    }
}