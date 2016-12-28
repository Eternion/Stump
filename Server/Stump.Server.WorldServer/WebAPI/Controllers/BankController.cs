using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Account/{accountId:int}/Bank")]
    public class BankController : ApiController
    {
        public IHttpActionResult Get(int accountId)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return NotFound();

            return Json(character.Bank.Select(x => x.GetObjectItem()));
        }

        [Route("Account/{accountId:int}/Bank/{guid:int}")]
        public IHttpActionResult Get(int accountId, int guid)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return NotFound();

            var item = character.Bank.TryGetItem(guid);

            if (item == null)
                return NotFound();

            return Json(item.GetObjectItem());
        }

        public IHttpActionResult Post(int accountId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        [Route("Account/{accountId:int}/Bank/{itemId:int}/{amount:int}/{maxStats:bool?}")]
        public IHttpActionResult Put(int accountId, int itemId, int amount, bool maxStats = false)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return NotFound();

            var item = ItemManager.Instance.CreateBankItem(character, itemId, amount, maxStats);

            if (item == null)
                return StatusCode(HttpStatusCode.InternalServerError);

            if (item.Template.Id == (int)ItemIdEnum.TokenScroll)
            {
                if (!item.Effects.Any(x => x.EffectId == EffectsEnum.Effect_AddOgrines))
                {
                    item.Effects.Add(new EffectInteger(EffectsEnum.Effect_AddOgrines, (short)amount));
                    item.Stack = 1;
                }
            }
            else if (!item.Effects.Any(x => x.EffectId == EffectsEnum.Effect_NonExchangeable_982))
            {
                item.Effects.Add(new EffectInteger(EffectsEnum.Effect_NonExchangeable_982, 0));
            }

            var playerItem = character.Bank.AddItem(item);

            if (playerItem == null)
                return StatusCode(HttpStatusCode.InternalServerError);

            //Des objets ont été déposés dans votre banque.
            character.SendSystemMessage(21, true);

            return Ok();
        }

        [Route("Account/{accountId:int}/Bank/{guid:int}/{amount:int}")]
        public IHttpActionResult Delete(int accountId, int guid, int amount)
        {
            var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

            if (character == null)
                return NotFound();

            var item = character.Bank.TryGetItem(guid);

            if (item == null)
                return NotFound();

            character.Bank.UnStackItem(item, amount);

            return Ok();
        }
    }
}