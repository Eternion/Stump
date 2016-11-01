using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Quest", typeof (NpcReply), typeof (NpcReplyRecord))]
    public class StartQuestReply
    {
        public StartQuestReply(NpcReplyRecord record)
        {
            
        }
    }
}