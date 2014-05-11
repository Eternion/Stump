using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Notifications;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPopup : Notification
    {
        public ArenaPopup(Character character, ArenaTeam team)
        {
            Character = character;
            Team = team;
            Fight.FightDenied += OnFightDenied;
        }


        public Character Character
        {
            get;
            private set;
        }

        public ArenaTeam Team
        {
            get;
            set;
        }

        public ArenaFight Fight
        {
            get { return Team.Fight as ArenaFight; }
        }

        public override void Display()
        {
            Character.ArenaPopup = this;
            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Character.Client, false, 
                PvpArenaStepEnum.ARENA_STEP_WAITING_FIGHT, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
            ContextHandler.SendGameRolePlayArenaFightPropositionMessage(Character.Client, this, 60);
        }

        public void Accept()
        {
            Team.ToggleReadyToFight(Character, true);
        }

        public void Deny()
        {
            Fight.DenyFight(Character);
        }

        private void OnFightDenied(ArenaFight arg1, Character arg2)
        {
            if (Character.ArenaParty != null)
            {
                if (Character.IsPartyLeader(Character.ArenaParty.Id))
                    ArenaManager.Instance.AddToQueue(Character.ArenaParty);
            }
            else
                ArenaManager.Instance.AddToQueue(Character);
        }
    }
}