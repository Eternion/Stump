using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.XmlSerialize
{
    public class InteractiveElementSerialized : PartialLocalizable
    {
        private InteractiveElementSerialized()
        {
            
        }

        public InteractiveElementSerialized(uint mapId, InteractiveElement interactiveElement)
            : base(mapId)
        {
            InteractiveElement = interactiveElement;
        }

        public InteractiveElement InteractiveElement
        {
            get;
            set;
        }
    }
}