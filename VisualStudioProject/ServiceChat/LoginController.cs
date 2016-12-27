using System.Web.Http;

namespace ServiceChat
{
    [RoutePrefix("Service1.svc/Login")]
    public class LoginController : ApiController
    {
        // Get logged in user data
        // GET Service1.svc/Login
        public object Get()
        {
            try
            {
                var account = Account.Authenticate();
                return Ok(account != null);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}