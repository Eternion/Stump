using System;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Dialog;
using Stump.Server.WorldServer.Worlds.Notifications;

namespace Stump.Server.WorldServer.Worlds.Parties
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
            Party.DoForAll(entry => PartyHandler.SendPartyRefuseInvitationNotificationMessage(entry.Client, this));
        }

        public void Cancel()
        {
            Target.RemoveInvitation(this);
            Party.RemoveGuest(Target);

            PartyHandler.SendPartyInvitationCancelledForGuestMessage(Target.Client, Source, this);
            Party.DoForAll(entry => PartyHandler.SendPartyCancelInvitationNotificationMessage(entry.Client, this));
        }

        public override void Display()
        {
            PartyHandler.SendPartyInvitationMessage(Target.Client, Party, Source);
        }
    }
}