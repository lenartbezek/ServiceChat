using System.Web.Http;

namespace ServiceChat
{
    [RoutePrefix("Service1.svc/User")]
    public class UserController : ApiController
    {
        // Get user data by username
        // GET Service1.svc/User
        public object Get(string id)
        {
            try
            {
                var account = Account.Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Account.Get(id));
            }
            catch (Account.UsernameNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}