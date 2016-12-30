using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        // Get all accounts
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Account.GetAll());
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Get account by username
        [Route("/{id}")]
        public object Get(string id)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Account.Get(id));
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Edit user
        [Route("/{id}")]
        public object Put(string id, [FromBody]dynamic data)
        {
            if (data == null ||
                data.DisplayName == null)
                return BadRequest();

            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var victim = Account.Get(id);
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