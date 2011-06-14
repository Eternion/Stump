
using System.Collections.Generic;
using System.Linq;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class StartupHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (StartupActionsExecuteMessage))]
        public static void HandleStartupActionsListRequestMessage(WorldClient client,
                                                                  StartupActionsExecuteMessage message)
        {
            SendStartupActionsListMessage(client);
        }

        [WorldHandler(typeof (StartupActionsObjetAttributionMessage))]
        public static void HandleStartupActionsObjetAttributionMessage(WorldClient client,
                                                                       StartupActionsObjetAttributionMessage message)
        {
            if (message.characterId != 0)
            {
                // TODO Ajout de l'item au personnage

                CharacterRecord character = client.Characters.FirstOrDefault(c => c.Id == message.characterId);


                SendStartupActionFinishedMessage(client);
            }
        }

        public static void SendStartupActionsListMessage(WorldClient client)
        {
            IEnumerable<StartupActionAddObject> startupsActions =
                client.Account.StartupActions.Select(
                    s => new StartupActionAddObject(s.Id, s.Title, s.Text, s.DescUrl, s.PictureUrl, s.Items.Select(
                        i => new ObjectItemMinimalInformation(i.ItemTemplate, 0, false, new List<ObjectEffect>())).ToList()));

            client.Send(new StartupActionsListMessage(startupsActions.ToList()));
        }

        public static void SendStartupActionFinishedMessage(WorldClient client)
        {
            client.Send(new StartupActionFinishedMessage());
        }
    }
}