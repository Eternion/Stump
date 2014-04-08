using ProtoBuf;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC;

namespace ArkalysPlugin.Votes
{    
    internal static class VoteNotificationMessageRegister
    {
        [Initialization(InitializationPass.Last, Silent=true)]
        public static void Initialize()
        {
            IPCMessageSerializer.Instance.RegisterMessage(typeof (VoteNotificationMessage), 101);
        }
    }
    [ProtoContract]
    // better copy the class than reference the auth plugin
    public class VoteNotificationMessage : IPCMessage
    {
        public VoteNotificationMessage()
        {
            
        }
        public VoteNotificationMessage(int[] accountsId)
        {
            AccountsToNotify = accountsId;
        }

        [ProtoMember(2)]
        public int[] AccountsToNotify
        {
            get;
            set;
        }
    }
}