
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendGameActionFightDeathMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameActionFightDeathMessage(
                            (uint) ActionsEnum.ACTION_CHARACTER_DEATH,
                            (int) entity.Id, (int) entity.Id
                            ));
        }

        public static void SendGameActionFightPointsVariationMessage(WorldClient client, ActionsEnum action,
                                                                     Entity source,
                                                                     Entity target, short delta)
        {
            client.Send(new GameActionFightPointsVariationMessage(
                            (uint) action,
                            (int) source.Id, (int) target.Id, delta
                            ));
        }

        public static void SendGameActionFightLifePointsVariationMessage(WorldClient client, Entity source,
                                                                         Entity target, short delta)
        {
            client.Send(new GameActionFightLifePointsVariationMessage(
                            (uint) ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST,
                            (int) source.Id, (int) target.Id, delta
                            ));
        }
    }
}