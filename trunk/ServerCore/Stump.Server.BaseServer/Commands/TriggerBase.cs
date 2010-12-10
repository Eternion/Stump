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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class TriggerBase
    {
        protected TriggerBase(StringStream args, RoleEnum userRole)
        {
            Args = args;
            UserRole = userRole;
        }

        public StringStream Args
        {
            get;
            private set;
        }

        public RoleEnum UserRole
        {
            get;
            set;
        }

        public CommandBase BindedCommand
        {
            get;
            internal set;
        }


        public SubCommand BindedSubCommand
        {
            get;
            internal set;
        }

        internal IEnumerable<ICommandParameter> CommandsParameters
        {
            get;
            set;
        }

        /// <summary>
        ///   Replies accordingly with the given text.
        /// </summary>
        public abstract void Reply(string text);

        public void Reply(string format, params object[] args)
        {
            Reply(string.Format(format, args));
        }

        private void ReplyError(string message)
        {
            Reply("(Error) " + message);
        }

        public T GetArgument<T>(string name)
        {
            IEnumerable<ICommandParameter> matchingParams =
                CommandsParameters.Where(entry => entry.IsRightName(name, CommandBase.IgnoreCommandCase));

            if (matchingParams.Count() <= 0)
                throw new ArgumentException("'" + name + "' is not an existing parameter");

            return (T) matchingParams.First().Value;
        }

        public bool ArgumentExists(string name)
        {
            IEnumerable<ICommandParameter> matchingParams =
                CommandsParameters.Where(entry => entry.IsRightName(name, CommandBase.IgnoreCommandCase));

            return matchingParams.Count() > 0;
        }

        public string GetBindedCommandName()
        {
            return BindedCommand != null
                       ? BindedCommand.Aliases.FirstOrDefault() +
                         (BindedSubCommand != null
                              ? " " + BindedSubCommand.Aliases.FirstOrDefault()
                              : "")
                       : "";
        }

        internal bool DefineParameters(IEnumerable<ICommandParameter> commandParameters)
        {
            return DefineParameters(commandParameters, ReplyError);
        }

        internal bool DefineParameters(IEnumerable<ICommandParameter> commandParameters, Action<string> errorDelegate)
        {
            var definedParam = new List<ICommandParameter>();

            string word = Args.NextWord();
            while (!string.IsNullOrEmpty(word) && definedParam.Count < commandParameters.Count())
            {
                if (word.StartsWith("\"") && word.EndsWith("\""))
                    word = word.Remove(word.Length - 1, 1).Remove(0, 1);

                Match matchIsNamed = Regex.Match(word, @"^(?!\"")(?:-|--)?(\w+)=([^\""\s]*)(?!\"")$");
                Match matchVar = Regex.Match(word, @"^(?!\"")(?:-|--)(\w+)(?!\"")$");
                if (matchIsNamed.Success)
                {
                    string name = matchIsNamed.Groups[1].Value;
                    string value = matchIsNamed.Groups[2].Value;

                    IEnumerable<ICommandParameter> matchingParams =
                        commandParameters.Where(entry => entry.IsRightName(name, CommandBase.IgnoreCommandCase));

                    if (matchingParams.Count() == 0)
                    {
                        errorDelegate(string.Format("Unknown parameter : {0}", name));
                        return false;
                    }

                    foreach (ICommandParameter commandParameter in matchingParams)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(value))
                                commandParameter.SetDefaultValue();
                            else
                                commandParameter.SetStringValue(value);
                        }
                        catch
                        {
                            errorDelegate(string.Format("Cannot parse : {0}", word));
                            return false;
                        }

                        definedParam.Add(commandParameter);
                    }
                }
                else if (matchVar.Success)
                {
                    string name = matchVar.Groups[1].Value;

                    IEnumerable<ICommandParameter> matchingParams =
                        commandParameters.Where(entry => entry.IsRightName(name, CommandBase.IgnoreCommandCase));

                    if (matchingParams.Count() == 0)
                    {
                        errorDelegate(string.Format("Unknown parameter : {0}", name));
                        return false;
                    }

                    foreach (ICommandParameter commandParameter in matchingParams)
                    {
                        try
                        {
                            commandParameter.SetDefaultValue();
                        }
                        catch
                        {
                            errorDelegate(string.Format("Cannot parse : {0}", word));
                            return false;
                        }

                        definedParam.Add(commandParameter);
                    }
                }
                else
                {
                    ICommandParameter next = commandParameters.SkipWhile(definedParam.Contains).First();

                    try
                    {
                        next.SetStringValue(word);
                    }
                    catch
                    {
                        errorDelegate(string.Format("Cannot parse : {0}", word));
                        return false;
                    }

                    definedParam.Add(next);
                }

                word = Args.NextWord();
            }


            foreach (ICommandParameter undefinedParameter in
                commandParameters.Where(entry => !definedParam.Contains(entry)))
            {
                if (!undefinedParameter.SetDefaultValue())
                {
                    errorDelegate(string.Format("{0} is not defined", undefinedParameter.Name));
                    return false;
                }

                definedParam.Add(undefinedParameter);
            }

            CommandsParameters = definedParam;
            return true;
        }
    }
}