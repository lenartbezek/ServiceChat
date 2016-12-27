using System.Web.Http;

namespace ServiceChat
{
    [RoutePrefix("Service1.svc/Messages")]
    public class MessagesController : ApiController
    {
        // GET Service1.svc/Messages
        public object Get()
        {
            try
            {
                var account = Account.Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Message.GetAll());
            }
            catch
            {
                return InternalServerError();
            }
        }

        // Returns messages that arrived after the message at given ID
        // GET Service1.svc/Messages/5
        public object Get(int id)
        {
            try
            {
                var account = Account.Authenticate();
                if (account == null) return Unauthorized();

                return Ok(Message.GetSince(id));
            }
            catch (Message.IdNotFoundException)
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