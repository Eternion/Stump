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
                client.Character.Stats.Health.Total,
                client.Character.Stats.Health.TotalMax));
        }

        public static void SendLifePointsRegenEndMessage(WorldClient client, int recoveredLife)
        {
            client.Send(new LifePointsRegenEndMessage(
                client.Character.Stats.Health.Total,
                client.Character.Stats.Health.TotalMax,
                recoveredLife));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(
                new CharacterStatsListMessage(
                    new CharacterCharacteristicsInformations(
                        client.Character.Experience, // EXPERIENCE
                        client.Character.LowerBoundExperience, // EXPERIENCE level floor 
                        client.Character.UpperBoundExperience, // EXPERIENCE nextlevel floor 

                        client.Character.Kamas, // Amount of kamas.

                        client.Character.StatsPoints, // Stats points
                        client.Character.SpellsPoints, // Spell points

                        // Alignment
                        client.Character.GetActorAlignmentExtendInformations(),
                        client.Character.Stats.Health.Total, // Life points
                        client.Character.Stats.Health.TotalMax, // Max Life points

                        client.Character.Energy, // Energy points
                        client.Character.EnergyMax, // maxEnergyPoints

                        (short)client.Character.Stats[PlayerFields.AP]
                                    .Total, // actionPointsCurrent
                        (short)client.Character.Stats[PlayerFields.MP]
                                    .Total, // movementPointsCurrent

                        client.Character.Stats[PlayerFields.Initiative],
                        client.Character.Stats[PlayerFields.Prospecting],
                        client.Character.Stats[PlayerFields.AP],
                        client.Character.Stats[PlayerFields.MP],
                        client.Character.Stats[PlayerFields.Strength],
                        client.Character.Stats[PlayerFields.Vitality],
                        client.Character.Stats[PlayerFields.Wisdom],
                        client.Character.Stats[PlayerFields.Chance],
                        client.Character.Stats[PlayerFields.Agility],
                        client.Character.Stats[PlayerFields.Intelligence],
                        client.Character.Stats[PlayerFields.Range],
                        client.Character.Stats[PlayerFields.SummonLimit],
                        client.Character.Stats[PlayerFields.DamageReflection],
                        client.Character.Stats[PlayerFields.CriticalHit],
                        (short) client.Character.Inventory.WeaponCriticalHit,
                        client.Character.Stats[PlayerFields.CriticalMiss],
                        client.Character.Stats[PlayerFields.HealBonus],
                        client.Character.Stats[PlayerFields.DamageBonus],
                        client.Character.Stats[PlayerFields.WeaponDamageBonus],
                        client.Character.Stats[PlayerFields.DamageBonusPercent],
                        client.Character.Stats[PlayerFields.TrapBonus],
                        client.Character.Stats[PlayerFields.TrapBonusPercent],
                        client.Character.Stats[PlayerFields.PermanentDamagePercent],
                        client.Character.Stats[PlayerFields.TackleBlock],
                        client.Character.Stats[PlayerFields.TackleEvade],
                        client.Character.Stats[PlayerFields.APAttack],
                        client.Character.Stats[PlayerFields.MPAttack],
                        client.Character.Stats[PlayerFields.PushDamageBonus],
                        client.Character.Stats[PlayerFields.CriticalDamageBonus],
                        client.Character.Stats[PlayerFields.NeutralDamageBonus],
                        client.Character.Stats[PlayerFields.EarthDamageBonus],
                        client.Character.Stats[PlayerFields.WaterDamageBonus],
                        client.Character.Stats[PlayerFields.AirDamageBonus],
                        client.Character.Stats[PlayerFields.FireDamageBonus],
                        client.Character.Stats[PlayerFields.DodgeAPProbability],
                        client.Character.Stats[PlayerFields.DodgeMPProbability],
                        client.Character.Stats[PlayerFields.NeutralResistPercent],
                        client.Character.Stats[PlayerFields.EarthResistPercent],
                        client.Character.Stats[PlayerFields.WaterResistPercent],
                        client.Character.Stats[PlayerFields.AirResistPercent],
                        client.Character.Stats[PlayerFields.FireResistPercent],
                        client.Character.Stats[PlayerFields.NeutralElementReduction],
                        client.Character.Stats[PlayerFields.EarthElementReduction],
                        client.Character.Stats[PlayerFields.WaterElementReduction],
                        client.Character.Stats[PlayerFields.AirElementReduction],
                        client.Character.Stats[PlayerFields.FireElementReduction],
                        client.Character.Stats[PlayerFields.PushDamageReduction],
                        client.Character.Stats[PlayerFields.CriticalDamageReduction],
                        client.Character.Stats[PlayerFields.PvpNeutralResistPercent],
                        client.Character.Stats[PlayerFields.PvpEarthResistPercent],
                        client.Character.Stats[PlayerFields.PvpWaterResistPercent],
                        client.Character.Stats[PlayerFields.PvpAirResistPercent],
                        client.Character.Stats[PlayerFields.PvpFireResistPercent],
                        client.Character.Stats[PlayerFields.PvpNeutralElementReduction],
                        client.Character.Stats[PlayerFields.PvpEarthElementReduction],
                        client.Character.Stats[PlayerFields.PvpWaterElementReduction],
                        client.Character.Stats[PlayerFields.PvpAirElementReduction],
                        client.Character.Stats[PlayerFields.PvpFireElementReduction],
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