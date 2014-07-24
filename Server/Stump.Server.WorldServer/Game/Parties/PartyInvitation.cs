using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Notifications;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;

namespace Stump.Server.WorldServer.Game.Parties
{
    public class PartyInvitation : Notification
    {
        public PartyInvitation(Party party, Character source, Character target)
        {
            Party = party;
            Source = source;
            Target = target;
        }

        public Party Party
        {
            get;
            private set;
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

        public void Accept()
        {
            Target.EnterParty(Party);
        }

        public void Deny()
        {
            Target.RemoveInvitation(this);
            Party.RemoveGuest(Target);

            PartyHandler.SendPartyInvitationCancelledForGuestMessage(Target.Client, Target, this);
            PartyHandler.SendPartyRefuseInvitationNotificationMessage(Party.Clients, this);
        }

        public void Cancel()
        {
            Target.RemoveInvitation(this);
            Party.RemoveGuest(Target);

            PartyHandler.SendPartyInvitationCancelledForGuestMessage(Target.Client, Source, this);
            PartyHandler.SendPartyCancelInvitationNotificationMessage(Party.Clients, this);
        }

        public override void Display()
        {
            PartyHandler.SendPartyInvitationMessage(Target.Client, Party, Source);
        }
    }
}