using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendGameActionFightDeathMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameActionFightDeathMessage(
                            (short) ActionsEnum.ACTION_CHARACTER_DEATH,
                            fighter.Id, fighter.Id
                            ));
        }

        public static void SendGameActionFightPointsVariationMessage(WorldClient client, ActionsEnum action,
                                                                     FightActor source,
                                                                     FightActor target, short delta)
        {
            client.Send(new GameActionFightPointsVariationMessage(
                            (short) action,
                            source.Id, target.Id, delta
                            ));
        }

        public static void SendGameActionFightLifePointsVariationMessage(WorldClient client, FightActor source,
                                                                         FightActor target, short delta)
        {
            client.Send(new GameActionFightLifePointsVariationMessage(
                            (short) ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST,
                            source.Id, target.Id, delta
                            ));
        }
    }
}