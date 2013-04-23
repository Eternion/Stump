using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Game.Formulas
{
    public class FightFormulas
    {
        public static event Func<CharacterFighter, int, int> WinXpModifier;

        public static int InvokeWinXpModifier(CharacterFighter looter, int xp)
        {
            var handler = WinXpModifier;
            if (handler != null)
                return handler(looter, xp);

            return xp;
        }

        public static event Func<CharacterFighter, int, int> WinKamasModifier;

        public static int InvokeWinKamasModifier(CharacterFighter looter, int kamas)
        {
            var handler = WinKamasModifier;
            if (handler != null)
                return handler(looter, kamas);

            return kamas;
        }

        public static event Func<CharacterFighter, DroppableItem, double, double> DropRateModifier;

        public static double InvokeDropRateModifier(CharacterFighter looter, DroppableItem item, double rate)
        {
            var handler = DropRateModifier;
            if (handler != null)
                return handler(looter, item, rate);

            return rate;
        }

        public static readonly double[] GroupCoefficients =
            new[]
                {
                    1,
                    1.1,
                    1.5,
                    2.3,
                    3.1,
                    3.6,
                    4.2,
                    4.7
                };


        public static int CalculateWinExp(CharacterFighter fighter)
        {
            if (fighter.HasLeft())
                return 0;

            IEnumerable<MonsterFighter> monsters = fighter.OpposedTeam.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            IEnumerable<CharacterFighter> players = fighter.Team.GetAllFighters<CharacterFighter>().ToList();

            if (!monsters.Any() || !players.Any())
                return 0;

            int sumPlayersLevel = players.Sum(entry => entry.Level);
            byte maxPlayerLevel = players.Max(entry => entry.Level);
            int sumMonstersLevel = monsters.Sum(entry => entry.Level);
            byte maxMonsterLevel = monsters.Max(entry => entry.Level);
            int sumMonsterXp = monsters.Sum(entry => entry.Monster.Grade.GradeXp);

            double levelCoeff = 1;
            if (sumPlayersLevel - 5 > sumMonstersLevel)
                levelCoeff = (double)sumMonstersLevel / sumPlayersLevel;
            else if (sumPlayersLevel + 10 < sumMonstersLevel)
                levelCoeff = ( sumPlayersLevel + 10 ) / (double)sumMonstersLevel;

            double xpRatio = Math.Min(fighter.Level, Math.Truncate(2.5d * maxMonsterLevel)) / sumPlayersLevel * 100d;

            int regularGroupRatio = players.Where(entry => entry.Level >= maxPlayerLevel / 3).Sum(entry => 1);

            if (regularGroupRatio <= 0)
                regularGroupRatio = 1;

            double baseXp = Math.Truncate(xpRatio / 100 * Math.Truncate(sumMonsterXp * GroupCoefficients[regularGroupRatio - 1] * levelCoeff));
            double multiplicator = fighter.Fight.AgeBonus <= 0 ? 1 : 1 + fighter.Fight.AgeBonus / 100d;
            var xp = (int)Math.Truncate(Math.Truncate(baseXp * ( 100 + fighter.Stats[PlayerFields.Wisdom].Total ) / 100d) * multiplicator * Rates.XpRate);

            return InvokeWinXpModifier(fighter, xp);
        }

        public static int AdjustDroppedKamas(CharacterFighter looter, int teamPP, long baseKamas)
        {
            int looterPP = looter.Stats[PlayerFields.Prospecting].Total;

            double multiplicator = looter.Fight.AgeBonus <= 0 ? 1 : 1 + looter.Fight.AgeBonus / 100d;
            int kamas = (int)( baseKamas * ( (double)looterPP / teamPP ) * multiplicator * Rates.KamasRate );

            return InvokeWinKamasModifier(looter, kamas);
        }

        public static double AdjustDropChance(CharacterFighter looter, DroppableItem item, Monster dropper, int monsterAgeBonus)
        {
            var rate = item.GetDropRate((int) dropper.Grade.GradeId) * ( looter.Stats[PlayerFields.Prospecting] / 100d ) * ( ( monsterAgeBonus / 100d ) + 1 ) * Rates.DropsRate;

            return InvokeDropRateModifier(looter, item, rate);
        }

    }
}