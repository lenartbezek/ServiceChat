using System.Web.Http;

namespace ChatDB
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        // Get user data by username
        // GET api/<controller>/name
        [AllowAnonymous]
        public object Get(string id)
        {
            try
            {
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