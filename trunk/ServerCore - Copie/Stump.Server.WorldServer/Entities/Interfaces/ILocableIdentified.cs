
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.Entities
{
    public interface ILocableIdentified : ILocable
    {
        /// <summary>
        ///   Id of this entity.
        /// </summary>
        long Id
        {
            get;
        }

        IdentifiedEntityDispositionInformations GetIdentifiedEntityDisposition();
    }
}