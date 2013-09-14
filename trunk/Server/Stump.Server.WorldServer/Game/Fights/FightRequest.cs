using System.Diagnostics.Contracts;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightRequest : RequestBox
    {
        public FightRequest(Character source, Character target)
            : base(source, target)
        {
        }

        protected override void OnOpen()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyRequestedMessage(Source.Client, Target, Source, Target);
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyRequestedMessage(Target.Client, Source, Source, Target);
        }

        protected override void OnAccept()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, true);

            var fight = FightManager.Instance.CreateDuel(Source.Map);

            fight.BlueTeam.AddFighter(Source.CreateFighter(fight.BlueTeam));
            fight.RedTeam.AddFighter(Target.CreateFighter(fight.RedTeam));

            fight.StartPlacement();
        }

        protected override void OnDeny()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, false);
        }

        protected override void OnCancel()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Target.Client, Source, Source, Target, false);
        }
    }
}