using System.Runtime.Serialization;
using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    public class LoginController : ApiController
    {
        [DataContract]
        private struct LoginResponse
        {
            [DataMember]
            public bool Success;
            [DataMember]
            public bool Admin;
            [DataMember]
            public string Error;
            [DataMember]
            public Account Account;

            public const string InvalidCredentials = "InvalidCredentials";
            public const string UserNotFoundError = "UserNotFound";
            public const string InvalidPasswordError = "InvalidPassword";
        }

        /// <summary>
        /// Authenticates GET request via http basic auth.
        /// </summary>
        /// <returns>Returns LoginResponse as JSON.</returns>
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account != null)
                {
                    return Ok(new LoginResponse
                    {
                        Success = true,
                        Admin = account.Admin,
                        Account = account
                    });
                }
                else
                {
                    return Ok(new LoginResponse
                    {
                        Error = LoginResponse.InvalidCredentials
                    });
                }
            }
            catch
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Authenticates POST request from Post body.
        /// </summary>
        /// <param name="data">
        /// {
        ///     "Username": "admin",
        ///     "Password": "Geslo.01"
        /// }
        /// </param>
        /// <returns>Returns LoginResponse as JSON.</returns>
        public object Post([FromBody] dynamic data)
        {
            try
            {
                if (data == null ||
                    data.Username == null ||
                    data.Password == null)
                    return BadRequest();

                var account = Account.Get((string)data.Username);
                if (account == null) return Ok(new LoginResponse { Error = LoginResponse.UserNotFoundError });

                var valid = account.VerifyPassword((string)data.Password);
                if (!valid) return Ok(new LoginResponse { Error = LoginResponse.InvalidPasswordError });

                return new LoginResponse { Success = true, Admin = account.Admin, Account = account };
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}