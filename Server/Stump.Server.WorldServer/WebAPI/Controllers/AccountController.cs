using MongoDB.Bson;
using Stump.Server.BaseServer.Logging;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Accounts;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Account/{accountId:int}")]
    public class AccountController : ApiController
    {
        public IHttpActionResult Get(int accountId)
        {
            var account = World.Instance.GetConnectedAccount(accountId);

            if (account == null)
                return NotFound();

            return Json(account);
        }

        [HttpPut]
        [Route("Account/{accountId:int}/AddTokens/{amount:int}")]
        public IHttpActionResult AddTokens(int accountId, int amount)
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

                var account = ClientManager.Instance.Clients.OfType<WorldClient>().FirstOrDefault(x => x.Account?.Id == accountId);

                if (account == null)
                {
                    var worldAccount = AccountManager.Instance.FindById(accountId);

                    if (worldAccount == null)
                    {
                        result = NotFound();
                        resetEvent.Set();
                        return;
                    }

                    worldAccount.Tokens += amount;
                    WorldServer.Instance.DBAccessor.Database.Save(worldAccount);

                    var documentAccount = new BsonDocument
                    {
                        {"AcctId", accountId},
                        {"AcctName", ""},
                        {"CharacterId", 0},
                        {"CharacterName", ""},
                        {"Amount", amount},
                        {"Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)}
                    };

                    MongoLogger.Instance.Insert("Transactions", documentAccount);

                    result = Ok();
                    resetEvent.Set();
                    return;
                }

                if (account.Character == null)
                {
                    account.WorldAccount.Tokens += amount;
                    WorldServer.Instance.DBAccessor.Database.Save(account.WorldAccount);

                    var documentAccount = new BsonDocument
                    {
                        {"AcctId", accountId},
                        {"AcctName", account.Account.Login},
                        {"CharacterId", 0},
                        {"CharacterName", ""},
                        {"Amount", amount},
                        {"Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)}
                    };

                    MongoLogger.Instance.Insert("Transactions", documentAccount);

                    result = Ok();
                    resetEvent.Set();
                    return;
                }

                var tokens = account.Character.Inventory.Tokens;

                if (tokens != null)
                {
                    tokens.Stack += (uint)amount;
                    account.Character.Inventory.RefreshItem(tokens);
                }
                else
                {
                    account.Character.Inventory.CreateTokenItem(amount);
                    account.Character.Inventory.RefreshItem(account.Character.Inventory.Tokens);
                }

                var document = new BsonDocument
                {
                    {"AcctId", accountId},
                    {"AcctName", account.Character.Account.Login},
                    {"CharacterId", account.Character.Id},
                    {"CharacterName", account.Character.Name},
                    {"Amount", amount},
                    {"Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)}
                };

                MongoLogger.Instance.Insert("Transactions", document);

                account.Character.SendServerMessage($"Vous venez de recevoir votre achat de {amount} Ogrines !");
                account.Character.SaveLater();

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

        public IHttpActionResult Put(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Post(int accountId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Delete(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);
    }
}