using System;
using System.Collections.Generic;
using System.Text;
using Stump.Server.WorldServer.Game.Conditions.Criterions;

namespace Stump.Server.WorldServer.Game.Conditions
{
    public class ConditionParser
    {
        private Dictionary<int, int> m_parentheses = new Dictionary<int, int>();

        public ConditionParser(string s)
        {
            String = s;
        }

        public string String
        {
            get;
            private set;
        }

        public ConditionExpression Parse()
        {
            TrimAllSpaces();

            PriorityOperator priorityOp = null;

            ParseParentheses();

            // str start and end with the same parenthese closure
            while (m_parentheses.ContainsKey(0) &&
                m_parentheses[0] == String.Length - 1)
            {
                // then we delete them, they are useless
                String = String.Remove(String.Length - 1, 1).Remove(0, 1);

                if (priorityOp != null)
                {
                    priorityOp.Expression = new PriorityOperator();
                    priorityOp = priorityOp.Expression as PriorityOperator;
                }
                else
                {
                    priorityOp = new PriorityOperator();
                }

                ParseParentheses();
            }

            var boolOp = TryParseBoolOperator();

            if (boolOp != null)
            {
                if (priorityOp != null)
                {
                    priorityOp.Expression = boolOp;
                    return priorityOp;
                }

                return boolOp;
            }

            var compOp = TryParseComparaisonOperator();

            if (compOp == null)
                throw new Exception(string.Format("Cannot parse {0} : No operator found", String));

            if (priorityOp != null)
            {
                priorityOp.Expression = compOp;
                return priorityOp;
            }

            return compOp;
        }

        private void TrimAllSpaces()
        {
            var builder = new StringBuilder();

            bool inQuote = false;

            for (int i = 0; i < String.Length; i++)
            {
                if (String[i] == '\"')
                    inQuote = !inQuote;

                if (inQuote)
                {
                    builder.Append(String[i]);
                }

                else
                {
                    if (String[i] != ' ')
                        builder.Append(String[i]);
                }
            }

            String = builder.ToString();
        }

        private ConditionExpression TryParseBoolOperator()
        {
            bool inQuote = false;
            bool inParenthese = false;

            for (int i = 0; i < String.Length; i++)
            {
                if (String[i] == '\"')
                {
                    inQuote = !inQuote;
                    continue;
                }

                if (String[i] == '(')
                {
                    inParenthese = true;
                    continue;
                }

                if (String[i] == ')')
                {
                    inParenthese = false;
                    continue;
                }

                if (inQuote || inParenthese)
                {
                    continue;
                }

                var op = BoolOperator.TryGetOperator(String[i]);

                if (op != null)
                {
                    if (i + 1 >= String.Length)
                        throw new Exception(string.Format("Cannot parse {0} :  Right Expression of bool operator index {1} is empty", String, i));

                    var leftStr = String.Substring(0, i);

                    if (string.IsNullOrEmpty(leftStr))
                        throw new Exception(string.Format("Cannot parse {0} : Left Expression of bool operator index {1} is empty", String, i));

                    var parser = new ConditionParser(leftStr);
                    var left = parser.Parse();

                    var rightStr = String.Substring(i + 1, String.Length - (i + 1));

                    if (string.IsNullOrEmpty(rightStr))
                        throw new Exception(string.Format("Cannot parse {0} : Right Expression of bool operator index {1} is empty", String, i));

                    parser = new ConditionParser(rightStr);
                    var right = parser.Parse();

                    return new BoolOperator(left, right, op.Value);
                }
            }

            return null;
        }

        private ConditionExpression TryParseComparaisonOperator()
        {
            int index = 0;
            bool inQuote = false;
            bool inParenthese = false;

            while (index < String.Length)
            {
                if (String[index] == '\"')
                {
                    inQuote = !inQuote;
                    index++;
                    continue;
                }

                if (String[index] == '(')
                {
                    inParenthese = true;
                    index++;
                    continue;
                }

                if (String[index] == ')')
                {
                    inParenthese = false;
                    index++;
                    continue;
                }

                if (inQuote || inParenthese)
                {
                    index++;
                    continue;
                }

                var op = Criterion.TryGetOperator(String[index]);

                if (op != null)
                {
                    if (index + 1 >= String.Length)
                        throw new Exception(string.Format("Cannot parse {0} :  Right Expression of comparaison operator index {1} is empty", String, index));

                    var leftStr = String.Substring(0, index);

                    if (string.IsNullOrEmpty(leftStr))
                        throw new Exception(string.Format("Cannot parse {0} : Left Expression of comparaison operator index {1} is empty", String, index));

                    var criterion = Criterion.CreateCriterionByName(leftStr);

                    var rightStr = String.Substring(index + 1, String.Length - (index + 1));

                    if (string.IsNullOrEmpty(rightStr))
                        throw new Exception(string.Format("Cannot parse {0} : Right Expression of comparaison operator index {1} is empty", String, index));

                    criterion.Literal = rightStr;
                    criterion.Operator = op.Value;
                    criterion.Build();

                    return criterion;
                }

                index++;
            }

            return null;
        }

        private void ParseParentheses()
        {
            m_parentheses.Clear();
            var openIndexes = new Stack<int>();

            for (int i = 0; i < String.Length; i++)
            {
                if (String[i] == '(')
                {
                    openIndexes.Push(i);
                }

                if (String[i] == ')' && openIndexes.Count > 0)
                {
                    m_parentheses.Add(openIndexes.Pop(), i);
                }
                else if (String[i] == ')' && openIndexes.Count <= 0)
                {
                    throw new Exception(string.Format("Cannot evaluate {0} : Parenthese at index {1} is not binded to an open parenthese", String, i));
                }
            }

            if (openIndexes.Count > 0)
            {
                throw new Exception(string.Format("Cannot evaluate {0} : Parenthese at index {1} is not closed", String, openIndexes.Pop()));
            }
        }
    }
}