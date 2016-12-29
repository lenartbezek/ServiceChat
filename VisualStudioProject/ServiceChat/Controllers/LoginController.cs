using System.Runtime.Serialization;
using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/login")]
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

            public const string InvalidCredentials = "InvalidCredentials";
            public const string UserNotFoundError = "UserNotFound";
            public const string InvalidPasswordError = "InvalidPassword";
        }

        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account != null)
                {
                    CreateAuthCookie(account);
                    return Ok(new LoginResponse
                    {
                        Success = true,
                        Admin = account.Admin
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

                return new LoginResponse { Success = true, Admin = account.Admin };
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}