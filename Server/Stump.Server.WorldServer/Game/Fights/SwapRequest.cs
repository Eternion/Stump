using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class SwapRequest : RequestBox
    {
        public SwapRequest(Character source, Character target)
            : base(source, target)
        {
        }

        protected override void OnOpen()
        {
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsOfferMessage(Source.Client, Source, Target);
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsOfferMessage(Target.Client, Source, Target);
        }

        protected override void OnAccept()
        {
           Source.Fighter.SwapPrePlacement(Target.Fighter);
        }

        protected override void OnDeny()
        {
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsCancelledMessage(Source.Client, Source, Target);
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsCancelledMessage(Target.Client, Source, Target);
        }

        protected override void OnCancel()
        {
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsCancelledMessage(Source.Client, Source, Source);
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsCancelledMessage(Target.Client, Source, Source);
        }
    }
}
