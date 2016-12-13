using System.Web.Http;

namespace ChatDB
{
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {
        // GET api/message
        public object Get()
        {
            return Ok(Message.GetAll());
        }

        // GET api/message/5
        public object Get(int id)
        {
            try
            {
                return Ok(Message.Get(id));
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

        // POST api/message
        public object Post([FromBody]dynamic data)
        {
            try
            {
                var account = Account.Authenticate();
                if (account == null) return Unauthorized();

                if (data.GetType().GetProperty("text") != null)
                    return Ok(Message.Create(account.Username, data.text));
                else
                    return BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // PUT api/message/5
        [Authorize]
        public object Put(int id, [FromBody]dynamic data)
        {
            return NotFound();
        }

        // DELETE api/message/5
        [Authorize]
        public object Delete(int id)
        {
            return NotFound();
        }
    }
}