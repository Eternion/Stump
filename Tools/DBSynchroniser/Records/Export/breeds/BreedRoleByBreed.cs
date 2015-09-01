 


// Generated on 09/01/2015 10:48:46
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
    [TableName("BreedRoleByBreeds")]
    [D2OClass("BreedRoleByBreed", "com.ankamagames.dofus.datacenter.breeds")]
    public class BreedRoleByBreedRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "BreedRoleByBreeds";
        public int breedId;
        public int roleId;
        [I18NField]
        public uint descriptionId;
        public int value;
        public int order;

        int ID2ORecord.Id
        {
            get { return (int)breedId; }
        }


        [D2OIgnore]
        [PrimaryKey("BreedId", false)]
        public int BreedId
        {
            get { return breedId; }
            set { breedId = value; }
        }

        [D2OIgnore]
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public int Value
        {
            get { return value; }
            set { value = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (BreedRoleByBreed)obj;
            
            BreedId = castedObj.breedId;
            RoleId = castedObj.roleId;
            DescriptionId = castedObj.descriptionId;
            Value = castedObj.value;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (BreedRoleByBreed)parent : new BreedRoleByBreed();
            obj.breedId = BreedId;
            obj.roleId = RoleId;
            obj.descriptionId = DescriptionId;
            obj.value = Value;
            obj.order = Order;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}