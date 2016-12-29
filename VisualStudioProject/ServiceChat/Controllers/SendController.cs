using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/send")]
    public class SendController : ApiController
    {
        // Posts a new message
        public object Post([FromBody]dynamic data)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                if (data.GetType().GetProperty("Text") != null)
                    return BadRequest();

                var message = new Message(account, data.text);
                message.Create();
                return Ok(message);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Edits a message
        [Route("/{id}")]
        public object Put(int id, [FromBody]dynamic data)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                var message = Message.Get(id);
                if (message == null) return NotFound();

                if (message.Username != account.Username || !account.Admin) return Unauthorized();

                if (data.GetType().GetProperty("Text") != null)
                    return BadRequest();

                message.Edit(data.Text);
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