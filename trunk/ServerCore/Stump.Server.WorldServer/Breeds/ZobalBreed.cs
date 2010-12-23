using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Breeds
{
    public class ZobalBreed : BaseBreed
    {
        public override PlayableBreedEnum Id
        {
            get
            {
                return PlayableBreedEnum.Zobal;
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