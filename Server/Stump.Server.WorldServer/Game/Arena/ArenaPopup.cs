using System.Linq;
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
            ContextHandler.SendGameRolePlayArenaFightPropositionMessage(Character.Client, this, 60);
        }

        public void Accept()
        {
            foreach (var allie in Team.GetAlliesInQueue())
            {
                ContextHandler.SendGameRolePlayArenaFighterStatusMessage(allie.Client, Fight.Id, Character, true); 
            }


            Team.ToggleReadyToFight(Character, true);
        }

        public void Deny()
        {
            Fight.DenyFight(Character);
        }

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
        }
    }
}