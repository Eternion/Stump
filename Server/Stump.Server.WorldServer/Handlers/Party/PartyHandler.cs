using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Arena;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Parties;
using Stump.Server.WorldServer.Handlers.Basic;

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

            if (target.FriendsBook.IsIgnored(client.Account.Id))
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_BUSY);
                return;
            }

            if (target.IsAway && !target.FriendsBook.IsFriend(client.Account.Id))
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_BUSY);
                return;
            }

            client.Character.Invite(target, PartyTypeEnum.PARTY_TYPE_CLASSICAL);
        }

        [WorldHandler(PartyInvitationArenaRequestMessage.Id)]
        public static void HandlePartyInvitationArenaRequestMessage(WorldClient client, PartyInvitationArenaRequestMessage message)
        {
            var target = World.Instance.GetCharacter(message.name);

            if (target == null)
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_NOT_FOUND);
                return;
            }

            if (target.IsAway)
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_BUSY);
                return;
            }

            if (ArenaManager.Instance.IsInQueue(client.Character) || client.Character.ArenaPopup != null || (client.Character.ArenaParty != null && ArenaManager.Instance.IsInQueue(client.Character.ArenaParty)))
            {
                //Vous ne pouvez pas inviter %1 en groupe de Kolizéum car vous êtes en préparation d'un combat de Kolizéum.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 353, target.Name);
                return;
            }

            if (client.Character.Fight is ArenaFight)
            {
                //Vous êtes déjà en combat de Kolizéum.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 334);
                SendPartyCannotJoinErrorMessage(client, client.Character.ArenaParty, PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNMODIFIABLE);
                return;
            }

            if (target.ArenaPopup != null || target.Fight is ArenaFight)
            {
                //%1 est déjà dans un combat de Kolizéum.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 335, target.Name);
                return;
            }

            client.Character.Invite(target, PartyTypeEnum.PARTY_TYPE_ARENA);
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
            if (!client.Character.IsInParty(message.partyId))
                return;

            var guest = client.Character.GetParty(message.partyId).GetGuest(message.guestId);

            if (guest == null)
                return;

            var invitation = guest.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            invitation.Cancel();
        }

        [WorldHandler(PartyLeaveRequestMessage.Id)]
        public static void HandlePartyLeaveRequestMessage(WorldClient client, PartyLeaveRequestMessage message)
        {
            if (!client.Character.IsInParty(message.partyId))
                return;

            client.Character.LeaveParty(client.Character.GetParty(message.partyId));
        }

        [WorldHandler(PartyAbdicateThroneMessage.Id)]
        public static void HandlePartyAbdicateThroneMessage(WorldClient client, PartyAbdicateThroneMessage message)
        {
            if (!client.Character.IsInParty())
                return;

            if (!client.Character.IsPartyLeader(message.partyId))
                return;

            var member = client.Character.GetParty(message.partyId).GetMember(message.playerId);

            client.Character.GetParty(message.partyId).ChangeLeader(member);
        }

        [WorldHandler(PartyKickRequestMessage.Id)]
        public static void HandlePartyKickRequestMessage(WorldClient client, PartyKickRequestMessage message)
        {
            if (!client.Character.IsInParty())
                return;

            if (!client.Character.IsPartyLeader(message.partyId))
                return;

            var member = client.Character.GetParty(message.partyId).GetMember(message.playerId);

            client.Character.GetParty(message.partyId).Kick(member);
        }

        [WorldHandler(PartyFollowMemberRequestMessage.Id)]
        public static void HandlePartyFollowMemberRequestMessage(WorldClient client, PartyFollowMemberRequestMessage message)
        {
            if (!client.Character.IsInParty(message.partyId))
                return;

            var target = client.Character.Party.GetMember(message.playerId);

            if (target == null)
                return;

            client.Character.FollowMember(target);
        }

        [WorldHandler(PartyFollowThisMemberRequestMessage.Id)]
        public static void HandlePartyFollowThisMemberRequestMessage(WorldClient client, PartyFollowThisMemberRequestMessage message)
        {
            if (!client.Character.IsPartyLeader(message.partyId))
                return;

            var target = client.Character.Party.GetMember(message.playerId);

            if (target == null)
                return;

            foreach(var member in target.Party.Members)
            {
                if (message.enabled)
                    member.FollowMember(target);
                else
                    member.UnfollowMember();
            } 
        }

        [WorldHandler(PartyStopFollowRequestMessage.Id)]
        public static void HandlePartyStopFollowRequestMessage(WorldClient client, PartyStopFollowRequestMessage message)
        {
            if (!client.Character.IsInParty(message.partyId))
                return;

            client.Character.UnfollowMember();
        }

        public static void SendPartyFollowStatusUpdateMessage(WorldClient client, Game.Parties.Party party, bool success, int followedId)
        {
            client.Send(new PartyFollowStatusUpdateMessage(party.Id, success, followedId));
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
            client.Send(new PartyUpdateMessage(party.Id, party.GetPartyMemberInformations(member)));
        }

        public static void SendPartyMemberInFightMessage(IPacketReceiver client, Game.Parties.Party party, Character member, PartyFightReasonEnum reason, IFight fight)
        {
            client.Send(new PartyMemberInFightMessage(party.Id, (sbyte)reason, member.Id, member.Account.Id, member.Name, fight.Id, 
                new MapCoordinatesExtended((short)fight.Map.Position.X, (short)fight.Map.Position.Y, fight.Map.Id, (short)fight.Map.SubArea.Id), fight.GetPlacementTimeLeft()));
        }

        public static void SendPartyNewMemberMessage(IPacketReceiver client, Game.Parties.Party party, Character member)
        {
            client.Send(new PartyNewMemberMessage(party.Id, party.GetPartyMemberInformations(member)));
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
                (sbyte)party.MembersLimit,
                party.Members.Select(party.GetPartyMemberInformations),
                party.Guests.Select(party.GetPartyGuestInformations),
                party.Restricted));
        }

        public static void SendPartyInvitationMessage(WorldClient client, Game.Parties.Party party, Character from)
        {
            client.Send(new PartyInvitationMessage(party.Id,
                (sbyte)party.Type,
                (sbyte)party.MembersLimit,
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
                invitation.Party.Members.Select(entry => entry.GetPartyInvitationMemberInformations()),
                invitation.Party.Guests.Select(invitation.Party.GetPartyGuestInformations)
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