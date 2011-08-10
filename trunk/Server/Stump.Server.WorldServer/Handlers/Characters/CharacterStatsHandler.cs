using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.Stats;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        
        public static void SendLifePointsRegenBeginMessage(WorldClient client, byte regenRate)
        {
            client.Send(new LifePointsRegenBeginMessage(regenRate));
        }

        public static void SendUpdateLifePointsMessage(WorldClient client, byte lifePoint)
        {
            client.Send(new UpdateLifePointsMessage(lifePoint, ((StatsHealth)client.ActiveCharacter.Stats[CaracteristicsEnum.Health]).TotalMax));
        }

        public static void SendLifePointsRegenEndMessage(WorldClient client)
        {
            client.Send(new LifePointsRegenEndMessage(
                client.ActiveCharacter.Stats[CaracteristicsEnum.Health].Total,
                ((StatsHealth)client.ActiveCharacter.Stats[CaracteristicsEnum.Health]).TotalMax,
                0));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(
                new CharacterStatsListMessage(
                    new CharacterCharacteristicsInformations(
                        client.ActiveCharacter.Experience, // EXPERIENCE
                        client.ActiveCharacter.LowerBoundExperience, // EXPERIENCE level floor 
                        client.ActiveCharacter.UpperBoundExperience, // EXPERIENCE nextlevel floor 

                        0, // Amount of kamas.

                        client.ActiveCharacter.StatsPoints, // Stats points
                        client.ActiveCharacter.SpellsPoints, // Spell points

                        // Alignment
                        client.ActiveCharacter.GetActorAlignmentExtendInformations(),
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Health].Total, // Life points
                        ((StatsHealth)client.ActiveCharacter.Stats[CaracteristicsEnum.Health]).TotalMax, // Max Life points

                        client.ActiveCharacter.Energy, // Energy points
                        client.ActiveCharacter.EnergyMax, // maxEnergyPoints

                        (short)client.ActiveCharacter.Stats[CaracteristicsEnum.AP]
                                    .Total, // actionPointsCurrent
                        (short)client.ActiveCharacter.Stats[CaracteristicsEnum.MP]
                                    .Total, // movementPointsCurrent

                        client.ActiveCharacter.Stats[CaracteristicsEnum.Initiative],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Prospecting],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.AP],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.MP],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Strength],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Vitality],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Wisdom],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Chance],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Agility],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Intelligence],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.Range],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.SummonLimit],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.DamageReflection],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.CriticalHit],
                        0, //client.ActiveCharacter.Inventory.WeaponCriticalHit,
                        client.ActiveCharacter.Stats[CaracteristicsEnum.CriticalMiss],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.HealBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.DamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.WeaponDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.DamageBonusPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.TrapBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.TrapBonusPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PermanentDamagePercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.TackleBlock],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.TackleEvade],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.APAttack],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.MPAttack],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PushDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.CriticalDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.NeutralDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.EarthDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.WaterDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.AirDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.FireDamageBonus],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.DodgeAPProbability],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.DodgeMPProbability],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.NeutralResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.EarthResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.WaterResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.AirResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.FireResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.NeutralElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.EarthElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.WaterElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.AirElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.FireElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PushDamageReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.CriticalDamageReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpNeutralResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpEarthResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpWaterResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpAirResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpFireResistPercent],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpNeutralElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpEarthElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpWaterElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpAirElementReduction],
                        client.ActiveCharacter.Stats[CaracteristicsEnum.PvpFireElementReduction],
                        new List<CharacterSpellModification>()
                        )));
        }

        public static void SendCharacterLevelUpMessage(WorldClient client, byte level)
        {
            client.Send(new CharacterLevelUpMessage(level));
        }

        public static void SendCharacterLevelUpInformationMessage(WorldClient client, Character character, byte level)
        {
            client.Send(new CharacterLevelUpInformationMessage(level, character.Name, character.Id, 0));
        }
    }
}