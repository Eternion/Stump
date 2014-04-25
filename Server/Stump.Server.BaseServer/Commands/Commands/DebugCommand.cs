using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using CSScriptLibrary;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class DebugCommand : SubCommandContainer
    {
        public DebugCommand()
        {
            Aliases = new[] { "debug" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Provides command to debug things";
        }
    }

    public class CommandsExceptions : SubCommand
    {
        public CommandsExceptions()
        {
            Aliases = new[] {"cmderror"};
            ParentCommandType = typeof (DebugCommand);
            RequiredRole = RoleEnum.Administrator;
            Description = "Give command error details";
            AddParameter<int>("index", "i", "Error index (last if not defined)", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            int index;

            if (!trigger.IsArgumentDefined("index"))
                index = trigger.User.CommandsErrors.Count - 1; // last index
            else index = trigger.Get<int>("index");

            if (trigger.User.CommandsErrors.Count <= index)
            {
                trigger.ReplyError("No error at index {0}", index);
                return;
            }

            var pair = trigger.User.CommandsErrors[index];

            trigger.Reply("Command : " + pair.Key);
            trigger.Reply("Exception : ");

            foreach (var line in pair.Value.ToString().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                trigger.Reply(line);
            }
        }
    }

    public class CommandGlobalExceptions : SubCommand
    {
        public CommandGlobalExceptions()
        {
            Aliases = new[] { "error" };
            ParentCommandType = typeof(DebugCommand);
            RequiredRole = RoleEnum.Administrator;
            Description = "Give error details";
            AddParameter<int>("index", "i", "Error index (last if not defined)", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            int index;

            if (!trigger.IsArgumentDefined("index"))
                index = trigger.User.CommandsErrors.Count - 1; // last index
            else index = trigger.Get<int>("index");

            if (trigger.User.CommandsErrors.Count <= index)
            {
                trigger.ReplyError("No error at index {0}", index);
                return;
            }

            var pair = trigger.User.CommandsErrors[index];

            trigger.Reply("Command : " + pair.Key);
            trigger.Reply("Exception : ");

            foreach (var line in pair.Value.ToString().Split(new [] {'\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                trigger.Reply(line);
            }
        }
    }

    /*public class CommandRunCode : SubCommand
    {
        public CommandRunCode()
        {
            Aliases = new[] { "code", "exec" };
            ParentCommand = typeof(DebugCommand);
            RequiredRole = RoleEnum.Administrator;
            Description = "Execute a code dynamically";
            AddParameter<string>("code", "code", "The code to execute");
        }

        public override void Execute(TriggerBase trigger)
        {
            CSScript.AssemblyResolvingEnabled = true;
            var eval = CSScript.BuildEval(string.Format("func() {{ {0}; }}", trigger.Get<string>("code")));
            
            eval();
            trigger.Reply("Executed");
        }
    }

    public class CommandEval : SubCommand
    {
        public CommandEval()
        {
            Aliases = new[] { "eval" };
            ParentCommand = typeof(DebugCommand);
            RequiredRole = RoleEnum.Administrator;
            Description = "Eval a code";
            AddParameter<string>("code", "code", "The code to eval");
        }

        public override void Execute(TriggerBase trigger)
        {
            CSScript.AssemblyResolvingEnabled = true;
            var eval = CSScript.BuildEval(string.Format("func() {{ return {0}; }}", trigger.Get<string>("code")));

            trigger.Reply(ObjectDumper.Dump(eval()));
        }
    }*/
}