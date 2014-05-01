using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Notifications;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPopup : Notification
    {

        private readonly ArenaQueueMember m_member;

        public ArenaPopup(Character character, ArenaQueueMember member)
        {
            Character = character;
            m_member = member;
        }


        public ArenaQueueMember Member
        {
            get { return m_member; }
        }
        public Character Character
        {
            get;
            private set;
        }

        public override void Display()
        {
            Character.ArenaPopup = this;
            ContextHandler.SendGameRolePlayArenaFightPropositionMessage(Character.Client, this);
        }

        public void Accept()
        {
            Member.Team.ToggleReadyToFight(Character, true);
        }

        public void Deny()
        {
        }
    }
}