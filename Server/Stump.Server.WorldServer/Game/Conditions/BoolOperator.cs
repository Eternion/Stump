using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions
{
    public enum BoolOperatorEnum
    {
        AND,
        OR
    }

    public class BoolOperator : ConditionExpression
    {
        public static BoolOperatorEnum? TryGetOperator(char c)
        {
            switch (c)
            {
                case '&':
                    return BoolOperatorEnum.AND;
                case '|':
                    return BoolOperatorEnum.OR;
                default:
                    return null;
            }
        }

        public static char GetOperatorChar(BoolOperatorEnum op)
        {
            switch (op)
            {
                case BoolOperatorEnum.AND:
                    return '&';
                case BoolOperatorEnum.OR:
                    return '|';
                default:
                    throw new Exception(string.Format("{0} is not a valid bool operator", op));
            }
        }

        public BoolOperator(ConditionExpression left, ConditionExpression right, BoolOperatorEnum @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public ConditionExpression Left
        {
            get;
            set;
        }

        public ConditionExpression Right
        {
            get;
            set;
        }

        public BoolOperatorEnum Operator
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            if (Operator != BoolOperatorEnum.AND && Operator != BoolOperatorEnum.OR)
                throw new Exception(string.Format("Cannot evaluate {0} : illegal bool operator {1}", this, Operator));

            var left = Left.Eval(character);

            if (Operator == BoolOperatorEnum.AND && !left)
                return false;

            if (Operator == BoolOperatorEnum.OR && left)
                return true;
            
            var right = Right.Eval(character);

            if (Operator == BoolOperatorEnum.AND)
                return left && right;

            if (Operator == BoolOperatorEnum.OR)
                return left || right;

            throw new Exception(string.Format("Cannot evaluate {0}", this));
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Left, GetOperatorChar(Operator), Right);
        }
    }
}