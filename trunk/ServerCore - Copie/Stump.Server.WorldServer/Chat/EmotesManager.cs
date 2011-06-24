using System.Collections.Generic;
using System.Linq;
using Stump.Database.Data.Communication;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Chat
{
    public class EmotesManager
    {
        public static Dictionary<uint, EmoticonRecord> Emotes;

        [StageStep(Stages.Two, "Loaded Emotes")]
        public static void Initialize()
        {
            Emotes = EmoticonRecord.FindAll().ToDictionary(entry => entry.Id);
        }
    }
}