using System;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    public class ControlStatementEnd : IStatement
    {

    }

    public class ControlStatement : IStatement
    {
        public static string Pattern =
            @"\b(?<type>if\(|else if\(|else\(|while\(|for each\(|for\(|break|return);?\s*(?<condition>\(?\s*[^;]*\s*\)?)?";

        public ControlType ControlType
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }

        public static ControlStatement Parse(string str)
        {
            Match match = Regex.Match(str, Pattern, RegexOptions.Multiline);
            ControlStatement result = null;

            if (match.Success)
            {
                result = new ControlStatement();

                if (match.Groups["type"].Value != "")
					result.ControlType =
                        (ControlType)
                        Enum.Parse(typeof (ControlType), match.Groups["type"].Value.Replace(" ", "").Replace("(", ""), true);

                if (match.Groups["condition"].Value != "")
                {
                    result.Condition = match.Groups["condition"].Value.Replace("(", "").Replace(")", "").Trim();
                }
            }

            return result;
        }
    }
}