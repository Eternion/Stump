using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Dialog
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

        /// <summary>
        /// Request accepted by Target
        /// </summary>
        void AcceptDialog();

        /// <summary>
        /// Request denied by Target
        /// </summary>
        void DeniedDialog();

        /// <summary>
        /// Request cancel by Source
        /// </summary>
        void CancelDialog();
    }
}