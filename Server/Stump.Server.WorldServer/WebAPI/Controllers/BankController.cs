using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [Route("Account/{accountId:int}/Bank")]
    public class BankController : ApiController
    {
        public IHttpActionResult Get(int accountId)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Json(character.Bank.Select(x => x.GetObjectItem()));
        }

        [Route("Account/{accountId:int}/Bank/{guid:int}")]
        public IHttpActionResult Get(int accountId, int guid)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var item = character.Bank.TryGetItem(guid);

            if (item == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Json(item.GetObjectItem());
        }

        public IHttpActionResult Post(int accountId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        [Route("Account/{accountId:int}/Bank/{itemId:int}/{amount:int}")]
        public IHttpActionResult Put(int accountId, int itemId, int amount)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var item = ItemManager.Instance.CreateBankItem(character, itemId, amount);

            if (item == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var playerItem = character.Bank.AddItem(item);

            if (playerItem == null)
                return StatusCode(HttpStatusCode.BadRequest);

            //TEXT_INFORMATION_POPUP		21		Des objets ont été déposés dans votre banque.

            return Ok();
        }

        [Route("Account/{accountId:int}/Bank/{guid:int}/{amount:int}")]
        public IHttpActionResult Delete(int accountId, int guid, int amount)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var item = character.Bank.TryGetItem(guid);

            if (item == null)
                return StatusCode(HttpStatusCode.BadRequest);

            character.Bank.UnStackItem(item, amount);

            return Ok();
        }
    }
}
