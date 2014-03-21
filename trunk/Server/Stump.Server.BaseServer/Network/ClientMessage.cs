using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public class ClientMessage
    {
        private readonly BaseClient m_client;
        private readonly Message m_message;

        public ClientMessage(BaseClient client, Message message)
        {
            m_client = client;
            m_message = message;
        }

        public BaseClient Client
        {
            get { return m_client; }
        }

        public Message Message
        {
            get { return m_message; }
        }
    }
}
