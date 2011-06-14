
using System;
using System.Linq;
using System.Collections.Generic;
using Squishy.Irc;
using Squishy.Irc.Commands;
using Squishy.Irc.Dcc;
using Squishy.Irc.Protocol;
using Squishy.Network;

namespace Stump.Tools.UtilityBot
{
    public class IrcConnection : IrcClient
    {
        public static bool AnnounceAuthToUser = true;
        public static bool AutoAuth = true;
        public static bool AutomaticTopicUpdating = true;
        public static bool HideChatting;
        public static bool HideIncomingIrcPackets;
        public static bool HideOutgoingIrcPackets;
        public static bool ReConnectOnDisconnect = true;
        public static int ReConnectWaitTime = 50;
        public static int ReConnectAttempts = 100; // If 0, attempts won't be limited
        public static bool ReJoinOnKick = true;
        public static bool ReplyOnUnknownCommandUsed = true;
        public static bool AuthAllUsersOnJoin;
        public static bool UpdateTopicOnFlagAdded = true;
        public static int ExceptionNotificationRank = 1000;
        public static bool ExceptionNotify = true;
        public static bool ExceptionChannelNotification;
        public static bool ExceptionNotifyStaffUsers = true;
        public static bool EchoBroadcasts = true;

        public IrcConnection()
        {
            WatchedChannels = new List<string>();
            // add a packet handler
            protHandler.PacketReceived += OnReceive;

            // DCC events
            Dcc.RequestReceived += OnDcc;
            Dcc.SendRequested += OnDccSendRequest;
            Dcc.ChatRequest += OnDccChatRequest;
            Dcc.InvalidRequest += OnDccInvalid;
            Dcc.ListenerTimeout += OnDccListenerTimeout;
            Dcc.ListenerFailed += OnDccListenerFailed;
            Dcc.TransferEstablished += OnTransferReady;
            Dcc.ReceiveTimeout += OnDccReceiveTimeout;
            Dcc.TransferFailed += OnTransferFailed;
            Dcc.TransferDone += OnTransferDone;
            Dcc.ChatEstablished += OnChatEstablished;
            Dcc.ChatMessageReceived += OnChatMsgReceived;
            Dcc.ChatClosed += OnChatClosed;
            Dcc.ChatFailed += OnChatFailed;
            Dcc.BytesTransferred += OnTransfer;

            IrcCommandHandler.Initialize();
        }

        public IrcConnection(List<string> watchedChannels)
            : this()
        {
            WatchedChannels = watchedChannels;
        }

        public IrcConnection(List<string> watchedChannels, string commandPrefix)
            : this()
        {
            WatchedChannels = watchedChannels;
            CommandHandler.RemoteCommandPrefix = commandPrefix;
        }

        public List<string> WatchedChannels
        {
            get;
            private set;
        }


        protected override void Perform()
        {
            foreach (string chan in WatchedChannels)
            {
                CommandHandler.Join(chan);
            }
        }

        protected override void OnExceptionRaised(Exception e)
        {
            Console.WriteLine(e);
        }

        protected override void OnConnecting()
        {
            Console.WriteLine("Connecting to {0}:{1} ...", Client.RemoteAddress, Client.RemotePort);
        }

        protected override void OnConnected()
        {
            Console.WriteLine("Connected.");
        }

        protected override void OnDisconnected(bool conLost)
        {
            Console.WriteLine("Disconnected" + (conLost ? " (Connection lost)" : ""));
        }

        protected override void OnConnectFail(Exception ex)
        {
            Console.WriteLine("Connection failed: " + ex);
            Console.WriteLine("Trying to reconnect in {0}", ReConnectWaitTime);
        }

        protected void OnReceive(IrcPacket packet)
        {
            Console.WriteLine("<-- " + packet);
        }

        protected override void OnBeforeSend(string text)
        {
            Console.WriteLine("--> " + text);
        }


        protected override void OnJoin(IrcUser user, IrcChannel chan)
        {
            Console.WriteLine(String.Format("{0} joined {1}", user.Nick, chan.Name));
        }

        /// <summary>
        ///   Return wether or not the given trigger may be processed.
        ///   Default: Only allows if local or no user triggered it.
        /// </summary>
        /// <param name = "trigger"></param>
        /// <returns></returns>
        public override bool MayTriggerCommand(CmdTrigger trigger, Command cmd)
        {
            if (base.MayTriggerCommand(trigger, cmd))
            {
                // default case (No User set (meaning its not a remote request) amongst others)
                return Bot.AllowedUserNicks.Count(entry => entry == trigger.User.Nick) > 0;
            }

            return Bot.AllowedUserNicks.Count(entry => entry == trigger.User.Nick) > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name = "user"></param>
        /// <param name = "chan"></param>
        /// <param name = "text"></param>
        /// <param name = "initial">Whether this is the initial topic (sent when first joining the channel) or whether the topic has been changed
        ///   by the given user</param>
        protected override void OnTopic(IrcUser user, IrcChannel chan, string text, bool initial)
        {
            if (initial)
            {
                Console.WriteLine("Topic is: {0}", text);
            }
            else
            {
                Console.WriteLine("{0} changed topic to: {1}", user, text);
            }
        }

        protected override void OnCommandFail(CmdTrigger trigger, Exception ex)
        {
            Command cmd = trigger.Command;
            string[] lines = ex.ToString().Split(new[] {"\r\n|\n|\r"}, StringSplitOptions.RemoveEmptyEntries);

            trigger.Reply("Exception raised: " + lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                // TODO: automatically detect lines before sending in Client-class
                trigger.Reply(lines[i]);
            }
            trigger.Reply("Uncorrect usage - " + cmd.Usage + ": " + cmd.Description);
        }

        protected override void OnUsersAdded(IrcChannel chan, IrcUser[] users)
        {
            Console.WriteLine("Topic was set {0} by {1}", chan.TopicSetTime, chan.TopicSetter);
        }

        protected override void OnFlagDeleted(IrcUser user, IrcChannel chan, Privilege priv, IrcUser target)
        {
            //Window.Active.WriteLine(user.Nick);
        }

        private static void OnDcc(IrcUser user, string command, string[] args)
        {
            Console.WriteLine("Dcc: {0} -> {1} {2}", user.Nick, command, Util.GetWords(args, 0));
        }

        private static void OnDccInvalid(IrcUser user, string command, string[] args)
        {
            Console.WriteLine("Dcc invalid: {0} -> {1} - {2}", user.Nick, command, Util.GetWords(args, 0));
        }

        private static void OnDccSendRequest(DccReceiveArgs info)
        {
            Console.WriteLine("Dcc send request: {0} wants to send \"{1}\" ({2} kb) from {3}", info.User.Nick,
                              info.FileName, (info.Size/1024).ToString("#00.00"), info.RemoteEndPoint);
            //info.DestinationDir = "D:\\Downloads";
            info.DestinationFile = info.FileName.Replace("_", " ");
            //info.Timeout = TimeSpan.FromMinutes(5);
            //info.Accept = false;
        }

        private static void OnDccChatRequest(DccChatArgs info)
        {
            Console.WriteLine("Dcc chat request: {0} wants to chat with you from {1}", info.User.Nick,
                              info.RemoteEndPoint);
            //info.Timeout = TimeSpan.FromMinutes(5);
            //info.Accept = false;
        }

        private static void OnTransfer(DccTransferClient client, int amount)
        {
        }

        #region Dcc Event Handling

        private static void OnDccListenerTimeout(DccListener serv)
        {
            Console.WriteLine("Dcc listener for \"{0}\" got a timeout (port:{1})", serv.Client.User,
                              serv.LocalEndPoint.Port);
        }

        private static void OnDccListenerFailed(DccListener serv, Exception ex)
        {
            Console.WriteLine("Dcc listener for the {0}connection with \"{1}\" (port:{2}) failed: {3}",
                              serv.Client is DccSendClient ? "Transfer" : "Chat", serv.Client.User,
                              serv.LocalEndPoint.Port,
                              ex);
        }

        private static void OnDccReceiveTimeout(DccReceiveClient client)
        {
            Console.WriteLine("Dcc receive timeout: {0}", client.File.Name);
        }

        private static void OnTransferReady(DccTransferClient client)
        {
            Console.WriteLine("Dcc transfer client ready: pos: {0}, client: {1}", client.StartPosition,
                              client.RemoteEndPoint);
        }

        private static void OnTransferDone(DccTransferClient client)
        {
            Console.WriteLine("Dcc transfer done: {0}, Speed: {1} kb/str", client.File.Name, client.BytesPerSecond/1024);
        }

        private static void OnTransferFailed(DccTransferClient client, Exception ex)
        {
            Console.WriteLine("Dcc transfer failed: {0} - {1}", client.File.Name, ex);
        }

        private static void OnChatEstablished(DccChatClient client)
        {
            Console.WriteLine("Dcc chat client with \"{0}\" ready", client.User);
        }

        private static void OnChatFailed(DccChatClient client, Exception ex)
        {
            Console.WriteLine("Dcc chat with {0} failed: {1}", client.User, ex);
        }

        private static void OnChatMsgReceived(DccChatClient client, StringStream text)
        {
            Console.WriteLine("Dcc chatmsg from {0}: {1}", client.User, text);
        }

        private static void OnChatClosed(DccChatClient client)
        {
            Console.WriteLine("Dcc chat with {0} closed", client.User);
        }

        #endregion
    }
}