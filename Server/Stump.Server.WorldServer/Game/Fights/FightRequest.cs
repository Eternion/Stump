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
            ContextRoleplayHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Source.Client, Target, Source, Target);
            ContextRoleplayHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Target.Client, Source, Source, Target);
        }

        protected override void OnAccept()
        {
            if (Source.Map != Target.Map)
            {
                ContextRoleplayHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, false);
                return;
            }

            ContextRoleplayHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, true);

            var fight = FightManager.Instance.CreateDuel(Source.Map);

            fight.DefendersTeam.AddFighter(Source.CreateFighter(fight.DefendersTeam));
            fight.ChallengersTeam.AddFighter(Target.CreateFighter(fight.ChallengersTeam));

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