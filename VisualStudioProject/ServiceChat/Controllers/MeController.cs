using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/me")]
    public class MeController : ApiController
    {

        // Get logged in user data
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                return Ok(account);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}