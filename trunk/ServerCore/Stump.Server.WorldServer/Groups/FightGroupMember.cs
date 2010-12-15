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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Groups
{
    public sealed class FightGroupMember : GroupMember
    {
        public FightGroupMember(LivingEntity ent, FightGroup groupOwner)
            : base(ent, groupOwner)
        {
            IsReady = false;
            IsDead = false;
            UsedAp = 0;
            UsedMp = 0;
            DamageTaken = 0;
        }

        public int TotalAp
        {
            get { return Entity.Stats["AP"].Total - UsedAp; }
        }

        public int TotalMp
        {
            get { return Entity.Stats["MP"].Total - UsedMp; }
        }

        public int CurrentHealth
        {
            get { return Entity.Stats["Health"].Total - DamageTaken; }
        }

        public int UsedAp
        {
            get;
            set;
        }

        public int UsedMp
        {
            get;
            set;
        }

        public int DamageTaken
        {
            get;
            set;
        }

        public CellData Cell
        {
            get;
            set;
        }

        public bool IsReady
        {
            get;
            set;
        }

        public bool IsDead
        {
            get;
            set;
        }

        public bool HasLeft
        {
            get;
            set;
        }

        public bool ReadyTurnEnd
        {
            get;
            set;
        }

        public bool IsInTurn
        {
            get { return Equals(((FightGroup) GroupOwner).Fight.FighterPlaying); }
        }

        public void ResetUsedProperties()
        {
            UsedMp = 0;
            UsedAp = 0;
            ReadyTurnEnd = false;
        }

        public ushort ReceiveDamage(ushort damage)
        {
            if (CurrentHealth - damage < 0)
                damage = (ushort) CurrentHealth;

            DamageTaken += damage;

            return damage;
        }

        public GameFightMinimalStats GetFightMinimalStats()
        {
            return new GameFightMinimalStats(
                (uint)CurrentHealth,
                (uint)Entity.MaxHealth,
                TotalAp,
                TotalMp,
                Entity.Stats["SummonLimit"].Total,
                Entity.Stats["NeutralResistPercent"].Total,
                Entity.Stats["EarthResistPercent"].Total,
                Entity.Stats["WaterResistPercent"].Total,
                Entity.Stats["AirResistPercent"].Total,
                Entity.Stats["FireResistPercent"].Total,
                (uint)Entity.Stats["DodgeAPProbability"].Total,
                (uint)Entity.Stats["DodgeMPProbability"].Total,
                0, // tackleblock
                (int)GameActionFightInvisibilityStateEnum.VISIBLE);
        }
    }
}