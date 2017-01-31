using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using System;
using System.Linq;
using System.Net;
using System.Threading;
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
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }

                var character = World.Instance.GetCharacter(x => x.Account.Id == accountId);

                if (character == null)
                {
                    result = NotFound();
                    resetEvent.Set();
                    return;
                }

                var item = ItemManager.Instance.CreateBankItem(character, itemId, amount, maxStats);

                if (item == null)
                {
                    result = StatusCode(HttpStatusCode.InternalServerError);
                    resetEvent.Set();
                    return;
                }

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
                {
                    result = StatusCode(HttpStatusCode.InternalServerError);
                    resetEvent.Set();
                    return;
                }

                //Des objets ont été déposés dans votre banque.
                character.SendSystemMessage(21, true);

                result = Ok();
                resetEvent.Set();
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;
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