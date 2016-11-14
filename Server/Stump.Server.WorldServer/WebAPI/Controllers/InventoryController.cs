using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
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
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return NotFound();

            var item = character.Inventory.TryGetItem(guid);

            if (item == null)
                return NotFound();

            return Json(item.GetObjectItem());
        }

        public IHttpActionResult Post(int characterId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        [Route("Character/{characterId:int}/Inventory/{itemId:int}/{amount:int}")]
        public IHttpActionResult Put(int characterId, int itemId, int amount)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return NotFound();

            var item = ItemManager.Instance.CreatePlayerItem(character, itemId, amount);

            if (item == null)
                return StatusCode(HttpStatusCode.InternalServerError);

            var playerItem = character.Inventory.AddItem(item);

            if (playerItem == null)
                return StatusCode(HttpStatusCode.InternalServerError);

            return Ok();
        }

        [Route("Character/{characterId:int}/Inventory/{guid:int}/{amount:int}")]
        public IHttpActionResult Delete(int characterId, int guid, int amount)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return NotFound();

            var item = character.Inventory.TryGetItem(guid);

            if (item == null)
                return NotFound();

            character.Inventory.UnStackItem(item, amount);

            return Ok();
        }
    }
}