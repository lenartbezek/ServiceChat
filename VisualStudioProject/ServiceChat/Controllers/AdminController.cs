using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        // Get all accounts
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account == null || !account.Admin) return Unauthorized();

                return Ok(Account.GetAll());
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Edit user
        [Route("/{username}")]
        public object Put(string username, [FromBody]dynamic data)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var victim = Account.Get(username);
                if (victim == null) return NotFound();

                if (account.Username != victim.Username && !account.Admin) return Unauthorized();

                if (data.GetType().GetProperty("DisplayName") != null)
                    victim.DisplayName = data.DisplayName;
                else
                    return BadRequest();

                victim.Update();

                return Ok(victim);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Delete user
        [Route("/{username}")]
        public object Delete(string username)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var victim = Account.Get(username);
                if (victim == null) return BadRequest();

                if (account.Username != victim.Username && !account.Admin) return Unauthorized();

                victim.Delete();

                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}