using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Dialogs;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public interface ITrade : IDialog
    {
        ExchangeTypeEnum ExchangeType
        {
            get;
        }

        Trader FirstTrader
        {
            get;
        }

        Trader SecondTrader
        {
            get;
        }
    }
}