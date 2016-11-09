using Stump.DofusProtocol.D2oClasses.Tools.Dlm;

namespace DBSynchroniser.Interactives
{
    public class IdentifiableElement
    {
        public IdentifiableElement(DlmGraphicalElement element, DlmMap map)
        {
            Element = element;
            Map = map;
        }

        public DlmGraphicalElement Element
        {
            get;
            set;
        }
        
        public DlmMap Map
        {
            get;
            set;
        }

        public bool Ignore
        {
            get;
            set;
        }
    }
}