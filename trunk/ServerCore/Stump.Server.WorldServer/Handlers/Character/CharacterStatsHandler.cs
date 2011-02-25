// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {

        public static void SendLifePointsRegenBeginMessage(WorldClient client, uint regenRate)
        {
            client.Send(new LifePointsRegenBeginMessage(regenRate));
        }

        public static void SendUpdateLifePointsMessage(WorldClient client, uint lifePoint)
        {
            client.Send(new UpdateLifePointsMessage(lifePoint, (uint)((StatsHealth)client.ActiveCharacter.Stats["Health"]).TotalMax));
        }

        public static void SendLifePointsRegenEndMessage(WorldClient client)
        {
            client.Send(new LifePointsRegenEndMessage(
                (uint)client.ActiveCharacter.Stats["Health"].Total,
                (uint)((StatsHealth)client.ActiveCharacter.Stats["Health"]).TotalMax,
                0));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(
                new CharacterStatsListMessage(
                    new CharacterCharacteristicsInformations(

                /* Experience */ client.ActiveCharacter.Experience,
                /* Experience Level Floor */ ThresholdProvider.Instance["CharacterExp"].GetLowerBound(client.ActiveCharacter.Experience),
                /* Experience Next Floor */  ThresholdProvider.Instance["CharacterExp"].GetUpperBound(client.ActiveCharacter.Experience),

                /* Kamas Amount */ (uint)client.ActiveCharacter.Inventory.Kamas,

                /* Stats points */ (uint)client.ActiveCharacter.StatsPoint,
                /* Spell points */ (uint)client.ActiveCharacter.SpellsPoints,


                        // Alignment
                        new ActorExtendedAlignmentInformations(
                            0, // alignmentSide
                            0, // alignmentValue 
                            10, // alignmentGrade 
                            0, // dishonor
                            0, // characterPower
                            0, // honor points
                            0, // honorGradeFloor
                            0, // honorNextGradeFloor
                            false // pvpEnabled
                            ),
                        (uint)client.ActiveCharacter.Stats["Health"].
                                   Total, // Life points
                        (uint)((StatsHealth)client.ActiveCharacter.Stats["Health"]).
                                   TotalMax, // Max Life points

              /* Current Energy */ client.ActiveCharacter.Energy,
                /* Max Energy */ client.ActiveCharacter.EnergyMax,

                        (short)client.ActiveCharacter.Stats["AP"]
                                    .Total, // actionPointsCurrent
                        (short)client.ActiveCharacter.Stats["MP"]
                                    .Total, // movementPointsCurrent

                        client.ActiveCharacter.Stats["Initiative"],
                        client.ActiveCharacter.Stats["Prospecting"],
                        client.ActiveCharacter.Stats["AP"],
                        client.ActiveCharacter.Stats["MP"],
                        client.ActiveCharacter.Stats["Strength"],
                        client.ActiveCharacter.Stats["Vitality"],
                        client.ActiveCharacter.Stats["Wisdom"],
                        client.ActiveCharacter.Stats["Chance"],
                        client.ActiveCharacter.Stats["Agility"],
                        client.ActiveCharacter.Stats["Intelligence"],
                        client.ActiveCharacter.Stats["Range"],
                        client.ActiveCharacter.Stats["SummonLimit"],
                        client.ActiveCharacter.Stats["DamageReflection"],
                        client.ActiveCharacter.Stats["CriticalHit"],
                        client.ActiveCharacter.Inventory.WeaponCriticalHit,
                        client.ActiveCharacter.Stats["CriticalMiss"],
                        client.ActiveCharacter.Stats["HealBonus"],
                        client.ActiveCharacter.Stats["DamageBonus"],
                        client.ActiveCharacter.Stats["WeaponDamageBonus"],
                        client.ActiveCharacter.Stats["DamageBonusPercent"],
                        client.ActiveCharacter.Stats["TrapBonus"],
                        client.ActiveCharacter.Stats["TrapBonusPercent"],
                        client.ActiveCharacter.Stats["PermanentDamagePercent"],
                        client.ActiveCharacter.Stats["TackleBlock"],
                        client.ActiveCharacter.Stats["TackleEvade"],
                        client.ActiveCharacter.Stats["APAttack"],
                        client.ActiveCharacter.Stats["MPAttack"],
                        client.ActiveCharacter.Stats["PushDamageBonus"],
                        client.ActiveCharacter.Stats["CriticalDamageBonus"],
                        client.ActiveCharacter.Stats["NeutralDamageBonus"],
                        client.ActiveCharacter.Stats["EarthDamageBonus"],
                        client.ActiveCharacter.Stats["WaterDamageBonus"],
                        client.ActiveCharacter.Stats["AirDamageBonus"],
                        client.ActiveCharacter.Stats["FireDamageBonus"],
                        client.ActiveCharacter.Stats["DodgeAPProbability"],
                        client.ActiveCharacter.Stats["DodgeMPProbability"],
                        client.ActiveCharacter.Stats["NeutralResistPercent"],
                        client.ActiveCharacter.Stats["EarthResistPercent"],
                        client.ActiveCharacter.Stats["WaterResistPercent"],
                        client.ActiveCharacter.Stats["AirResistPercent"],
                        client.ActiveCharacter.Stats["FireResistPercent"],
                        client.ActiveCharacter.Stats["NeutralElementReduction"],
                        client.ActiveCharacter.Stats["EarthElementReduction"],
                        client.ActiveCharacter.Stats["WaterElementReduction"],
                        client.ActiveCharacter.Stats["AirElementReduction"],
                        client.ActiveCharacter.Stats["FireElementReduction"],
                        client.ActiveCharacter.Stats["PushDamageReduction"],
                        client.ActiveCharacter.Stats["CriticalDamageReduction"],
                        client.ActiveCharacter.Stats["PvpNeutralResistPercent"],
                        client.ActiveCharacter.Stats["PvpEarthResistPercent"],
                        client.ActiveCharacter.Stats["PvpWaterResistPercent"],
                        client.ActiveCharacter.Stats["PvpAirResistPercent"],
                        client.ActiveCharacter.Stats["PvpFireResistPercent"],
                        client.ActiveCharacter.Stats["PvpNeutralElementReduction"],
                        client.ActiveCharacter.Stats["PvpEarthElementReduction"],
                        client.ActiveCharacter.Stats["PvpWaterElementReduction"],
                        client.ActiveCharacter.Stats["PvpAirElementReduction"],
                        client.ActiveCharacter.Stats["PvpFireElementReduction"],
                        new List<CharacterSpellModification>()
                        )));
        }

        public static void SendCharacterLevelUpMessage(WorldClient client, uint level)
        {
            client.Send(new CharacterLevelUpMessage(level));
        }

        public static void SendCharacterLevelUpInformationMessage(WorldClient client, Character character, uint level)
        {
            client.Send(new CharacterLevelUpInformationMessage(level, character.Name, (uint)character.Id, 0));
        }
    }
}