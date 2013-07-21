#region License GNU GPL
// OrbsManager.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin
{
    public static class OrbsManager
    {
        [Variable] public static short OrbItemTemplate = 20000;
        [Variable]
        public static double FormulasCoefficient = 0.001;
        [Variable]
        public static double FormulasExponent = 2.2;



        [Initialization(InitializationPass.Fourth)]
        public static void Initialize()
        {
            FightManager.Instance.EntityAdded += OnFightCreated;
        }

        private static void OnFightCreated(Fight fight)
        {
            if (fight is FightPvM)
            {
                fight.WinnersDetermined += OnWinnersDetermined;
            }
        }

        private static void OnWinnersDetermined(Fight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            var monsters = fight.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            var players = fight.GetAllFighters<CharacterFighter>().ToList();

            var totalOrbs = (uint) monsters.Sum(x => GetMonsterDroppedOrbs(x));

            foreach (var player in players)
            {
                int teamPP = player.Team.GetAllFighters().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                var orbs = (uint) (((double)player.Stats[PlayerFields.Prospecting].Total/teamPP)*totalOrbs);

                if (orbs > 0)
                    player.Loot.AddItem(new DroppedItem(OrbItemTemplate, orbs));
            }
        }

        private static uint GetMonsterDroppedOrbs(MonsterFighter monster)
        {
            return (uint)Math.Floor(FormulasCoefficient * Math.Pow(monster.Level, FormulasExponent)) +
                (uint)Math.Floor(Math.Pow(5 * Math.Log(5 * monster.Level), 0.6));
        }
    }
}