using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Dialogs;

namespace Stump.Server.WorldServer.Worlds.Fights
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

            var fight = FightManager.Instance.Create(Source.Map, FightTypeEnum.FIGHT_TYPE_CHALLENGE);

            fight.BlueTeam.AddFighter(Source.JoinFight(fight.BlueTeam));
            fight.RedTeam.AddFighter(Target.JoinFight(fight.RedTeam));

            fight.StartPlacementPhase();

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
                SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Source, Source, Target, false);

            Close();
        }

        private void Close()
        {
            Source.ResetRequestBox();
            Target.ResetRequestBox();
        }
    }
}