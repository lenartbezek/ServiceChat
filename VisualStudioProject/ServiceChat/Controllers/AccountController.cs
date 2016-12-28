using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    public class AccountController : ApiController
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

        // Register new user
        public object Post([FromBody]dynamic data)
        {
            try
            {
                var newAccount = new Account(data.Username, data.Password, data.DisplayName);
                newAccount.Create();
                return Ok(newAccount);
            }
            catch (Account.InvalidUsernameException)
            {
                return BadRequest("InvalidUsername");
            }
            catch (Account.InvalidPasswordException)
            {
                return BadRequest("InvalidPassword");
            }
            catch (Account.UsernameDuplicateException)
            {
                return BadRequest("DuplicateUsername");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}