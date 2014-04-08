using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Dialogs
{
    public interface IDialog
    {
        DialogTypeEnum DialogType
        {
            get;
        }

        void Close();
    }
}