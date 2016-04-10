using Stump.Server.WorldServer.Game;
using System.Linq;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [Route("Character/{characterId:int}/Inventory")]
    public class InventoryController : ApiController
    {
        public IHttpActionResult Get(int characterId)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return NotFound();

            return Json(character.Inventory.GetItems().Select(x => x.GetObjectItem()));
        }

        [Route("Character/{characterId:int}/Inventory/{guid:int}")]
        public IHttpActionResult Get(int characterId, int guid)
        {
            return Ok($"value: {guid}");
        }

        public IHttpActionResult Post(int characterId, string value)
        {
            return Unauthorized();
        }

        public IHttpActionResult Put(int characterId, int guid, string value)
        {
            return Unauthorized();
        }

        public IHttpActionResult Delete(int characterId, int guid)
        {
            return Unauthorized();
        }
    }
}
