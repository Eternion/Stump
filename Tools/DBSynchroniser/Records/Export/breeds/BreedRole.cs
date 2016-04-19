 


// Generated on 04/19/2016 10:18:06
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
    [TableName("BreedRoles")]
    [D2OClass("BreedRole", "com.ankamagames.dofus.datacenter.breeds")]
    public class BreedRoleRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "BreedRoles";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public int assetId;
        public int color;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        [D2OIgnore]
        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (BreedRole)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            AssetId = castedObj.assetId;
            Color = castedObj.color;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (BreedRole)parent : new BreedRole();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.assetId = AssetId;
            obj.color = Color;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}