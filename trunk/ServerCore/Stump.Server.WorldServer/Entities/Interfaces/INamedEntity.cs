
namespace Stump.Server.WorldServer.Entities
{
    public interface INamedEntity
    {
        /// <summary>
        ///   The name of this character.
        /// </summary>
        string Name
        {
            get;
            set;
        }
    }
}