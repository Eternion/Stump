
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class ReportHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (CharacterReportMessage))]
        public static void HandeCharacterReportMessage(WorldClient client, CharacterReportMessage message)
        {
        }
    }
}