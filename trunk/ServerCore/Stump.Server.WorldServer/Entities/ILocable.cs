using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public interface ILocable
    {
        /// <summary>
        ///   Representation of Entity's World Position
        /// </summary>
        VectorIsometric Position
        {
            get;
            set;
        }

        EntityDispositionInformations GetEntityDisposition();
    }
}