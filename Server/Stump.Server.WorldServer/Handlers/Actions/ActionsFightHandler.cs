using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendGameActionFightDeathMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameActionFightDeathMessage(
                            (short) ActionsEnum.ACTION_CHARACTER_DEATH,
                            fighter.Id, fighter.Id));
        }

        public static void SendGameActionFightVanishMessage(IPacketReceiver client, FightActor source, FightActor target)
        {
            client.Send(new GameActionFightVanishMessage((short)EffectsEnum.Effect_Vanish, source.Id, target.Id));
        }
         
        public static void SendGameActionFightSummonMessage(IPacketReceiver client, SummonedFighter summon)
        {
            var fighterInfos = summon.GetGameFightFighterInformations();
            var action = summon.CanPlay() ? (short)ActionsEnum.ACTION_SUMMON_CREATURE : (short)ActionsEnum.ACTION_SUMMON_STATIC_CREATURE;

            if (summon is SummonedClone)
            {
                action = summon is SummonedImage ? (short)EffectsEnum.Effect_Illusions : (short)ActionsEnum.ACTION_CHARACTER_ADD_DOUBLE;
            }

            if (summon.IsControlled())
            {
                action = (short)ActionsEnum.ACTION_SUMMON_SLAVE;
            }

            client.Send(new GameActionFightSummonMessage(action, summon.Summoner.Id, new[] { fighterInfos }));
        }

        public static void SendGameActionFightReviveMessage(IPacketReceiver client, FightActor caster, FightActor actor)
        {
            client.Send(new GameActionFightSummonMessage((short)ActionsEnum.ACTION_CHARACTER_SUMMON_DEAD_ALLY_IN_FIGHT, caster.Id, new[] { actor.GetGameFightFighterInformations() }));
        }

        public static void SendGameActionFightSummonMessage(IPacketReceiver client, SummonedBomb summon)
        {
            client.Send(new GameActionFightSummonMessage((short)ActionsEnum.ACTION_SUMMON_BOMB, summon.Summoner.Id, new[] { summon.GetGameFightFighterInformations() }));
        }

        public static void SendGameActionFightInvisibilityMessage(IPacketReceiver client, FightActor source, FightActor target, GameActionFightInvisibilityStateEnum state)
        {
            client.Send(new GameActionFightInvisibilityMessage((short)ActionsEnum.ACTION_CHARACTER_MAKE_INVISIBLE, source.Id, target.Id, (sbyte)state));
        }

        public static void SendGameActionFightInvisibleDetectedMessage(IPacketReceiver client, FightActor source, FightActor target)
        {
            client.Send(new GameActionFightInvisibleDetectedMessage((short)ActionsEnum.ACTION_CHARACTER_MAKE_INVISIBLE, source.Id, target.Id, target.Cell.Id));
        }

        public static void SendGameActionFightDispellSpellMessage(IPacketReceiver client, FightActor source, FightActor target, int spellId)
        {
            client.Send(new GameActionFightDispellSpellMessage(406, source.Id, target.Id, (short)spellId));
        }

        public static void SendGameActionFightDispellEffectMessage(IPacketReceiver client, FightActor source, FightActor target, Buff buff)
        {
            client.Send(new GameActionFightDispellEffectMessage((short)ActionsEnum.ACTION_CHARACTER_BOOST_DISPELLED, source.Id, target.Id, buff.Id));
        }

        public static void SendGameActionFightReflectDamagesMessage(IPacketReceiver client, FightActor source, FightActor target)
        {
            client.Send(new GameActionFightReflectDamagesMessage((short)ActionsEnum.ACTION_CHARACTER_LIFE_LOST_REFLECTOR, source.Id, target.Id));
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
            client.Send(new GameActionFightTackledMessage((short)ActionsEnum.ACTION_CHARACTER_ACTION_TACKLED, source.Id, tacklers.Select(entry => (double)entry.Id)));
        }

        public static void SendGameActionFightLifePointsLostMessage(IPacketReceiver client, ActionsEnum action, FightActor source, FightActor target, short loss, short permanentDamages)
        {
            client.Send(new GameActionFightLifePointsLostMessage((short)action, source.Id, target.Id, loss, permanentDamages));
        }

        public static void SendGameActionFightLifeAndShieldPointsLostMessage(IPacketReceiver client, ActionsEnum action, FightActor source, FightActor target, short loss, short permanentDamages, short shieldLoss)
        {
            client.Send(new GameActionFightLifeAndShieldPointsLostMessage((short)action, source.Id, target.Id, loss, permanentDamages, shieldLoss));
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

        public static void SendGameActionFightCloseCombatMessage(IPacketReceiver client, FightActor source, FightActor target, Cell cell, FightSpellCastCriticalEnum castCritical, bool silentCast, WeaponTemplate weapon)
        {
            SendGameActionFightCloseCombatMessage(client, source, target, cell, castCritical, silentCast, (short)weapon.Id);
        }

        public static void SendGameActionFightCloseCombatMessage(IPacketReceiver client, FightActor source, FightActor target, Cell cell, FightSpellCastCriticalEnum castCritical, bool silentCast, short weaponId)
        {
            var action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT;
            switch (castCritical)
            {
                case FightSpellCastCriticalEnum.CRITICAL_FAIL:
                    action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT_CRITICAL_MISS;
                    break;
                case FightSpellCastCriticalEnum.CRITICAL_HIT:
                    action = ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT_CRITICAL_HIT;
                    break;
            }

            client.Send(new GameActionFightCloseCombatMessage((short)action, source.Id, silentCast, true, target?.Id ?? 0, cell.Id, (sbyte)castCritical, weaponId));
        }

        public static void SendGameActionFightChangeLookMessage(IPacketReceiver client, FightActor source, FightActor target, ActorLook look)
        {
            client.Send(new GameActionFightChangeLookMessage((short)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK, source.Id, target.Id, look.GetEntityLook()));
        }

        public static void SendGameActionFightSpellCooldownVariationMessage(IPacketReceiver client, FightActor source, FightActor target, Spell spell, short duration)
        {
            client.Send(new GameActionFightSpellCooldownVariationMessage(duration > 0 ? (short)ActionsEnum.ACTION_CHARACTER_ADD_SPELL_COOLDOWN : (short)ActionsEnum.ACTION_CHARACTER_REMOVE_SPELL_COOLDOWN, source.Id, target.Id, (short)spell.Id, duration));
        }

        public static void SendGameActionFightExchangePositionsMessage(IPacketReceiver client, FightActor caster, FightActor target)
        {
            client.Send(new GameActionFightExchangePositionsMessage((short)ActionsEnum.ACTION_CHARACTER_EXCHANGE_PLACES, caster.Id, target.Id, caster.Cell.Id, target.Cell.Id));
        }

        public static void SendGameActionFightCarryCharacterMessage(IPacketReceiver client, FightActor caster, FightActor target)
        {
            client.Send(new GameActionFightCarryCharacterMessage((short)ActionsEnum.ACTION_CARRY_CHARACTER, caster.Id, target.Id, target.Cell.Id));
        }

        public static void SendGameActionFightThrowCharacterMessage(IPacketReceiver client, FightActor caster, FightActor target, Cell cell)
        {
            client.Send(new GameActionFightThrowCharacterMessage((short)ActionsEnum.ACTION_THROW_CARRIED_CHARACTER, caster.Id, target.Id, cell.Id));
        }

        public static void SendGameActionFightDropCharacterMessage(IPacketReceiver client, FightActor caster, FightActor target, Cell cell)
        {
            client.Send(new GameActionFightDropCharacterMessage((short)ActionsEnum.ACTION_NO_MORE_CARRIED, caster.Id, target.Id, cell.Id));
        }
    }
}