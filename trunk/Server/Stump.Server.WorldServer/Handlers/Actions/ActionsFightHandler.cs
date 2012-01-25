using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendGameActionFightDeathMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameActionFightDeathMessage(
                            (short) ActionsEnum.ACTION_CHARACTER_DEATH,
                            fighter.Id, fighter.Id
                            ));
        }

        public static void SendGameActionFightPointsVariationMessage(IPacketReceiver client, ActionsEnum action,
                                                                     FightActor source,
                                                                     FightActor target, short delta)
        {
            client.Send(new GameActionFightPointsVariationMessage(
                            (short) action,
                            source.Id, target.Id, delta
                            ));
        }

        public static void SendGameActionFightLifePointsVariationMessage(IPacketReceiver client, FightActor source,
                                                                         FightActor target, short delta)
        {
            client.Send(new GameActionFightLifePointsVariationMessage(
                            (short) ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST,
                            source.Id, target.Id, delta
                            ));
        }

        public static void SendGameActionFightTackledMessage(IPacketReceiver client, FightActor source, IEnumerable<FightActor> tacklers)
        {
            client.Send(new GameActionFightTackledMessage((short)ActionsEnum.ACTION_CHARACTER_ACTION_TACKLED, source.Id, tacklers.Select(entry => entry.Id)));
        }

        public static void SendGameActionFightLifePointsLostMessage(IPacketReceiver client, FightActor source,
                                                                         FightActor target, short loss, short permanentDamages)
        {
            client.Send(new GameActionFightLifePointsLostMessage((short)ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST, source.Id, target.Id, loss, permanentDamages));
        }


        public static void SendGameActionFightDodgePointLossMessage(IPacketReceiver client, ActionsEnum action, FightActor source, FightActor target, short amount)
        {
            client.Send(new GameActionFightDodgePointLossMessage((short)action, source.Id, target.Id, amount));
        }


        public static void SendGameActionFightReduceDamagesMessage(IPacketReceiver client, FightActor source, FightActor target, int amount)
        {
            client.Send(new GameActionFightReduceDamagesMessage(105, source.Id, target.Id, amount));
        }

        public static void SendGameActionFightReflectSpellMessage(IPacketReceiver client, FightActor source, FightActor target)
        {
            client.Send(new GameActionFightReflectSpellMessage((short)ActionsEnum.ACTION_CHARACTER_SPELL_REFLECTOR, source.Id, target.Id));
        }

        public static void SendGameActionFightTeleportOnSameMapMessage(IPacketReceiver client, FightActor source, FightActor target, Cell destination)
        {
            client.Send(new GameActionFightTeleportOnSameMapMessage((short)ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP, source.Id, target.Id, destination.Id));
        }

        public static void SendGameActionFightSlideMessage(IPacketReceiver client, FightActor source, FightActor target, short startCellId, short endCellId)
        {
            client.Send(new GameActionFightSlideMessage((short)ActionsEnum.ACTION_CHARACTER_PUSH, source.Id, target.Id, startCellId, endCellId));
        }
    }
}