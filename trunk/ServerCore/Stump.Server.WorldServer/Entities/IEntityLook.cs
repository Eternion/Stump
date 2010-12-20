using System.Collections.Generic;

namespace Stump.Server.WorldServer.Entities
{
    public interface IEntityLook
    {
        int BonesId
        {
            get;
            set;
        }

        List<uint> Skins
        {
            get;
            set;
        }

        List<int> Colors
        {
            get;
            set;
        }

        List<int> ColorsIndexed
        {
            get;
        }

        int Scale
        {
            get;
            set;
        }

        List<int> Scales
        {
            get;
        }
    }
}