using System;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands.Patterns
{
    public abstract class TargetCommand : CommandBase
    {
        protected void AddTargetParameter(bool optional = false, string description = "Defined target")
        {
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.CharactersConverter);
        }

        public Character[] GetTargets(TriggerBase trigger)
        {
            Character[] targets = null;
            if (trigger.IsArgumentDefined("target"))
                targets = trigger.Get<Character[]>("target");
            else if (trigger is GameTrigger)
                targets = new []{(trigger as GameTrigger).Character};

            if (targets == null)
                throw new Exception("Target is not defined");

            if (targets.Length == 0)
                throw new Exception("No target found");

            return targets;
        }

        public Character GetTarget(TriggerBase trigger)
        {
            var targets = GetTargets(trigger);

            if (targets.Length > 1)
                throw new Exception("Only 1 target allowed");

            return targets[0];
        }
    }

    public abstract class TargetSubCommand : SubCommand
    {
        protected void AddTargetParameter(bool optional = false, string description = "Defined target")
        {
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.CharactersConverter);
        }

        public Character[] GetTargets(TriggerBase trigger)
        {
            Character[] targets = null;
            if (trigger.IsArgumentDefined("target"))
                targets = trigger.Get<Character[]>("target");
            else if (trigger is GameTrigger)
                targets = new []{(trigger as GameTrigger).Character};

            if (targets == null)
                throw new Exception("Target is not defined");

            if (targets.Length == 0)
                throw new Exception("No target found");

            return targets;
        }

        public Character GetTarget(TriggerBase trigger)
        {
            var targets = GetTargets(trigger);

            if (targets.Length > 1)
                throw new Exception("Only 1 target allowed");

            return targets[0];
        }
    }
}