using System.Web.Http;
using ServiceChat.Models;
using static ServiceChat.Authentication;

namespace ServiceChat.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        // Returns all messages
        public object Get()
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Message.GetAll());
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Returns messages that arrived after the message at given ID
        [Route("/{id}")]
        public object Get(int id)
        {
            try
            {
                var account = Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Message.GetSince(id));
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}