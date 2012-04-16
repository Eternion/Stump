using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
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

    public class AlignmentSideCommand : TargetSubCommand
    {
        public AlignmentSideCommand()
        {
            Aliases = new[] { "side" };
            RequiredRole = RoleEnum.Moderator;
            ParentCommand = typeof(AlignmentCommands);
            Description = "Set the alignement side of the given target";
            AddParameter("side", "s", "Alignement side", converter: ParametersConverter.GetEnumConverter<AlignmentSideEnum>());
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            Character target = GetTarget(trigger);

            target.ChangeAlignementSide(trigger.Get<AlignmentSideEnum>("side"));
        }
    }
}