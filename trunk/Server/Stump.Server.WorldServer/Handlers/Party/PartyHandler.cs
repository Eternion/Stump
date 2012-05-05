using System.Diagnostics.Contracts;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Parties;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay.Party
{
    public class PartyHandler : WorldHandlerContainer
    {
        [WorldHandler(PartyInvitationRequestMessage.Id)]
        public static void HandlePartyInvitationRequestMessage(WorldClient client, PartyInvitationRequestMessage message)
        {
            var target = World.Instance.GetCharacter(message.name);

            if (target == null)
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_NOT_FOUND);
                return;
            }

            client.Character.Invite(target);
        }

        [WorldHandler(PartyInvitationDetailsRequestMessage.Id)]
        public static void HandlePartyInvitationDetailsRequestMessage(WorldClient client, PartyInvitationDetailsRequestMessage message)
        {
            var invitation = client.Character.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            SendPartyInvitationDetailsMessage(client, invitation);
        }

        [WorldHandler(PartyAcceptInvitationMessage.Id)]
        public static void HandlePartyAcceptInvitationMessage(WorldClient client, PartyAcceptInvitationMessage message)
        {
            var invitation = client.Character.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            invitation.Accept();
        }

        [WorldHandler(PartyRefuseInvitationMessage.Id)]
        public static void HandlePartyRefuseInvitationMessage(WorldClient client, PartyRefuseInvitationMessage message)
        {
            var invitation = client.Character.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            invitation.Deny();
        }

        [WorldHandler(PartyCancelInvitationMessage.Id)]
        public static void HandlePartyCancelInvitationMessage(WorldClient client, PartyCancelInvitationMessage message)
        {
            if (!client.Character.IsInParty())
                return;

            var guest = client.Character.Party.GetGuest(message.guestId);

            if (guest == null)
                return;

            var invitation = guest.GetInvitation(client.Character.Party.Id);

            if (invitation == null)
                return;

            invitation.Cancel();
        }

        [WorldHandler(PartyLeaveRequestMessage.Id)]
        public static void HandlePartyLeaveRequestMessage(WorldClient client, PartyLeaveRequestMessage message)
        {
            if (!client.Character.IsInParty())
                return;

            // todo : check something ?

            client.Character.LeaveParty();
        }

        [WorldHandler(PartyAbdicateThroneMessage.Id)]
        public static void HandlePartyAbdicateThroneMessage(WorldClient client, PartyAbdicateThroneMessage message)
        {
            if (!client.Character.IsPartyLeader())
                return;

            var member = client.Character.Party.GetMember(message.playerId);

            client.Character.Party.ChangeLeader(member);
        }

        [WorldHandler(PartyKickRequestMessage.Id)]
        public static void HandlePartyKickRequestMessage(WorldClient client, PartyKickRequestMessage message)
        {
            if (!client.Character.IsPartyLeader())
                return;

            var member = client.Character.Party.GetMember(message.playerId);

            client.Character.Party.Kick(member);
        }

        public static void SendPartyKickedByMessage(IPacketReceiver client, Game.Parties.Party party, Character kicker)
        {
            client.Send(new PartyKickedByMessage(party.Id, kicker.Id));
        }

        public static void SendPartyLeaderUpdateMessage(IPacketReceiver client, Game.Parties.Party party, Character leader)
        {
            client.Send(new PartyLeaderUpdateMessage(party.Id, leader.Id));
        }

        public static void SendPartyRestrictedMessage(IPacketReceiver client, Game.Parties.Party party, bool restricted)
        {
            client.Send(new PartyRestrictedMessage(party.Id, restricted));
        }

        public static void SendPartyUpdateMessage(IPacketReceiver client, Game.Parties.Party party, Character member)
        {
            client.Send(new PartyUpdateMessage(party.Id, member.GetPartyMemberInformations()));
        }

        public static void SendPartyNewGuestMessage(IPacketReceiver client, Game.Parties.Party party, Character guest)
        {
            client.Send(new PartyNewGuestMessage(party.Id, guest.GetPartyGuestInformations(party)));
        }

        public static void SendPartyMemberRemoveMessage(IPacketReceiver client, Game.Parties.Party party, Character leaver)
        {
            client.Send(new PartyMemberRemoveMessage(party.Id, leaver.Id));
        }

        public static void SendPartyInvitationCancelledForGuestMessage(IPacketReceiver client, Character canceller, PartyInvitation invitation)
        {
            client.Send(new PartyInvitationCancelledForGuestMessage(invitation.Party.Id, canceller.Id));
        }

        public static void SendPartyCancelInvitationNotificationMessage(IPacketReceiver client, PartyInvitation invitation)
        {
            client.Send(new PartyCancelInvitationNotificationMessage(
                invitation.Party.Id,
                invitation.Source.Id,
                invitation.Target.Id));
        }

        public static void SendPartyRefuseInvitationNotificationMessage(IPacketReceiver client, PartyInvitation invitation)
        {
            client.Send(new PartyRefuseInvitationNotificationMessage(invitation.Party.Id, invitation.Target.Id));
        }

        public static void SendPartyDeletedMessage(IPacketReceiver client, Game.Parties.Party party)
        {
            client.Send(new PartyDeletedMessage(party.Id));
        }

        public static void SendPartyJoinMessage(IPacketReceiver client, Game.Parties.Party party)
        {
            client.Send(new PartyJoinMessage(party.Id,
                (sbyte)party.Type,
                party.Leader.Id,
                Game.Parties.Party.MaxMemberCount,
                party.Members.Select(entry => entry.GetPartyMemberInformations()),
                party.Guests.Select(entry => entry.GetPartyGuestInformations(party)),
                party.Restricted));
        }

        public static void SendPartyInvitationMessage(WorldClient client, Game.Parties.Party party, Character from)
        {
            client.Send(new PartyInvitationMessage(party.Id,
                (sbyte)party.Type,
                Game.Parties.Party.MaxMemberCount,
                from.Id,
                from.Name,
                client.Character.Id // what is that ?
                ));
        }

        public static void SendPartyInvitationDetailsMessage(IPacketReceiver client, PartyInvitation invitation)
        {
            client.Send(new PartyInvitationDetailsMessage(
                invitation.Party.Id,
                (sbyte)invitation.Party.Type,
                invitation.Source.Id,
                invitation.Source.Name,
                invitation.Party.Leader.Id,
                invitation.Party.Members.Select(entry => entry.GetPartyInvitationMemberInformations())
                ));
        }    

        public static void SendPartyCannotJoinErrorMessage(IPacketReceiver client, Game.Parties.Party party, PartyJoinErrorEnum reason)
        {
            client.Send(new PartyCannotJoinErrorMessage(party.Id, (sbyte)reason));
        }

        public static void SendPartyCannotJoinErrorMessage(IPacketReceiver client, PartyJoinErrorEnum reason)
        {
            client.Send(new PartyCannotJoinErrorMessage(0, (sbyte)reason));
        }
    }
}