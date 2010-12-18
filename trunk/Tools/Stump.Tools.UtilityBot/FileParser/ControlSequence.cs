// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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