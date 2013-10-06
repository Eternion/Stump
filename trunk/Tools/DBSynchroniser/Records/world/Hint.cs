 


// Generated on 10/06/2013 14:22:02
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Hints")]
    [D2OClass("Hint")]
    public class HintRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Hint)obj;
            
            Id = castedObj.id;
            CategoryId = castedObj.categoryId;
            Gfx = castedObj.gfx;
            NameId = castedObj.nameId;
            MapId = castedObj.mapId;
            RealMapId = castedObj.realMapId;
            X = castedObj.x;
            Y = castedObj.y;
            Outdoor = castedObj.outdoor;
            SubareaId = castedObj.subareaId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Hint();
            obj.id = Id;
            obj.categoryId = CategoryId;
            obj.gfx = Gfx;
            obj.nameId = NameId;
            obj.mapId = MapId;
            obj.realMapId = RealMapId;
            obj.x = X;
            obj.y = Y;
            obj.outdoor = Outdoor;
            obj.subareaId = SubareaId;
            return obj;
        
        }
    }
}