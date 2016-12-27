using System.Web.Http;

namespace ServiceChat
{
    [RoutePrefix("Service1.svc/Send")]
    public class SendController : ApiController
    {
        // POST Service1.svc/Send
        public object Post([FromBody]dynamic data)
        {
            try
            {
                var account = Account.Authenticate();
                if (account == null) return Unauthorized();

                return data.GetType().GetProperty("text") != null
                    ? Ok(Message.Create(account.Username, data.text))
                    : BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}