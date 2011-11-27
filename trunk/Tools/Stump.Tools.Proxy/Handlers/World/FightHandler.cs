using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class FightHandler : WorldHandlerContainer
    {
        [WorldHandler(GameFightJoinMessage.Id)]
        public static void HandleGameFightJoinMessage(WorldClient client, GameFightJoinMessage message)
        {
            client.Send(message);
            client.Fight = message;
        }

        [WorldHandler(GameFightShowFighterMessage.Id)]
        public static void HandleGameFightShowFighterMessage(WorldClient client, GameFightShowFighterMessage message)
        {
            client.Send(message);
            client.Actors.Add(message.informations.contextualId, message.informations);
        }

        [WorldHandler(GameActionFightSpellCastMessage.Id)]
        public static void HandleGameActionFightSpellCastMessage(WorldClient client, GameActionFightSpellCastMessage message)
        {
            client.Send(message);
            if (!client.IsInFight)
                return;

            if (!client.Actors.ContainsKey(message.sourceId))
                return;

            var caster = client.Actors[message.sourceId] as GameFightMonsterInformations;

            if (caster == null)
                return;

            DataFactory.BuildMonsterSpell(client, caster, message);
        }

        [WorldHandler(GameContextCreateMessage.Id)]
        public static void HandleGameContextCreateMessage(WorldClient client, GameContextCreateMessage message)
        {
            client.Send(message);
            client.Actors.Clear();
            if (message.context == 2)
                client.IsInFight = true;
            else
                client.IsInFight = false;

            client.GuessCellTrigger = null;
        }

        [WorldHandler(GameFightPlacementPossiblePositionsMessage.Id)]
        public static void HandleGameFightPlacementPossiblePositionsMessage(WorldClient client, GameFightPlacementPossiblePositionsMessage message)
        {
            client.Send(message);
            if (!client.IsInFight) 
                return;

            DataFactory.BuildMapFightPlacement(client, client.CurrentMap, message.positionsForDefenders, message.positionsForChallengers);
        }
    }
}