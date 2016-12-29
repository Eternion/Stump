#region License GNU GPL
// SpellCast.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class AISpellCastPossibility
    {
        public const int MAX_CONSECUTIVE_CAST = 10;

        public AISpellCastPossibility(Spell spell, SpellTarget target)
        {
            Target = target;
            Spell = spell;
            MaxConsecutiveCast = MAX_CONSECUTIVE_CAST;
        }

        public Spell Spell
        {
            get;
            set;
        }
        
        public SpellTarget Target
        {
            get;
            private set;
        }

        public TargetCell TargetCell
        {
            get { return Target.Target; }
        }

        public Path MoveBefore
        {
            get;
            set;
        }

        public int MPCost
        {
            get { return MoveBefore != null ? MoveBefore.MPCost : 0; }
        }

        public Path MoveAfter
        {
            get;
            set;
        }

        public int MaxConsecutiveCast
        {
            get;
            set;
        }
    }
}