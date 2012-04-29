using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public abstract class Criterion : ConditionExpression
    {
        public ComparaisonOperatorEnum Operator
        {
            get;
            set;
        }

        public string Literal
        {
            get;
            set;
        }

        public static ComparaisonOperatorEnum? TryGetOperator(char c)
        {
            switch (c)
            {
                case '=':
                    return ComparaisonOperatorEnum.EQUALS;
                case '!':
                    return ComparaisonOperatorEnum.INEQUALS;
                case '>':
                    return ComparaisonOperatorEnum.SUPERIOR;
                case '<':
                    return ComparaisonOperatorEnum.INFERIOR;
                case '~':
                    return ComparaisonOperatorEnum.LIKE;
                // it uses symboles of criterions identifier ...
                // todo : compare the 2 first letters
                /*case 's':
                    return ComparaisonOperatorEnum.STARTWITH;
                case 'S':
                    return ComparaisonOperatorEnum.STARTWITHLIKE;
                case 'e':
                    return ComparaisonOperatorEnum.ENDWITH;
                case 'E':
                    return ComparaisonOperatorEnum.ENDWITHLIKE;
                case 'i':
                    return ComparaisonOperatorEnum.INVALID;
                case 'v':
                    return ComparaisonOperatorEnum.VALID;
                case '#':
                    return ComparaisonOperatorEnum.UNKNOWN_1;
                case '/':
                    return ComparaisonOperatorEnum.UNKNOWN_2;
                case 'X': 
                    return ComparaisonOperatorEnum.UNKNOWN_3;*/
                default:
                    return null;
            }
        }

        public static char GetOperatorChar(ComparaisonOperatorEnum op)
        {
            switch (op)
            {
                case ComparaisonOperatorEnum.EQUALS:
                    return '=';
                case ComparaisonOperatorEnum.INEQUALS:
                    return '!';
                case ComparaisonOperatorEnum.SUPERIOR:
                    return '>';
                case ComparaisonOperatorEnum.INFERIOR:
                    return '<';
                case ComparaisonOperatorEnum.LIKE:
                    return '~';
                case ComparaisonOperatorEnum.STARTWITH:
                    return 's';
                case ComparaisonOperatorEnum.STARTWITHLIKE:
                    return 'S';
                case ComparaisonOperatorEnum.ENDWITH:
                    return 'e';
                case ComparaisonOperatorEnum.ENDWITHLIKE:
                    return 'E';
                case ComparaisonOperatorEnum.INVALID:
                    return 'i';
                case ComparaisonOperatorEnum.VALID:
                    return 'v';
                case ComparaisonOperatorEnum.UNKNOWN_1:
                    return '#';
                case ComparaisonOperatorEnum.UNKNOWN_2:
                    return '/';
                case ComparaisonOperatorEnum.UNKNOWN_3:
                    return 'X';
                default:
                    throw new Exception(string.Format("{0} is not a valid comparaison operator", op));
            }
        }

        public static Criterion CreateCriterionByName(string name)
        {
            if (StatsCriterion.IsStatsIdentifier(name))
                return new StatsCriterion(name);

            // switch are ugly but faster
            switch (name)
            {
                case AdminRightsCriterion.Identifier:
                    return new AdminRightsCriterion();
                case AlignementLevelCriterion.Identifier:
                    return new AlignementLevelCriterion();
                case AlignmentCriterion.Identifier:
                    return new AlignmentCriterion();
                case BonesCriterion.Identifier:
                    return new BonesCriterion();
                case BreedCriterion.Identifier:
                    return new BreedCriterion();
                case EmoteCriterion.Identifier:
                    return new EmoteCriterion();
                case FriendListCriterion.Identifier:
                    return new FriendListCriterion();
                case GiftCriterion.Identifier:
                    return new GiftCriterion();
                case HasItemCriterion.Identifier:
                    return new HasItemCriterion();
                case JobCriterion.Identifier:
                    return new JobCriterion();
                case KamaCriterion.Identifier:
                    return new KamaCriterion();
                case LevelCriterion.Identifier:
                    return new LevelCriterion();
                case MapCharactersCriterion.Identifier:
                    return new MapCharactersCriterion();
                case MariedCriterion.Identifier:
                    return new MariedCriterion();
                case MaxRankCriterion.Identifier:
                    return new MaxRankCriterion();
                case MonthCriterion.Identifier:
                    return new MonthCriterion();
                case NameCriterion.Identifier:
                    return new NameCriterion();
                case PreniumAccountCriterion.Identifier:
                    return new PreniumAccountCriterion();
                case PvpRankCriterion.Identifier:
                case PvpRankCriterion.Identifier2:
                    return new PvpRankCriterion();
                case QuestActiveCriterion.Identifier:
                    return new QuestActiveCriterion();
                case QuestDoneCriterion.Identifier:
                    return new QuestDoneCriterion();
                case QuestStartableCriterion.Identifier:
                    return new QuestStartableCriterion();
                case RankCriterion.Identifier:
                    return new RankCriterion();
                case RideCriterion.Identifier:
                    return new RideCriterion();
                case ServerCriterion.Identifier:
                    return new ServerCriterion();
                case SexCriterion.Identifier:
                    return new SexCriterion();
                case SkillCriterion.Identifier:
                case SkillCriterion.Identifier2:
                    return new SkillCriterion();
                case SoulStoneCriterion.Identifier:
                    return new SoulStoneCriterion();
                case SpecializationCriterion.Identifier:
                    return new SpecializationCriterion();
                case StaticCriterion.Identifier:
                    return new StaticCriterion();
                case SubAreaCriterion.Identifier:
                    return new SubAreaCriterion();
                case SubscribeCriterion.Identifier:
                    return new SubscribeCriterion();
                case UnusableCriterion.Identifier:
                    return new UnusableCriterion();
                case WeightCriterion.Identifier:
                    return new WeightCriterion();
                default:
                    throw new Exception(string.Format("Criterion {0} doesn't not exist or not handled", name)); 
            }
        }

        public abstract void Build();

        protected bool Compare(object obj, object comparand)
        {
            switch (Operator)
            {
                case ComparaisonOperatorEnum.EQUALS:
                    return obj.Equals(comparand);
                case ComparaisonOperatorEnum.INEQUALS:
                    return !obj.Equals(comparand);
                default:
                    throw new NotImplementedException(string.Format("Cannot use {0} comparator on objects {1} and {2}", Operator, obj, comparand));
            }
        }

        protected bool Compare<T>(IComparable<T> obj, T comparand)
        {
            int comparaison = obj.CompareTo(comparand);

            switch (Operator)
            {
                case ComparaisonOperatorEnum.EQUALS:
                    return comparaison == 0;
                case ComparaisonOperatorEnum.INEQUALS:
                    return comparaison != 0;
                case ComparaisonOperatorEnum.INFERIOR:
                    return comparaison < 0;
                case ComparaisonOperatorEnum.SUPERIOR:
                    return comparaison > 0;
                default:
                    throw new NotImplementedException(string.Format("Cannot use {0} comparator on IComparable {1} and {2}", Operator, obj, comparand));
            }
        }

        protected bool Compare(string str, string comparand)
        {
            switch (Operator)
            {
                case ComparaisonOperatorEnum.EQUALS:
                    return str == comparand;
                case ComparaisonOperatorEnum.INEQUALS:
                    return str != comparand;
                case ComparaisonOperatorEnum.LIKE:
                    return str.Equals(comparand, StringComparison.InvariantCultureIgnoreCase);
                case ComparaisonOperatorEnum.STARTWITH:
                    return str.StartsWith(comparand);
                case ComparaisonOperatorEnum.ENDWITH:
                    return str.EndsWith(comparand);
                case ComparaisonOperatorEnum.STARTWITHLIKE:
                    return str.StartsWith(comparand, StringComparison.InvariantCultureIgnoreCase);
                case ComparaisonOperatorEnum.ENDWITHLIKE:
                    return str.EndsWith(comparand, StringComparison.InvariantCultureIgnoreCase);
                default:
                    throw new NotImplementedException(string.Format("Cannot use {0} comparator on strings '{1}' and '{2}'", Operator, str, comparand));
            }
        }

        protected string FormatToString(string identifier)
        {
            return string.Format("{0}{1}{2}", identifier, GetOperatorChar(Operator), Literal);
        }
    }
}