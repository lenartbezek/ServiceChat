using System.Web.Http;
using ServiceChat.Models;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/register")]
    public class RegisterController : ApiController
    {

        // Register new user
        public object Post([FromBody]dynamic data)
        {
            if (data == null ||
                data.Username == null ||
                data.Password == null)
                return BadRequest();

            try
            {
                var newAccount = new Account((string)data.Username, (string)data.Password, (string)data.DisplayName);
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