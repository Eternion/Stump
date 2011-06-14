
using System;
using System.Text.RegularExpressions;
using Stump.Tools.UtilityBot.FileWriter;

namespace Stump.Tools.UtilityBot.FileParser
{
    public class ControlSequenceEnd : IExecution
    {
        #region IExecution Members

        public ExecutionType Type
        {
            get { return ExecutionType.ControlSequenceEnd; }
        }

        #endregion
    }

    public class ControlSequence : IExecution
    {
        public static string Pattern =
            @"\b(?<type>if|else if|else|while|break|return);?\s*(?<condition>\(?\s*[^;]*\s*\)?)?";

        public ControlSequenceType SequenceType
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }

        #region IExecution Members

        public ExecutionType Type
        {
            get { return ExecutionType.ControlSequence; }
        }

        #endregion

        public static ControlSequence Parse(string str)
        {
            Match match = Regex.Match(str, Pattern);
            ControlSequence result = null;

            if (match.Success)
            {
                result = new ControlSequence();

                if (match.Groups["type"].Value != "")
                    result.SequenceType =
                        (ControlSequenceType)
                        Enum.Parse(typeof (ControlSequenceType), match.Groups["type"].Value.Trim(), true);

                if (match.Groups["condition"].Value != "")
                {
                    // remove the ( at the begin and the ) at the end
                    if (match.Groups["condition"].Value.StartsWith("(") &&
                        match.Groups["condition"].Value.EndsWith(")"))
                        result.Condition = match.Groups["condition"].Value.
                            Remove(match.Groups["condition"].Value.Length - 1, 1).
                            Remove(0, 1).
                            Trim();
                    else
                        result.Condition = match.Groups["condition"].Value.Trim();
                }
            }

            return result;
        }
    }
}