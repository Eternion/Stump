
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Dialog
{
    public interface IDialogRequest
    {
        Character Source
        {
            get;
            set;
        }

        Character Target
        {
            get;
            set;
        }

        void AcceptDialog();
        void DeniedDialog();
    }
}