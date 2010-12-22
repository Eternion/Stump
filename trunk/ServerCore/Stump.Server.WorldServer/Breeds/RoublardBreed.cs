using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Breeds
{
    public class RoublardBreed : BaseBreed
    {
        public override BreedEnum Id
        {
            get
            {
                return BreedEnum.Roublard;
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