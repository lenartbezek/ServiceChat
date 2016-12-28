using System.Web.Http;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    public class LoginController : ApiController
    {
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account != null)
                {
                    CreateAuthCookie(account);
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}