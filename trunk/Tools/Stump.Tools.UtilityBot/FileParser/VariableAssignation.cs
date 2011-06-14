
using System.Linq;
using System.Text.RegularExpressions;

namespace Stump.Tools.UtilityBot.FileParser
{
    /// <summary>
    /// </summary>
    /// <remarks>
    ///   Only work if the value is a variable or a hard coded value
    /// </remarks>
    public class VariableAssignation : IExecution
    {
        public static string Pattern =
            @"(?<assignationtype>^[^(new)]\w+\s+)?(?<target>\w+\.)*(?<name>\w+(?:\[\w+\])?)\s*(?:=|:\*=)\s*(?<value>.+);$";

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

        public string Value
        {
            get;
            set;
        }

        public string TypeDeclaration
        {
            get;
            set;
        }

        #region IExecution Members

        public ExecutionType Type
        {
            get { return ExecutionType.VariableAssignation; }
        }

        #endregion

        public static VariableAssignation Parse(string str, AsParser parser)
        {
            Match match = Regex.Match(str, Pattern);
            VariableAssignation result = null;

            if (match.Success)
            {
                result = new VariableAssignation();

                if (match.Groups["assignationtype"].Value != "")
                    result.TypeDeclaration = parser.ExecuteNameReplacement(match.Groups["assignationtype"].Value.Trim());


                if (match.Groups["target"].Value != "")
                {
                    foreach (object target in match.Groups["target"].Captures)
                    {
                        string trim = target.ToString().Trim();

                        result.Target += trim;
                    }
                    if (result.Target != null && result.Target.EndsWith("."))
                        result.Target = parser.ExecuteNameReplacement(result.Target.Remove(result.Target.Length - 1, 1));
                            // remove dot
                }

                result.Name = parser.ExecuteNameReplacement(match.Groups["name"].Value.Trim());

                result.Value = match.Groups["value"].Value.Trim();

                if (!result.Value.Contains("\""))
                {
                    if (result.Value.Contains("<"))
                    {
                        string generictype = result.Value.Split('<').Last().Split('>').First().Split('.').Last();
                        string defaulttype = result.Value.Split('<').Last().Split('>').First();

                        result.Value = result.Value.Replace(defaulttype, generictype);
                    }

                    result.Value = parser.ExecuteNameReplacement(result.Value);
                }
            }

            return result;
        }
    }
}