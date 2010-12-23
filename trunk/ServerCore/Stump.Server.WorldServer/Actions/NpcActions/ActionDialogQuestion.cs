using System;

namespace Stump.Server.WorldServer.Actions.NpcActions
{
    public class ActionDialogQuestion : NpcAction
    {
        public ActionDialogQuestion(NpcActionArgument argument)
            : base(argument)
        {
        }

        public override void Execute()
        {
            var questionId = Argument.Next<uint>();

            // todo : get the associact NpcQuestion
        }
    }
}