
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stump.Tools.UtilityBot.FileParser
{
    public class FunctionCall : IExecution
    {
        public static string Pattern =
            @"(?<assignationtype>[^=(new|throw new)]\w+\s+)?(?:(?<assignationtarget>\w+\.)*(?<assignation>\w+(?:\[\w+\])?\s*(?:=|:\*=)\s*))?(?<stereotype>new|throw new)?\s?(?<target>[\[\]_\w]+\.)*(?<name>[_\w]+)\((?<argument>[^,]+,?)*\);";

        public string Name
        {
            get;
            set;
        }

        public string Target
        {
            get;
            set;
        }

        public string ReturnVariableTypeAssignation
        {
            get;
            set;
        }

        public string ReturnVariableAssignationTarget
        {
            get;
            set;
        }

        public string ReturnVariableAssignation
        {
            get;
            set;
        }

        public string[] Args
        {
            get;
            set;
        }

        public string Stereotype
        {
            get;
            set;
        }

        #region IExecution Members

        public ExecutionType Type
        {
            get { return ExecutionType.FunctionCall; }
        }

        #endregion

        public static FunctionCall Parse(string str, AsParser parser)
        {
            Match match = Regex.Match(str, Pattern);
            FunctionCall result = null;

            if (match.Success)
            {
                result = new FunctionCall();

                if (match.Groups["assignationtype"].Value != "")
                    result.ReturnVariableTypeAssignation =
                        parser.ExecuteNameReplacement(match.Groups["assignationtype"].Value.Trim());

                foreach (object capture in match.Groups["assignationtarget"].Captures)
                {
                    result.ReturnVariableAssignationTarget += capture;
                }
                if (result.ReturnVariableAssignationTarget != null &&
                    result.ReturnVariableAssignationTarget.EndsWith("."))
                    result.ReturnVariableAssignationTarget =
                        parser.ExecuteNameReplacement(
                            result.ReturnVariableAssignationTarget.Remove(
                                result.ReturnVariableAssignationTarget.Length - 1, 1)); // remove dot

                if (match.Groups["assignation"].Value != "")
                    result.ReturnVariableAssignation = parser.ExecuteNameReplacement(
                        match.Groups["assignation"].Value.Trim().
                            Replace("=", "").
                            Replace(":", "").
                            Replace("*", "").
                            Trim());

                result.Stereotype = match.Groups["stereotype"].Value;

                foreach (object capture in match.Groups["target"].Captures)
                {
                    result.Target += capture;
                }
                if (result.Target != null && result.Target.EndsWith("."))
                    result.Target = parser.ExecuteNameReplacement(result.Target.Remove(result.Target.Length - 1, 1));
                        // remove dot

                result.Name = parser.ExecuteNameReplacement(match.Groups["name"].Value);

                var args = new List<string>();
                foreach (object capture in match.Groups["argument"].Captures)
                {
                    args.Add(capture.ToString().Trim().Replace(",", ""));
                }

                result.Args = args.ToArray();
            }

            return result;
        }
    }
}