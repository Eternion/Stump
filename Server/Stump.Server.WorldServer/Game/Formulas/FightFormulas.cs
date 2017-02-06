using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Fights.Results;

namespace Stump.Server.WorldServer.Game.Formulas
{
    public class FightFormulas
    {
        public static event Func<IFightResult, int, int> WinXpModifier;

        public static int InvokeWinXpModifier(IFightResult looter, int xp)
        {
            var handler = WinXpModifier;
            return handler != null ? handler(looter, xp) : xp;
        }

        public static event Func<IFightResult, int, int> WinKamasModifier;

        public static int InvokeWinKamasModifier(IFightResult looter, int kamas)
        {
            var handler = WinKamasModifier;
            return handler != null ? handler(looter, kamas) : kamas;
        }

        public static event Func<IFightResult, DroppableItem, double, double> DropRateModifier;

        public static double InvokeDropRateModifier(IFightResult looter, DroppableItem item, double rate)
        {
            var handler = DropRateModifier;
            return handler != null ? handler(looter, item, rate) : rate;
        }

        public static readonly double[] GroupCoefficients =
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

        public static int CalculateWinExp(IFightResult fighter, IEnumerable<FightActor> alliesResults, IEnumerable<FightActor> droppersResults)
        {
            var droppers = droppersResults as MonsterFighter[] ?? droppersResults.ToArray();
            var allies = alliesResults as FightActor[] ?? alliesResults.ToArray();

            if (!droppers.Any() || !allies.Any())
                return 0;

            var sumPlayersLevel = allies.Sum(entry => entry.Level);
            var maxPlayerLevel = allies.Max(entry => entry.Level);
            var sumMonstersLevel = droppers.Sum(entry => entry.Level);
            var sumMonstersHiddenLevel = droppers.OfType<MonsterFighter>().Sum(entry => entry.HiddenLevel == 0 ? entry.Level : entry.HiddenLevel);
            var maxMonsterLevel = droppers.Max(entry => entry.Level);
            var sumMonsterXp = droppers.Sum(entry => entry.GetGivenExperience());

            double levelCoeff = 1;
            if (sumPlayersLevel - 5 > sumMonstersLevel)
                levelCoeff = (double)sumMonstersLevel / sumPlayersLevel;
            else if (sumPlayersLevel + 10 < sumMonstersLevel)
                levelCoeff = ( sumPlayersLevel + 10 ) / (double)sumMonstersLevel;

            var xpRatio = Math.Min(fighter.Level, Math.Truncate(2.5d * maxMonsterLevel)) / sumPlayersLevel * 100d;

            var regularGroupRatio = allies.Where(entry => entry.Level >= maxPlayerLevel / 3).Sum(entry => 1);

            if (regularGroupRatio <= 0)
                regularGroupRatio = 1;

            var baseXp = Math.Truncate(xpRatio / 100 * Math.Truncate(sumMonsterXp * GroupCoefficients[regularGroupRatio - 1] * levelCoeff));
            var multiplicator = fighter.Fight.AgeBonus <= 0 ? 1 : 1 + fighter.Fight.AgeBonus / 100d;
            var challengeBonus = fighter.Fight.GetChallengesBonus();

            var idolsBonus = fighter.Fight.GetIdolsXPBonus();
            var idolsMalus = Math.Pow(Math.Min(4, ((double)sumMonstersHiddenLevel / droppers.Count() / maxPlayerLevel)), 2);
            var idolsWisdomBonus = Math.Truncate((100 + fighter.Level * 2.5d) * Math.Truncate(idolsBonus * idolsMalus) / 100d);

            var xp = (int)Math.Truncate(Math.Truncate(baseXp * (100 + Math.Max(fighter.Wisdom + idolsWisdomBonus, 0)) / 100d) * multiplicator * Rates.XpRate);
            xp += (int)Math.Truncate(xp * (challengeBonus / 100d));

            return InvokeWinXpModifier(fighter, xp);
        }

        public static int AdjustDroppedKamas(IFightResult looter, int teamPP, long baseKamas, bool kamasRate = true)
        {
            var challengeBonus = looter.Fight.GetChallengesBonus();
            var idolsBonus = looter.Fight.GetIdolsDropBonus();

            var looterPP = looter.Prospecting + ((looter.Prospecting * (challengeBonus + idolsBonus)) / 100d);

            var multiplicator = looter.Fight.AgeBonus <= 0 ? 1 : 1 + (looter.Fight.AgeBonus / 5) / 100d;
            var kamas = (int)( baseKamas * (looterPP / teamPP) * multiplicator * (kamasRate ? Rates.KamasRate : 1));

            return InvokeWinKamasModifier(looter, kamas);
        }

        public static double AdjustDropChance(IFightResult looter, DroppableItem item, Monster dropper, int monsterAgeBonus)
        {
            var challengeBonus = looter.Fight.GetChallengesBonus();
            var idolsBonus = looter.Fight.GetIdolsDropBonus();

            var looterPP = looter.Prospecting + ((looter.Prospecting * (challengeBonus + idolsBonus)) / 100d);

            var rate = item.GetDropRate((int)dropper.Grade.GradeId) * (looterPP / 100d) * ((monsterAgeBonus / 100d) + 1) * Rates.DropsRate;

            return InvokeDropRateModifier(looter, item, rate);
        }

        public static int CalculatePushBackDamages(FightActor source, FightActor target, int range, int targets)
        {
            var level = source.Level;

            var summon = source as SummonedMonster;
            if (summon != null)
                level = summon.Summoner.Level;

            return (int)((level / 2 + (source.Stats[PlayerFields.PushDamageBonus] - target.Stats[PlayerFields.PushDamageReduction]) + 32) * range / (4 * (Math.Pow(2, targets))));
        }
    }
}