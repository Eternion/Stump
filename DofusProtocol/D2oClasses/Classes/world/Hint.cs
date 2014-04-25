

// Generated on 10/28/2013 14:03:21
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Hint : IDataObject, IIndexedData
    {
        private const String MODULE = "Hints";
        public int id;
        public uint categoryId;
        public uint gfx;
        [I18NField]
        public uint nameId;
        public uint mapId;
        public uint realMapId;
        [I18NField]
        public int x;
        public int y;
        public Boolean outdoor;
        public int subareaId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        [D2OIgnore]
        public uint Gfx
        {
            get { return gfx; }
            set { gfx = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }
        [D2OIgnore]
        public uint RealMapId
        {
            get { return realMapId; }
            set { realMapId = value; }
        }
        [D2OIgnore]
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        [D2OIgnore]
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        [D2OIgnore]
        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }
        [D2OIgnore]
        public int SubareaId
        {
            get { return subareaId; }
            set { subareaId = value; }
        }
    }
}