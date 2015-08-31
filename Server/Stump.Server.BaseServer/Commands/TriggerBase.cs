using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class TriggerBase
    {
        private readonly Regex m_regexIsNamed = new Regex(@"^(?!\"")(?:-|--)?([\w\d]+)=(.*)$", RegexOptions.Compiled);
        private readonly Regex m_regexVar = new Regex(@"^(?!\"")(?:-|--)([\w\d]+)(?!\"")$", RegexOptions.Compiled);

        private TriggerBase()
        {
            CommandsParametersByName = new Dictionary<string, IParameter>();
            CommandsParametersByShortName = new Dictionary<string, IParameter>();
        }

        protected TriggerBase(StringStream args, RoleEnum userRole)
            : this()
        {
            Args = args;
            UserRole = userRole;
        }

        protected TriggerBase(string args, RoleEnum userRole)
            : this(new StringStream(args), userRole)
        {

        }

        public abstract ICommandsUser User
        {
            get;
        }

        public StringStream Args
        {
            get;
            private set;
        }

        public virtual RoleEnum UserRole
        {
            get;
            private set;
        }

        public CommandBase BoundCommand
        {
            get;
            private set;
        }

        public abstract bool CanFormat
        {
            get;
        }

        internal Dictionary<string, IParameter> CommandsParametersByName
        {
            get;
            private set;
        }

        internal Dictionary<string, IParameter> CommandsParametersByShortName
        {
            get;
            private set;
        }

        public virtual bool CanAccessCommand(CommandBase command)
        {
            return command.RequiredRole <= UserRole;
        }

        public abstract void Log();

        /// <summary>
        ///   Replies accordingly with the given text.
        /// </summary>
        public abstract void Reply(string text);

        public void Reply(string format, params object[] args)
        {
            Reply(string.Format(format, args));
        }

        public void ReplyBold(string format, params object[] args)
        {
            Reply(string.Format(format, args.Select(Bold).ToArray()));
        }

        public virtual void ReplyError(string message)
        {
            if (!CanFormat)
                Reply("(Error) " + message);
            else
                Reply(Bold("(Error)") + " " + message);
        }

        public void ReplyError(string format, params object[] args)
        {
            ReplyError(string.Format(format, args));
        }

        public string Bold(object obj)
        {
            return Bold(obj.ToString());
        }

        public string Bold(string message)
        {
            if (!CanFormat)
                return message; 

            return "<b>" + message + "</b>";
        }

        public string Underline(object obj)
        {
            return Underline(obj.ToString());
        }

        public string Underline(string message)
        {
            if (!CanFormat)
                return message;

            return "<u>" + message + "</u>";
        }

        public string Italic(object obj)
        {
            return Italic(obj.ToString());
        }

        public string Italic(string message)
        {
            if (!CanFormat)
                return message;

            return "<i>" + message + "</i>";
        }

        public string Color(object obj, Color color)
        {
            return Color(obj.ToString(), color);
        }

        public string Color(string message, Color color)
        {
            if (!CanFormat)
                return message;

            return "<font color=\"#" + color.ToArgb().ToString("X") + "\">" + message + "</font>";
        }

        public virtual T Get<T>(string name)
        {
            if (CommandsParametersByName.ContainsKey(name))
                return (T) CommandsParametersByName[name].Value;
            if (CommandsParametersByShortName.ContainsKey(name))
                return (T) CommandsParametersByShortName[name].Value;

            throw new ArgumentException("'" + name + "' is not an existing parameter");
        }

        /// <summary>
        /// Returns true only if the argument as been set by the user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool IsArgumentDefined(string name)
        {
            if (CommandsParametersByName.ContainsKey(name))
            {
                return CommandsParametersByName[name].IsDefined;
            }
            if (CommandsParametersByShortName.ContainsKey(name))
            {
                return CommandsParametersByShortName[name].IsDefined;
            }

            return false;
        }

        public abstract BaseClient GetSource();

        public int RegisterException(Exception ex)
        {
            User.CommandsErrors.Add(new KeyValuePair<string, Exception>(Args.String, ex));

            return User.CommandsErrors.Count - 1;
        }

        /// <summary>
        /// Bind the trigger to a command instance and initialize his parameters. Returns false whenever an error occurs during the initialization
        /// </summary>
        public bool BindToCommand(CommandBase command)
        {
            BoundCommand = command;

            if (command is SubCommandContainer) // SubCommandContainer has no params
                return true;

            var definedParam = new List<IParameter>();
            var paramToDefine = new List<IParameterDefinition>(BoundCommand.Parameters);

            // command has only a string parameter -> then we can assign the entire string to this parameter
            if (paramToDefine.Count == 1 && paramToDefine[0].ValueType == typeof(string) && !paramToDefine[0].IsOptional)
            {
                var param = paramToDefine[0].CreateParameter();

                param.SetValue(Args.NextWords(), this);

                definedParam.Add(param);
                paramToDefine.Remove(paramToDefine[0]);
            }

            if (BoundCommand.Parameters.Count == 0)
                return true;

            var word = Args.NextWord();
            var definedOnly = false;
            while (!string.IsNullOrEmpty(word) && definedParam.Count < BoundCommand.Parameters.Count)
            {
                if (word.StartsWith("\"") && word.EndsWith("\""))
                    word = word.Remove(word.Length - 1, 1).Remove(0, 1);

                var parsed = false;
                if (word.StartsWith("-")) // becareful it can be the minus sign
                {
                    string name = null;
                    string value = null;
                    var matchIsNamed = m_regexIsNamed.Match(word);
                    if (matchIsNamed.Success)
                    {
                        name = matchIsNamed.Groups[1].Value;
                        value = matchIsNamed.Groups[2].Value;

                        if (value.StartsWith("\"") && value.EndsWith("\""))
                            value = value.Remove(value.Length - 1, 1).Remove(0, 1);
                    }
                    else
                    {
                        var matchVar = m_regexVar.Match(word);
                        if (matchVar.Success)
                        {
                            name = matchVar.Groups[1].Value;
                            value = string.Empty;
                            definedOnly = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(name)) // if one of both regex success
                    {
                        var definition =
                            paramToDefine.SingleOrDefault(entry => CompareParameterName(entry, name, CommandBase.IgnoreCommandCase));

                        if (definition != null)
                        {
                            var parameter = definition.CreateParameter();

                            try
                            {
                                // parameters defined like "-life" imply being true
                                // writting "-life" is similar to "-life=true"
                                if (definedOnly && definition.ValueType == typeof (bool))
                                    value = "true";

                                parameter.SetValue(value, this);
                            }
                            catch (ConverterException ex)
                            {
                                ReplyError(ex.Message);
                                return false;
                            }
                            catch (Exception ex)
                            {
                                ReplyError("Cannot parse : {0} as {1} (error-index:{2})", word, definition.ValueType, RegisterException(ex));
                                return false;
                            }

                            definedParam.Add(parameter);
                            paramToDefine.Remove(definition);
                            parsed = true;
                        }
                    }
                }


                if (!parsed)
                {
                    IParameterDefinition definition = paramToDefine.First();

                    IParameter parameter = definition.CreateParameter();

                    try
                    {
                        parameter.SetValue(word, this);
                    }
                    catch (ConverterException ex)
                    {
                        ReplyError(ex.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        ReplyError("Cannot parse : {0} as {1} (error-index:{2})", word, definition.ValueType, RegisterException(ex));
                        return false;
                    }

                    definedParam.Add(parameter);
                    paramToDefine.Remove(definition);
                }

                word = Args.NextWord();
            }


            foreach (var unusedDefinition in paramToDefine)
            {
                if (!unusedDefinition.IsOptional)
                {
                    ReplyError("{0} is not defined", unusedDefinition.Name);
                    return false;
                }

                var parameter = unusedDefinition.CreateParameter();

                parameter.SetDefaultValue(this);
                definedParam.Add(parameter);
            }

            CommandsParametersByName = definedParam.ToDictionary(entry => entry.Definition.Name);
            CommandsParametersByShortName = definedParam.ToDictionary(entry =>
                    !string.IsNullOrEmpty(entry.Definition.ShortName) ?
                        entry.Definition.ShortName : entry.Definition.Name);
            return true;
        }

        public static bool CompareParameterName(IParameterDefinition parameter, string name, bool useCase)
        {
            return name.Equals(parameter.Name,
                               useCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                   || name.Equals(parameter.ShortName,
                               useCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
        }
    }
}