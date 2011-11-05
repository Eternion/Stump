using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NLog;
using Stump.Core.IO;
using Stump.Core.Pool.Task;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private readonly object m_lock = new object();
        private MessagePart m_currentMessage;


        protected BaseClient(Socket socket)
        {
            Socket = socket;
            IP = ( (IPEndPoint)socket.RemoteEndPoint ).Address.ToString();
        }

        public Socket Socket
        {
            get;
            private set;
        }

        public bool CanReceive
        {
            get;
            protected set;
        }

        public bool Connected
        {
            get { return Socket != null && Socket.Connected; }
        }

        public string IP
        {
            get;
            private set;
        }

        /// <summary>
        /// Last activity as a socket client (last received packet or sent packet)
        /// </summary>
        public DateTime LastActivity
        {
            get;
            private set;
        }

        #region IPacketReceiver Members

        public virtual void Send(Message message)
        {
            if (Socket == null || !Connected)
            {
               logger.Debug("Attempt to send a packet when client isn't connected");
            }

            var args = PopWriteSocketAsyncArgs();

            byte[] data;
            using (var writer = new BigEndianWriter())
            {
                message.Pack(writer);
                data = writer.Data;
            }

            args.SetBuffer(data, 0, data.Length);

            if (!Socket.SendAsync(args))
            {
                ClientManager.Instance.PushWriteSocketAsyncArgs(args);
            }

#if DEBUG
            Console.WriteLine(string.Format("{0} >> {1}", this, message.GetType().Name));
#endif

            LastActivity = DateTime.Now;
            NotifyMessageSended(message);
        }

        protected virtual SocketAsyncEventArgs PopWriteSocketAsyncArgs()
        {
            return ClientManager.Instance.PopWriteSocketAsyncArgs();
        }

        protected virtual void PushWriteSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            ClientManager.Instance.PushWriteSocketAsyncArgs(args);
        }

        protected virtual SocketAsyncEventArgs PopReadSocketAsyncArgs()
        {
            return ClientManager.Instance.PopReadSocketAsyncArgs();
        }

        protected virtual void PushReadSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            ClientManager.Instance.PushReadSocketAsyncArgs(args);
        }

        #endregion

        public event Action<BaseClient, Message> MessageReceived;
        public event Action<BaseClient, Message> MessageSended;

        private void NotifyMessageReceived(Message message)
        {
            OnMessageReceived(message);

            /* A BOUGER/SUPPRIMER (useless ?)
             * Un anti flood se mesure en message/min pas avec l'interval entre deux */

            /* if (MessageListener.MinMessageInterval.HasValue && DateTime.Now.Subtract(LastActivity).TotalMilliseconds < MessageListener.MinMessageInterval)
            {
                Close();
                return;
            }
            if (Settings.InactivityDisconnectionTime.HasValue || MessageListener.MinMessageInterval.HasValue)
                LastActivity = DateTime.Now;
            */

            if (MessageReceived != null)
                MessageReceived(this, message);
        }

        protected virtual void OnMessageReceived(Message message)
        {
            ClientManager.Instance.MessageQueue.Enqueue(this, message);
        }

        private void NotifyMessageSended(Message message)
        {
            if (MessageSended != null)
                MessageSended(this, message);
        }

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            try
            {
                if (!CanReceive)
                    throw new Exception("Cannot receive packet : CanReceive is false");

                lock (m_lock)
                {
                    m_buffer.Add(data, offset, count);

                    BuildMessage();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Forced disconnection " + ToString() + " : " + ex);

                Disconnect();
            }
        }

        protected virtual void BuildMessage()
        {
            if (m_buffer.BytesAvailable <= 0)
                return;

            if (m_currentMessage == null)
                m_currentMessage = new MessagePart();

            // if message is complete
            if (m_currentMessage.Build(m_buffer))
            {
                var messageDataReader = new BigEndianReader(m_currentMessage.Data);
                Message message = MessageReceiver.BuildMessage((uint) m_currentMessage.MessageId.Value, messageDataReader);

                LastActivity = DateTime.Now;
                NotifyMessageReceived(message);

                m_currentMessage = null;

                BuildMessage(); // there is maybe a second message in the buffer
            }
        }

        /// <summary>
        ///   Disconnect the Client. Cannot reuse the socket.
        /// </summary>
        public void Disconnect()
        {
            if (!Connected)
                return;

            try
            {
                OnDisconnect();
            }
            catch (Exception ex)
            {
                logger.Error("Exception occurs while disconnecting client {0} : {1}", ToString(), ex);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        ///   Disconnect the Client after a time
        /// </summary>
        /// <param name = "timeToWait"></param>
        public void DisconnectLater(int timeToWait = 0)
        {
            if (timeToWait == 0)
                ServerBase.InstanceAsBase.IOTaskPool.EnqueueTask(Disconnect);
            else
                ServerBase.InstanceAsBase.CyclicTaskPool.RegisterCyclicTask(Disconnect, timeToWait, callsLimit:1);
        }

        protected virtual void OnDisconnect()
        {
        }

        protected void Close()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Socket = null;
            }
        }

        public override string ToString()
        {
            return string.Concat("<", IP, ">");
        }
    }
}