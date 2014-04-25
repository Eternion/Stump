#region License GNU GPL
// SpellCastResult.cs
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
namespace Stump.Server.WorldServer.Game.Spells
{
    public enum SpellCastResult
    {
        NO_LOS,
        HISTORY_ERROR,
        NOT_IN_ZONE,
        STATE_REQUIRED,
        STATE_FORBIDDEN,
        CELL_NOT_FREE,
        NOT_ENOUGH_AP,
        UNWALKABLE_CELL,
        HAS_NOT_SPELL,
        CANNOT_PLAY,
        UNKNOWN,
        OK,
    }
}