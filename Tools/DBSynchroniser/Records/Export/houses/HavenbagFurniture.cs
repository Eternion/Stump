 


// Generated on 12/20/2015 18:16:38
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("HavenbagFurnitures")]
    [D2OClass("HavenbagFurniture", "com.ankamagames.dofus.datacenter.houses")]
    public class HavenbagFurnitureRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "HavenbagFurnitures";
        public int typeId;
        public int themeId;
        public int elementId;
        public int color;
        public int skillId;
        public int layerId;
        public Boolean blocksMovement;
        public Boolean isStackable;

        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OIgnore]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public int ThemeId
        {
            get { return themeId; }
            set { themeId = value; }
        }

        [D2OIgnore]
        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }

        [D2OIgnore]
        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        [D2OIgnore]
        public int SkillId
        {
            get { return skillId; }
            set { skillId = value; }
        }

        [D2OIgnore]
        public int LayerId
        {
            get { return layerId; }
            set { layerId = value; }
        }

        [D2OIgnore]
        public Boolean BlocksMovement
        {
            get { return blocksMovement; }
            set { blocksMovement = value; }
        }

        [D2OIgnore]
        public Boolean IsStackable
        {
            get { return isStackable; }
            set { isStackable = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (HavenbagFurniture)obj;
            
            TypeId = castedObj.typeId;
            ThemeId = castedObj.themeId;
            ElementId = castedObj.elementId;
            Color = castedObj.color;
            SkillId = castedObj.skillId;
            LayerId = castedObj.layerId;
            BlocksMovement = castedObj.blocksMovement;
            IsStackable = castedObj.isStackable;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (HavenbagFurniture)parent : new HavenbagFurniture();
            obj.typeId = TypeId;
            obj.themeId = ThemeId;
            obj.elementId = ElementId;
            obj.color = Color;
            obj.skillId = SkillId;
            obj.layerId = LayerId;
            obj.blocksMovement = BlocksMovement;
            obj.isStackable = IsStackable;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}