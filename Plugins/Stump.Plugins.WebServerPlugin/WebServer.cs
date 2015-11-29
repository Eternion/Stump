using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Accounts;
using System.IO;
using System.Linq;
using System.Threading;

namespace Stump.Plugins.WebServerPlugin
{
    public class WebServer : HttpServer
    {
        [Variable(true, DefinableRunning = true)]
        public static string Token = "7MBWJNC3CF4L6QUE8HCWEMXB8B4XEBRC";

        public WebServer(int port)
            : base(port)
        {
        }

        [Initialization(InitializationPass.First)]
        public static void Initialize()
        {
            HttpServer httpServer = new WebServer(8080);
            var thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            var url = p.http_url.Substring(1);
            var urlBase = url.Split('&');

            if (urlBase.Length != 2)
            {
                p.outputStream.WriteLine("401 Unauthorized (" + url + ")");
                return;
            }

            var token = urlBase[1].Substring(6);

            if (token != Token)
            {
                p.outputStream.WriteLine("401 Unauthorized (" + url + ")");
                return;
            }

            var urlFull = urlBase[0].Split('/');

            if (urlFull.Length < 2)
            {
                p.outputStream.WriteLine("400 Bad request (" + url + ")");
                return;
            }

            var urlMethod = urlFull[0];
            var urlParam = urlFull[1];

            string response;

            switch (urlMethod)
            {
                case "kick":
                    {
                        int characterId;
                        if (!int.TryParse(urlParam, out characterId))
                        {
                            response = "400 Bad request";
                            break;
                        }

                        var character = World.Instance.GetCharacter(characterId);

                        if (character == null)
                        {
                            response = "Ko - Character not connected";
                            break;
                        }

                        character.Client.Disconnect();

                        response = "Ok";
                        break;
                    }
                case "addtokens":
                    {
                        if (urlFull.Length != 3)
                        {
                            response = "400 Bad request";
                            break;
                        }

                        int characterId;
                        if (!int.TryParse(urlParam, out characterId))
                        {
                            response = "400 Bad request";
                            break;
                        }

                        int amount;
                        if (!int.TryParse(urlFull[2], out amount))
                        {
                            response = "400 Bad request";
                            break;
                        }

                        var character = World.Instance.GetCharacter(characterId);

                        if (character == null)
                        {
                            response = "Ko - Character not connected";
                            break;
                        }

                        var tokens = character.Inventory.Tokens;

                        if (tokens != null)
                        {
                            tokens.Stack += (uint)amount;
                            character.Inventory.RefreshItem(tokens);
                        }
                        else
                        {
                            character.Inventory.CreateTokenItem((uint)amount);
                        }

                        WorldServer.Instance.IOTaskPool.AddMessage(() => character.Inventory.Save());
                        character.SendServerMessage(string.Format("Vous avez reçu {0} Jetons", amount));

                        response = "Ok";
                        break;
                    }
                case "deltokens":
                    {
                        if (urlFull.Length != 3)
                        {
                            response = "400 Bad request";
                            break;
                        }

                        int accountId;
                        if (!int.TryParse(urlParam, out accountId))
                        {
                            response = "400 Bad request";
                            break;
                        }

                        int amount;
                        if (!int.TryParse(urlFull[2], out amount))
                        {
                            response = "400 Bad request";
                            break;
                        }

                        var account = ClientManager.Instance.FindAll<WorldClient>(x => x.Account != null && x.Account.Id == accountId).FirstOrDefault();

                        if (account == null || account.WorldAccount == null)
                        {
                            response = "Ko - Account not connected";
                            break;
                        }

                        if (account.Character == null)
                        {
                            if (account.Account.Tokens < amount)
                            {
                                response = "Ko - Account doesn't have enough tokens";
                                break;
                            }

                            account.Account.Tokens -= (uint)amount;
                            IPCAccessor.Instance.SendRequest<CommonOKMessage>(new UpdateAccountMessage(account.Account), msg => { });
                        }
                        else
                        {
                            var character = account.Character;
                            var tokens = character.Inventory.Tokens;

                            if (tokens == null)
                            {
                                response = "Ko - Character doesn't have any tokens";
                                break;
                            }

                            if (tokens.Stack < amount)
                            {
                                response = "Ko - Character doesn't have enough tokens";
                                break;
                            }

                            tokens.Stack -= (uint)amount;
                            character.Inventory.RefreshItem(tokens);

                            WorldServer.Instance.IOTaskPool.AddMessage(() => character.Inventory.Save());
                            character.SendServerMessage(string.Format("Vous avez perdu {0} Jetons", amount));
                        }

                        response = "Ok";
                        break;
                    }
                default:
                    {
                        response = "404 Not Found";
                        break;
                    }
            }

            p.outputStream.WriteLine(response);
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            p.outputStream.WriteLine("POST Method is not implemented");
        }
    }
}
