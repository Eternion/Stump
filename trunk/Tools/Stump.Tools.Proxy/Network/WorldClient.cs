using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Tools.Proxy.Network
{
    public class WorldClient : ProxyClient
    {
        private static readonly Dictionary<string, SelectedServerDataMessage> m_tickets = new Dictionary<string, SelectedServerDataMessage>();
        private FightTeamInformations m_blueTeam;
        private Map m_currentMap;
        private Action m_delegateToCall;
        private EntityDispositionInformations m_disposition;
        private NpcDialogReplyMessage m_guessNpcReply;
        private FightTeamInformations m_redTeam;

        public WorldClient(Socket socket)
            : base(socket)
        {
            MapNpcs = new Dictionary<int, GameRolePlayNpcInformations>();
            MapIOs = new Dictionary<int, InteractiveElement>();
            Actors = new Dictionary<int, GameContextActorInformations>();

            Send(new ProtocolRequired(VersionExtension.ProtocolRequired, VersionExtension.ActualProtocol));
            Send(new HelloGameMessage());
        }

        public string Ticket
        {
            get;
            set;
        }

        public CharacterBaseInformations CharacterInformations
        {
            get;
            set;
        }

        public NpcDialogReplyMessage GuessNpcReply
        {
            get { return m_guessNpcReply; }
            set
            {
                LastNpcReply = m_guessNpcReply;

                m_guessNpcReply = value;
            }
        }

        public NpcDialogReplyMessage LastNpcReply
        {
            get;
            set;
        }

        public NpcMessage LastNpcMessage
        {
            get;
            set;
        }

        public NpcGenericActionRequestMessage GuessNpcFirstAction
        {
            get;
            set;
        }

        public Tuple<Map, InteractiveUseRequestMessage, InteractiveUsedMessage> GuessSkillAction
        {
            get;
            set;
        }

        public bool IsSkillActionValid()
        {
            return HasReceive(InteractiveUsedMessage.Id, 3);
        }

        public Dictionary<int, GameRolePlayNpcInformations> MapNpcs
        {
            get;
            set;
        }

        public Dictionary<int, InteractiveElement> MapIOs
        {
            get;
            set;
        }

        public Map LastMap
        {
            get;
            set;
        }

        public Map CurrentMap
        {
            get { return m_currentMap; }
            set
            {
                LastMap = m_currentMap;

                m_currentMap = value;
            }
        }

        public bool IsInFight
        {
            get;
            set;
        }

        public GameFightJoinMessage Fight
        {
            get;
            set;
        }

        public Dictionary<int, GameContextActorInformations> Actors
        {
            get;
            set;
        }

        public ushort? GuessCellTrigger
        {
            get;
            set;
        }

        public bool IsCellTriggerValid()
        {
            return HasReceive(GameMapMovementConfirmMessage.Id, 3);
        }

        public EntityDispositionInformations Disposition
        {
            get { return m_disposition; }
            set
            {
                m_disposition = value;

                if (m_delegateToCall != null)
                {
                    m_delegateToCall.DynamicInvoke();

                    m_delegateToCall = null;
                }
            }
        }

        public bool GuessAction
        {
            get { return GuessNpcReply != null || GuessNpcFirstAction != null || GuessSkillAction != null || GuessCellTrigger != null; }
        }

        public static void PushTicket(string ticket, SelectedServerDataMessage server)
        {
            m_tickets.Add(ticket, server);
        }

        public static SelectedServerDataMessage PopTicket(string ticket)
        {
            if (!m_tickets.ContainsKey(ticket))
                return null;

            SelectedServerDataMessage result = m_tickets[ticket];

            m_tickets.Remove(ticket);

            return result;
        }

        protected override SocketAsyncEventArgs PopWriteSocketAsyncArgs()
        {
            return Proxy.Instance.WorldClientManager.PopWriteSocketAsyncArgs();
        }

        protected override void PushWriteSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            Proxy.Instance.WorldClientManager.PushWriteSocketAsyncArgs(args);
        }

        protected override SocketAsyncEventArgs PopReadSocketAsyncArgs()
        {
            return Proxy.Instance.WorldClientManager.PopReadSocketAsyncArgs();
        }

        protected override void PushReadSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            Proxy.Instance.WorldClientManager.PushReadSocketAsyncArgs(args);
        }

        protected override bool Dispatch(Message message)
        {
            if (!Proxy.Instance.WorldHandler.IsRegister(message.MessageId))
                return false;

            Proxy.Instance.WorldHandler.Dispatch(this, message);

            return true;
        }

        public void CallWhenTeleported(Action action)
        {
            m_delegateToCall = action;
        }

        public void SendChatMessage(string message)
        {
            SendChatMessage(message, Color.BlueViolet);
        }

        public void SendChatMessage(string message, Color color)
        {
            Send(new ChatServerMessage(
                     (sbyte) ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO,
                     "<font color=\"#" + color.ToArgb().ToString("X") + "\">" + "[PROXY] : " + message + "</font>",
                     DateTime.Now.GetUnixTimeStamp(),
                     "",
                     0,
                     "",
                     0));
        }
    }
}