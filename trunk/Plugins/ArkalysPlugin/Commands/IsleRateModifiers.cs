using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Maps;

namespace ArkalysPlugin.Commands
{
    public class IsleRateModifiers
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Dictionary<SubArea, Isle> m_isles = new Dictionary<SubArea, Isle>();

        [Variable]
        public static double OverLevelMalus = 4;

        [Variable]
        public static double MalusLimit = 80;

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            int i = 0;
            foreach (var isle in IsleCommand.ProfilsIsle)
            {
                if (isle.SubAreas == null || isle.SubAreas.Length == 0)
                {
                    logger.Debug("Isle {0} has no subareas, Rates cannot be applied", i);
                    continue;
                }

                foreach (var subAreaId in isle.SubAreas)
                {
                    var area = World.Instance.GetSubArea(subAreaId);
                    m_isles.Add(area, isle);

                    foreach (var spawn in area.Maps.SelectMany(entry => entry.MonsterSpawns).Distinct())
                    {
                        var monster = MonsterManager.Instance.GetTemplate(spawn.MonsterId);

                        foreach (var grade in monster.Grades)
                        {
                            grade.Strength = (short)( grade.Strength * ( 1 / isle.StatsModifier ) );
                            grade.Chance = (short)( grade.Chance * ( 1 / isle.StatsModifier ) );
                            grade.Agility = (short)( grade.Agility * ( 1 / isle.StatsModifier ) );
                            grade.Intelligence = (short)( grade.Intelligence * ( 1 / isle.StatsModifier ) );
                        }
                    }
                }

                i++;
            }

            FightFormulas.WinXpModifier += WinXpModifier;
            FightFormulas.WinKamasModifier += WinKamasModifier;
        }

        private static int DropRateModifier(CharacterFighter looter, DroppableItem item, int rate)
        {
            return rate;
        }

        private static int WinKamasModifier(CharacterFighter looter, int kamas)
        {
            var area = looter.Fight.Map.SubArea;

            if (!m_isles.ContainsKey(area))
                return kamas;

            var isle = m_isles[area];

            return (int)( kamas * isle.KamasRate * GetMalus(looter.Character, isle));
        }

        private static int WinXpModifier(CharacterFighter looter, int xp)
        {
            var area = looter.Fight.Map.SubArea;

            if (!m_isles.ContainsKey(area))
                return xp;

            var isle = m_isles[area];

            return (int)( xp * isle.XPRate * GetMalus(looter.Character, isle));
        }

        private static double GetMalus(Character character, Isle currentIsle)
        {
            var index = Array.IndexOf(IsleCommand.ProfilsIsle, currentIsle);

            if (index == -1)
                return 1;

            // last isle
            if (IsleCommand.ProfilsIsle.Length - 1 == index)
                return 1;

            var isle = GetCharacterLastIsle(character);

            if (isle == currentIsle)
                return 1;

            var difference = character.Level - isle.Level;

            var malusPercent = OverLevelMalus * difference;

            if (malusPercent > MalusLimit)
                malusPercent = MalusLimit;

            return 1 - ( malusPercent ) / 100d;
        }

        private static Isle GetCharacterLastIsle(Character character)
        {
            for (int i = 0; i < IsleCommand.ProfilsIsle.Length; i++)
            {
                if (IsleCommand.ProfilsIsle[i].Level > character.Level)
                {
                    if (i == 0)
                        return IsleCommand.ProfilsIsle[i];
                    else
                        return IsleCommand.ProfilsIsle[i - 1];
                }
            }

            return IsleCommand.ProfilsIsle.Last();
        }
    }
}