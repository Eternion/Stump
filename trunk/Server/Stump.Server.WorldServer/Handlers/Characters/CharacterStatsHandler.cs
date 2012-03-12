using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        
        public static void SendLifePointsRegenBeginMessage(IPacketReceiver client, byte regenRate)
        {
            client.Send(new LifePointsRegenBeginMessage(regenRate));
        }

        public static void SendUpdateLifePointsMessage(WorldClient client)
        {
            client.Send(new UpdateLifePointsMessage(
                client.ActiveCharacter.Stats.Health.Total,
                client.ActiveCharacter.Stats.Health.TotalMax));
        }

        public static void SendLifePointsRegenEndMessage(WorldClient client, int recoveredLife)
        {
            client.Send(new LifePointsRegenEndMessage(
                client.ActiveCharacter.Stats.Health.Total,
                client.ActiveCharacter.Stats.Health.TotalMax,
                recoveredLife));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(
                new CharacterStatsListMessage(
                    new CharacterCharacteristicsInformations(
                        client.ActiveCharacter.Experience, // EXPERIENCE
                        client.ActiveCharacter.LowerBoundExperience, // EXPERIENCE level floor 
                        client.ActiveCharacter.UpperBoundExperience, // EXPERIENCE nextlevel floor 

                        client.ActiveCharacter.Kamas, // Amount of kamas.

                        client.ActiveCharacter.StatsPoints, // Stats points
                        client.ActiveCharacter.SpellsPoints, // Spell points

                        // Alignment
                        client.ActiveCharacter.GetActorAlignmentExtendInformations(),
                        client.ActiveCharacter.Stats.Health.Total, // Life points
                        client.ActiveCharacter.Stats.Health.TotalMax, // Max Life points

                        client.ActiveCharacter.Energy, // Energy points
                        client.ActiveCharacter.EnergyMax, // maxEnergyPoints

                        (short)client.ActiveCharacter.Stats[PlayerFields.AP]
                                    .Total, // actionPointsCurrent
                        (short)client.ActiveCharacter.Stats[PlayerFields.MP]
                                    .Total, // movementPointsCurrent

                        client.ActiveCharacter.Stats[PlayerFields.Initiative],
                        client.ActiveCharacter.Stats[PlayerFields.Prospecting],
                        client.ActiveCharacter.Stats[PlayerFields.AP],
                        client.ActiveCharacter.Stats[PlayerFields.MP],
                        client.ActiveCharacter.Stats[PlayerFields.Strength],
                        client.ActiveCharacter.Stats[PlayerFields.Vitality],
                        client.ActiveCharacter.Stats[PlayerFields.Wisdom],
                        client.ActiveCharacter.Stats[PlayerFields.Chance],
                        client.ActiveCharacter.Stats[PlayerFields.Agility],
                        client.ActiveCharacter.Stats[PlayerFields.Intelligence],
                        client.ActiveCharacter.Stats[PlayerFields.Range],
                        client.ActiveCharacter.Stats[PlayerFields.SummonLimit],
                        client.ActiveCharacter.Stats[PlayerFields.DamageReflection],
                        client.ActiveCharacter.Stats[PlayerFields.CriticalHit],
                        (short) client.ActiveCharacter.Inventory.WeaponCriticalHit,
                        client.ActiveCharacter.Stats[PlayerFields.CriticalMiss],
                        client.ActiveCharacter.Stats[PlayerFields.HealBonus],
                        client.ActiveCharacter.Stats[PlayerFields.DamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.WeaponDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.DamageBonusPercent],
                        client.ActiveCharacter.Stats[PlayerFields.TrapBonus],
                        client.ActiveCharacter.Stats[PlayerFields.TrapBonusPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PermanentDamagePercent],
                        client.ActiveCharacter.Stats[PlayerFields.TackleBlock],
                        client.ActiveCharacter.Stats[PlayerFields.TackleEvade],
                        client.ActiveCharacter.Stats[PlayerFields.APAttack],
                        client.ActiveCharacter.Stats[PlayerFields.MPAttack],
                        client.ActiveCharacter.Stats[PlayerFields.PushDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.CriticalDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.NeutralDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.EarthDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.WaterDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.AirDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.FireDamageBonus],
                        client.ActiveCharacter.Stats[PlayerFields.DodgeAPProbability],
                        client.ActiveCharacter.Stats[PlayerFields.DodgeMPProbability],
                        client.ActiveCharacter.Stats[PlayerFields.NeutralResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.EarthResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.WaterResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.AirResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.FireResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.NeutralElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.EarthElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.WaterElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.AirElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.FireElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PushDamageReduction],
                        client.ActiveCharacter.Stats[PlayerFields.CriticalDamageReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PvpNeutralResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PvpEarthResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PvpWaterResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PvpAirResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PvpFireResistPercent],
                        client.ActiveCharacter.Stats[PlayerFields.PvpNeutralElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PvpEarthElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PvpWaterElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PvpAirElementReduction],
                        client.ActiveCharacter.Stats[PlayerFields.PvpFireElementReduction],
                        new List<CharacterSpellModification>()
                        )));
        }

        public static void SendCharacterLevelUpMessage(IPacketReceiver client, byte level)
        {
            client.Send(new CharacterLevelUpMessage(level));
        }


        public static void SendCharacterLevelUpInformationMessage(IPacketReceiver client, Character character, byte level)
        {
            client.Send(new CharacterLevelUpInformationMessage(level, character.Name, character.Id, 0));
        }
    }
}