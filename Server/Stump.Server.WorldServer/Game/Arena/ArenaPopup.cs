using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Notifications;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPopup : Notification
    {
        [Variable] public static int DisplayTime = 60;
        private TimedTimerEntry m_timer;

        public ArenaPopup(ArenaWaitingCharacter character)
        {
            WaitingCharacter = character;
        }


        public ArenaWaitingCharacter WaitingCharacter
        {
            get;
            private set;
        }

        public Character Character
        {
            get { return WaitingCharacter.Character; }
        }

        public ArenaPreFightTeam Team
        {
            get { return WaitingCharacter.Team; }
        }

        public ArenaPreFight Fight
        {
            get { return Team.Fight; }
        }

        public override void Display()
        {
            Character.ArenaPopup = this;
            m_timer = Character.Area.CallDelayed(DisplayTime*1000, Deny);
            ContextHandler.SendGameRolePlayArenaFightPropositionMessage(Character.Client, this, DisplayTime);
            
            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Character.Client, true,
                PvpArenaStepEnum.ARENA_STEP_WAITING_FIGHT, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }

        public void Accept()
        {
            if (m_timer != null)
                m_timer.Dispose();

            WaitingCharacter.ToggleReady(true);
        }

        public void Deny()
        {
            if (m_timer != null)
                m_timer.Dispose();

            WaitingCharacter.DenyFight();
        }
        /*
        private void OnFightDenied(ArenaFight arg1, Character arg2)
        {
            foreach (var allie in Team.GetAlliesInQueue().Where(allie => allie != Character))
            {
                ArenaManager.Instance.AddToQueue(allie);
            }

            if (Character.ArenaParty != null)
            {
                if (!Character.IsPartyLeader(Character.ArenaParty.Id))
                    return;

                ArenaManager.Instance.RemoveFromQueue(Character.ArenaParty);
            }
            else
            {
                ArenaManager.Instance.RemoveFromQueue(Character);
            }
        }*/
    }
}