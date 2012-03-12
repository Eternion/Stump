using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AlignmentCommands : SubCommandContainer
    {
        public AlignmentCommands()
        {
            Aliases = new[] {"alignment", "align"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Provides many commands to manage player alignment";
        }
    }

    public class AlignmentSideCommand : SubCommand
    {
        public AlignmentSideCommand()
        {
            Aliases = new[] { "side" };
            RequiredRole = RoleEnum.Moderator;
            ParentCommand = typeof(AlignmentCommands);
            Description = "Set the alignement side of the given target";
            AddParameter("side", "s", "Alignement side", converter: ParametersConverter.GetEnumConverter<AlignmentSideEnum>());
            AddParameter("target", "t", "Target", isOptional: true, converter:ParametersConverter.CharacterConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            Character target = null;
            if (trigger.IsArgumentDefined("target"))
                target = trigger.Get<Character>("target");
            else if (trigger is GameTrigger)
                target = ( trigger as GameTrigger ).Character;

            if (target == null)
            {
                trigger.ReplyError("Target not defined");
                return;
            }

            target.ChangeAlignementSide(trigger.Get<AlignmentSideEnum>("side"));
        }
    }
}