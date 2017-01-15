using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    public class SendController : ApiController
    {
        // Posts a new message
        public object Post([FromBody]dynamic data)
        {
            if (data == null ||
                data.Text == null)
                return BadRequest();

            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var message = new Message(account, (string) data.Text);
                message.Create();
                return Ok(message);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Edits a message
        public object Put(int id, [FromBody]dynamic data)
        {
            if (data == null ||
                data.Text == null)
                return BadRequest();

            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var message = Message.Get(id);
                if (message == null) return NotFound();

                if (message.Username != account.Username || !account.Admin) return Unauthorized();

                message.Text = (string) data.Text;
                message.Update();
                return Ok(message);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Deletes a message
        public object Delete(int id)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var message = Message.Get(id);
                if (message == null) return NotFound();

                if (message.Username != account.Username || !account.Admin) return Unauthorized();

                message.Delete();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}