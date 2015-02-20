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
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin
{
    public static class OrbsManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static short OrbItemTemplateId = 20000;

        [Variable]
        public static short OrbsExchangeRate = 1000;

        public static ItemTemplate OrbItemTemplate;

        [Variable]
        public static double FormulasCoefficient = 0.0022;

        [Variable]
        public static double FormulasExponent = 2.18;



        [Initialization(typeof(ItemManager))]
        public static void Initialize()
        {
            OrbItemTemplate = ItemManager.Instance.TryGetTemplate(OrbItemTemplateId);

            if (OrbItemTemplate == null)
                logger.Error("Orb item template {0} doesn't exist in database !", OrbItemTemplateId);
            else
                FightManager.Instance.EntityAdded += OnFightCreated;
        }

        private static void OnFightCreated(IFight fight)
        {
            if (fight is FightPvM)
            {
                fight.WinnersDetermined += OnWinnersDetermined;
            }
        }

        private static void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            var monsters = fight.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            var players = fight.GetAllFighters<CharacterFighter>().ToList();

            var totalOrbs = (uint) monsters.Sum(x => GetMonsterDroppedOrbs(x));

            foreach (var player in players)
            {                
                var teamPP = player.Team.GetAllFighters().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                var orbs = (uint) (((double)player.Stats[PlayerFields.Prospecting].Total/teamPP)*totalOrbs);

                if (orbs > 0)
                    player.Loot.AddItem(new DroppedItem(OrbItemTemplateId, orbs));
            }

            if (fight.Map.TaxCollector == null)
                return;

            var item = fight.Map.TaxCollector.Bag.TryGetItem(OrbItemTemplate);
            var limit = fight.Map.TaxCollector.Guild.TaxCollectorPods;

            if (item != null)
            {
                limit -= (int)item.Stack;
            }

            var collectorOrbs = (uint) (((double)fight.Map.TaxCollector.Guild.TaxCollectorProspecting/
                                players.Sum(entry => entry.Stats[PlayerFields.Prospecting].Total))*totalOrbs*0.05);

            if (collectorOrbs > limit)
                collectorOrbs = (uint)limit;

            fight.TaxCollectorLoot.AddItem(new DroppedItem(OrbItemTemplateId, collectorOrbs));
        }

        private static uint GetMonsterDroppedOrbs(MonsterFighter monster)
        {
            return (uint)Math.Floor(FormulasCoefficient * (monster.Monster.Template.IsBoss ? 8 : 1) * Math.Pow(monster.Level, FormulasExponent)) +
                (uint)Math.Floor(Math.Pow(Math.Log(2 * monster.Level), 0.6));
        }
    }
}