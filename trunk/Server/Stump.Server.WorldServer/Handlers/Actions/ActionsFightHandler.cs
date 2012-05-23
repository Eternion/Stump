using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;

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

        public static void SendGameActionFightSummonMessage(IPacketReceiver client, SummonedMonster summonedMonster)
        {
            client.Send(new GameActionFightSummonMessage((short)ActionsEnum.ACTION_SUMMON_CREATURE, summonedMonster.Summoner.Id, summonedMonster.GetGameFightFighterInformations()));
        }

        public static void SendGameActionFightInvisibilityMessage(IPacketReceiver client, FightActor source, FightActor target, GameActionFightInvisibilityStateEnum state)
        {
            client.Send(new GameActionFightInvisibilityMessage((short)ActionsEnum.ACTION_CHARACTER_MAKE_INVISIBLE, source.Id, target.Id, (sbyte)state));
        }
        
        public static void SendGameActionFightDispellEffectMessage(IPacketReceiver client, FightActor source, FightActor target, Buff buff)
        {
            client.Send(new GameActionFightDispellEffectMessage(514, source.Id, target.Id, buff.Id));
        }


        public static void SendGameActionFightReflectDamagesMessage(IPacketReceiver client, FightActor source, FightActor target, int amount)
        {
            client.Send(new GameActionFightReflectDamagesMessage((short)ActionsEnum.ACTION_CHARACTER_LIFE_LOST_REFLECTOR, source.Id, target.Id, amount));
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

        public static void SendGameActionFightCloseCombatMessage(IPacketReceiver client, FightActor source, Cell cell, FightSpellCastCriticalEnum castCritical, bool silentCast, WeaponTemplate weapon)
        {
            var action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT;
            if (castCritical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT_CRITICAL_MISS;
            else if (castCritical == FightSpellCastCriticalEnum.CRITICAL_HIT)
                action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT_CRITICAL_HIT;

            client.Send(new GameActionFightCloseCombatMessage((short)action, source.Id, cell.Id, (sbyte)castCritical, silentCast, weapon.Id));
        }

        public static void SendGameActionFightChangeLookMessage(IPacketReceiver client, FightActor source, FightActor target, EntityLook look)
        {
            client.Send(new GameActionFightChangeLookMessage((short)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK, source.Id, target.Id, look));
        }

        public static void SendGameActionFightExchangePositionsMessage(IPacketReceiver client, FightActor caster, FightActor target)
        {
            client.Send(new GameActionFightExchangePositionsMessage((short)ActionsEnum.ACTION_CHARACTER_EXCHANGE_PLACES, caster.Id, target.Id, caster.Cell.Id, target.Cell.Id));
        }
    }
}