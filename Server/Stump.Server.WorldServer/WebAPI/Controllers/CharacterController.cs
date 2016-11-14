using Stump.Server.WorldServer.Game;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Character/{characterId:int}")]
    public class CharacterController : ApiController
    {
        public IHttpActionResult Get(int characterId)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return NotFound();

            return Json(character.Record);
        }

        public IHttpActionResult Put(int characterId) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Post(int characterId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Delete(int characterId) => StatusCode(HttpStatusCode.MethodNotAllowed);
    }
}