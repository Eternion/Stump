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
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.CharacterConverter);
        }

        public Character GetTarget(TriggerBase trigger)
        {
            Character target = null;
            if (trigger.IsArgumentDefined("target"))
                target = trigger.Get<Character>("target");
            else if (trigger is GameTrigger)
                target = ( trigger as GameTrigger ).Character;

            if (target == null)
                throw new Exception("Target is not defined");

            return target;
        }
    }

    public abstract class TargetSubCommand : SubCommand
    {
        protected void AddTargetParameter(bool optional = false, string description = "Defined target")
        {
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.CharacterConverter);
        }

        public Character GetTarget(TriggerBase trigger)
        {
            Character target = null;
            if (trigger.IsArgumentDefined("target"))
                target = trigger.Get<Character>("target");
            else if (trigger is GameTrigger)
                target = ( trigger as GameTrigger ).Character;

            if (target == null)
                throw new Exception("Target is not defined");

            return target;
        }
    }
}