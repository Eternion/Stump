using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AlignmentCommands : SubCommandContainer
    {
        public AlignmentCommands()
        {
            Aliases = new[] {"alignment", "align"};
            RequiredRole = RoleEnum.GameMaster;
            Description = "Provides many commands to manage player alignment";
        }
    }

    public class AlignmentSideCommand : TargetSubCommand
    {
        public AlignmentSideCommand()
        {
            Aliases = new[] { "side" };
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof(AlignmentCommands);
            Description = "Set the alignement side of the given target";
            AddParameter("side", "s", "Alignement side", converter: ParametersConverter.GetEnumConverter<AlignmentSideEnum>());
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach(var target in GetTargets(trigger))
                target.ChangeAlignementSide(trigger.Get<AlignmentSideEnum>("side"));
        }
    }
}