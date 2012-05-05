using System.Diagnostics.Contracts;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightRequest : IRequestBox
    {
        public FightRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        public Character Source
        {
            get;
            private set;
        }

        public Character Target
        {
            get;
            private set;
        }

        public void Open()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyRequestedMessage(Source.Client, Target, Source, Target);
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyRequestedMessage(Target.Client, Source, Source, Target);
        }

        public void Accept()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, true);

            var fight = FightManager.Instance.CreateDuel(Source.Map);

            fight.BlueTeam.AddFighter(Source.CreateFighter(fight.BlueTeam));
            fight.RedTeam.AddFighter(Target.CreateFighter(fight.RedTeam));

            fight.StartPlacement();

            Close();
        }

        public void Deny()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Target, Source, Target, false);

            Close();
        }

        public void Cancel()
        {
            ContextRoleplayHandler.
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Target.Client, Source, Source, Target, false);

            Close();
        }

        private void Close()
        {
            Source.ResetRequestBox();
            Target.ResetRequestBox();
        }
    }
}