using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Breeds
{
    public class ZobalBreed : BaseBreed
    {
        public override BreedEnum Id
        {
            get
            {
                return BreedEnum.Zobal;
            }
        }

        public override int StartHealthPoint
        {
            get
            {
                return 44;
            }
        }

        protected override void OnInitialize()
        {
            
        }
    }
}