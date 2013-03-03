
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Hints")]
    [Serializable]
    public class Hint : IDataObject, IIndexedData
    {
        private const String MODULE = "Hints";
        public int id;
        public uint categoryId;
        public uint gfx;
        public uint nameId;
        public uint mapId;
        public uint realMapId;
        public int x;
        public int y;
        public Boolean outdoor;
        public int subareaId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public uint Gfx
        {
            get { return gfx; }
            set { gfx = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public uint RealMapId
        {
            get { return realMapId; }
            set { realMapId = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }

        public int SubareaId
        {
            get { return subareaId; }
            set { subareaId = value; }
        }

    }
}