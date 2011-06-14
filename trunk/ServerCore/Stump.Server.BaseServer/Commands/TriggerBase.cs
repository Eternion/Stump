
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
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

        protected TriggerBase(string args, RoleEnum userRole)
        {
            Args = new StringStream(args);
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

        internal Dictionary<string, ICommandParameter> CommandsParametersByName
        {
            get;
            set;
        }

        internal Dictionary<string, ICommandParameter> CommandsParametersByShortName
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

        public virtual T GetArgument<T>(string name)
        {
            if (CommandsParametersByName.ContainsKey(name))
                return (T)CommandsParametersByName[name].Value;
            if (CommandsParametersByShortName.ContainsKey(name))
                return (T) CommandsParametersByShortName[name].Value;

            throw new ArgumentException("'" + name + "' is not an existing parameter");
        }

        public virtual bool ArgumentExists(string name)
        {
            ICommandParameter parameter;
            if (CommandsParametersByName.ContainsKey(name))
            {
                parameter = CommandsParametersByName[name];

                return parameter.IsValueDefined;
            }
            if (CommandsParametersByShortName.ContainsKey(name))
            {
                parameter = CommandsParametersByShortName[name];

                return parameter.IsValueDefined;
            }

            return false;
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

                Match matchIsNamed = Regex.Match(word, @"^(?!\"")(?:-|--)?(\w+)=([^\""\s]*)(?!\"")$", RegexOptions.Compiled);
                Match matchVar = Regex.Match(word, @"^(?!\"")(?:-|--)([a-zA-Z]+)(?!\"")$", RegexOptions.Compiled);
                if (matchIsNamed.Success)
                {
                    string name = matchIsNamed.Groups[1].Value;
                    string value = matchIsNamed.Groups[2].Value;

                    IEnumerable<ICommandParameter> matchingParams =
                        commandParameters.Where(entry => IsRightName(entry, name, CommandBase.IgnoreCommandCase));

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
                                commandParameter.SetStringValue(value, this);
                        }
                        catch(ConverterException ex)
                        {
                            errorDelegate(string.Format("Cannot convert : {0} > {1}", word, ex.Message));
                            return false;
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
                        commandParameters.Where(entry => IsRightName(entry, name, CommandBase.IgnoreCommandCase));

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
                        catch (ConverterException ex)
                        {
                            errorDelegate(string.Format("Cannot convert : {0} > {1}", word, ex.Message));
                            return false;
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
                        next.SetStringValue(word, this);
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

            CommandsParametersByName = definedParam.ToDictionary(entry => entry.Name);
            CommandsParametersByShortName = definedParam.ToDictionary(entry => !string.IsNullOrEmpty(entry.ShortName) ? entry.ShortName : entry.Name);
            return true;
        }

        public bool IsRightName(ICommandParameter parameter, string name, bool useCase)
        {
            return name.Equals(parameter.Name,
                               useCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                   ||
                   name.Equals(parameter.ShortName,
                               useCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
        }
    }
}