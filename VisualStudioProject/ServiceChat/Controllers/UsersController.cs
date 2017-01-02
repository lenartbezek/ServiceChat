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
        public object Get(string id)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var a = Account.Get(id);
                if (a != null)
                    return Ok(a);
                else
                    return NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Edit user
        public object Put([FromBody]dynamic data)
        {
            if (data == null ||
                data.Username == null)
                return BadRequest();

            try
            {
                var account = Authenticate();
                if (account == null || !account.Admin) return Unauthorized();

                var victim = Account.Get((string) data.Username);
                if (victim == null) return NotFound();

                if (data.DisplayName != null)
                    victim.DisplayName = (string) data.DisplayName;
                if (data.Admin != null)
                    victim.Admin = (bool) data.Admin;

                victim.Update();

                return Ok(victim);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Delete user
        public object Delete(string id)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var victim = Account.Get(id);
                if (victim == null) return NotFound();

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